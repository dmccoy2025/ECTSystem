
-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	5/6/2015
-- Work Item:		TFS User Story 120
-- Description:		Altered the stored procedure to get the last, first, and
--					middle names the member as individual parameters. Modified
--					the where clause to use the three new parameters instead.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	9/2/2015
-- Work Item:		TFS User Story 120
-- Description:		Reverted the changes made that added the last, first, and
--					middle names as individual parameters. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	4/28/2016
-- Description:		Reverted the changes made that added the last, first, and
--					middle names as individual parameters. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	6/28/2016
-- Description:		- Modified to no longer select cases which revolved around
--					the user calling this procedure (i.e. conducting the search). 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	7/6/2016
-- Description:		- Added the PH workflow module Id (23) to the unit scope
--					conditional check. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	9/2/2016
-- Description:		- Added an additional condition on the JOINing of the
--					core_WorkStatus_Tracking using the module to find the 
--					correct tracking records for the cases.
--					- Returns NULL for the days field when the case is complete.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	1/10/2017
-- Description:		- Modified to no use the fn_IsUserTheMember() function when
--					checking if the user and member match.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	3/28/2017
-- Description:		- Modified the procedure to now insert the data from the 
--					SELECT statement with complex JOINs into a temporary table.
--					The WHERE conditions where then moved to a select against
--					this temporary table. This was done in due to the
--					inefficient execution plan that was being created when the
--					complex JOINS and the WHERE conditions were occuring in the
--					same SELECT query. 
--					- The SELECT statement was modified to include checks for
--					which unit (case's or member's) needs to be used for the
--					user purview WHERE condition checks.
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	4/4/2017
-- Description:		Return the workflow title.
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	8/2/2017
-- Description:		Group permissions are now taken into account when
--					retrieving cases
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	8/3/2017
-- Description:		WorkStatus is now an integer
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	12/4/2017
-- Description:		o Removed the obscure rule that limited the workflows a 
--					Med Tech could search if a specific module was not selected.
--					System Admin Jenet Taylor did not know where that business
--					rule came from or why it was there.
-- ============================================================================
CREATE PROCEDURE [dbo].[form348_SC_sp_Search]
		  @caseID varchar(50)=null
		, @ssn varchar(9)=null
		, @name varchar(50)=null
		, @status INT =null
		, @userId  int
		, @rptView tinyint  
		, @compo varchar(10) =null
		, @maxCount int =null
		, @moduleId tinyint=null
		, @unitId int =null
		, @sarcpermission  bit =null
		, @overridescope bit =null
AS
BEGIN
	IF  @caseID ='' SET @caseID=null 
	IF  @ssn ='' SET @ssn=null 
	IF  @name ='' SET @name=null 	
	--IF  @lastName ='' SET @lastName=null 	
	--IF  @firstName ='' SET @firstName=null 	
	--IF  @middleName ='' SET @middleName=null 	
	IF  @compo ='' SET @compo=null 
	IF  @status =0 SET @status=null
	IF  @maxCount =0 SET @maxCount=null
	IF  @moduleId =0 SET @moduleId=null
	IF  @unitId =0 SET @unitId=null	
 
	Declare @userUnit as int, @scope tinyint, @userSSN CHAR(9), @groupId INT
	SELECT @userUnit = unit_id, @scope = accessscope, @userSSN = SSN, @groupId = r.groupId 
	from vw_Users u
	JOIN core_UserRoles r on r.userRoleID = u.currentRole  
	where u.userId=@userId	 

	--DECLARE @middleIncluded BIT = 1
	--IF @middleName IS NULL SET @middleIncluded = 0

	IF(dbo.TRIM(ISNULL(@userSSN, '')) = '')
	BEGIN
		SET @userSSN = NULL
	END

	if @overridescope is not null 
	BEGIN 
		SET @scope=3 
		SET @unitId=NULL
		SET @userSSN = NULL
	END 

	CREATE TABLE #TempSearchData
	(
		RefId INT,
		CaseId VARCHAR(50),
		Name NVARCHAR(100),
		SSN NVARCHAR(9),
		Compo CHAR(1),
		Pascode VARCHAR(4),
		WorkStatusId INT,
		WorkStatus VARCHAR(50),
		IsFinal BIT,
		WorkflowId INT,
		Workflow VARCHAR(50),
		ModuleId INT,
		DateCreated CHAR(11),
		UnitName NVARCHAR(100),
		ReceiveDate CHAR(11),
		Days INT,
		lockId INT,
		PrintId VARCHAR(50),
		SubWorkflowType INT,
		statusId INT,
		caseUnitId INT,
		memberCurrentUnitId INT,
		workflow_title NVARCHAR(50)
	)

	-- Inserting data into a temporary table due to the complex joins of this query and its inefficient interactions with the WHERE conditions which need to be done. 
	-- Place the data into a temporary table AND THEN doing a select with the complex where conditions is easier for the SQL Server to create an efficient execution
	-- plan. 
	INSERT	INTO	#TempSearchData
			SELECT	a.SC_Id as RefId,
					a.case_id  as CaseId,
					a.member_name  as Name,
					a.member_ssn AS SSN,
					a.member_compo as Compo,
					cs.PAS_CODE as Pascode,
					a.status as WorkStatusId,
					s.description as WorkStatus,
					s.isFinal as IsFinal,
					a.workflow  as WorkflowId,
					w.title as Workflow,
					w.moduleId as ModuleId,
					convert(char(11), a.created_date, 101) DateCreated,
					cs.long_name as UnitName,
					convert(char(11), ISNULL(t.ReceiveDate, a.created_date), 101) ReceiveDate,
					CASE s.isFinal WHEN 0 THEN DATEDIFF(D, ISNULL(t.ReceiveDate, a.created_date), GETDATE()) ELSE NULL END AS Days,
					ISNULL(l.id,0) lockId,
					Convert(varchar, a.SC_Id) + ', ' + CONVERT(varchar, w.moduleId) As PrintId,
					sub_workflow_type as Sub_Type,
					s.statusId,
					a.member_unit_id,
					mCS.CS_ID,
					(CASE a.sub_workflow_type WHEN 0 Then w.title ELSE sub.subTypeTitle END) AS workflow_title
			FROM	Form348_sc  a
					JOIN core_WorkStatus ws ON ws.ws_id = a.status
					JOIN core_StatusCodes s ON s.statusId = ws.statusId
					JOIN core_workflow w ON w.workflowId = a.workflow
					JOIN COMMAND_STRUCT CS ON cs.cs_id = a.member_unit_id 
					LEFT JOIN MemberData md ON a.member_ssn = md.SSAN
					LEFT JOIN Command_Struct mCS ON md.PAS_NUMBER = mCS.PAS_CODE
					LEFT JOIN core_workflowLocks l on l.refId = a.sc_id AND (l.module = IsNull(@moduleId,l.module))
					LEFT JOIN (SELECT max(startDate) ReceiveDate, refId, module FROM core_WorkStatus_Tracking GROUP BY refId, module) t ON t.refId = a.SC_Id AND t.module = w.moduleId
					LEFT JOIN core_lkupSCSubType sub ON sub.subTypeId = a.sub_workflow_type
					JOIN core_WorkflowPermissions wp ON wp.workflowId = a.workflow
					JOIN core_GroupPermissions gp ON gp.permId = wp.permId
					JOIN core_Permissions p ON p.permId = wp.permId
			WHERE	s.moduleId = ISNULL(@moduleId, s.moduleId)
					AND gp.groupId = @groupId
					AND gp.permId NOT IN (37, 38, 39)
					AND p.permName LIKE '%View%'


	SELECT	r.RefId,
			r.CaseId,
			r.Name,
			RIGHT(r.SSN, 4) AS SSN,
			r.Compo,
			r.Pascode,
			r.WorkStatusId,
			r.WorkStatus,
			r.IsFinal,
			r.WorkflowId,
			r.Workflow,
			r.ModuleId,
			r.DateCreated,
			r.UnitName,
			r.ReceiveDate,
			r.Days,
			r.lockId,
			r.PrintId,
			r.SubWorkflowType AS Sub_Type,
			r.workflow_title
	FROM	#TempSearchData r
	WHERE	r.Name LIKE '%' + ISNULL(@Name,r.Name) + '%'
			AND r.CaseId LIKE ISNULL(@caseId,r.CaseId) + '%'
			AND r.SSN LIKE '%' + ISNULL(@ssn,r.SSN)
			AND r.Compo LIKE '%' + ISNULL(@compo,r.Compo) + '%'
			AND ISNULL(@status, r.WorkStatusId) = r.WorkStatusId
			AND ISNULL(@unitId, CASE r.IsFinal WHEN 1 THEN r.memberCurrentUnitId ELSE r.caseUnitId END) = (CASE r.IsFinal WHEN 1 THEN r.memberCurrentUnitId ELSE r.caseUnitId END)
			--m.LAST_NAME LIKE '%' + IsNull(@lastName, m.LAST_NAME) + '%'
			--AND m.FIRST_NAME LIKE '%' + IsNull(@firstName, m.FIRST_NAME) + '%'
			--AND
			--(
			--	(
			--		@middleIncluded = 1
			--		AND m.MIDDLE_NAMES LIKE '%' + IsNull(@middleName, m.MIDDLE_NAMES) + '%'
			--	)
			--	OR
			--	(
			--		@middleIncluded = 0
			--		AND 1 = 1
			--	)
			--)
 			AND  
			(
				-- global scope
				(@scope = 3)
				OR
				-- component scope
				(@scope = 2)
				OR
				-- unit scope
				(
					@scope = 1  
					AND
					(
						(CASE r.IsFinal WHEN 1 THEN r.memberCurrentUnitId ELSE r.caseUnitId END) IN
						(	
							SELECT child_id FROM Command_Struct_Tree WHERE parent_id=@userUnit and view_type=@rptView 
						)
					)
					AND	dbo.fn_IsUserTheMember(@userId, r.SSN) = 0	-- Don't return cases which revolve around the user doing the search...
				)
			)
	ORDER	BY r.CaseId
END
GO

