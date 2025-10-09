
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 1/17/2017
-- Description:	Performs a seach of the SARC Appeal Request cases.
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
-- Modified By:		Ken Barnett
-- Modified Date:	6/9/2017
-- Description:		- Modified to use the APSA cases member info instead of
--					the LODs/SARCs member info.
--					- Fixed a bug where the search was only selecting from 
--					SARC appeals against LOD cases instead of also selecting
--					from Restricted SARCs.
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	7/24/2017
-- Description:		Workstatus now takes in an integer
-- ============================================================================
CREATE PROCEDURE [dbo].[form348_AP_SARC_sp_Search]
		  @caseID varchar(50)= null
		, @ssn varchar(9)= null
		, @name varchar(50)=null
		, @status INT = null
		, @userId int
		, @rptView tinyint  
		, @compo varchar(10) = null
		, @maxCount int = null
		, @moduleId tinyint= null
		, @unitId int = null
		, @overridescope bit = null
AS
BEGIN
	IF  @caseID = '' SET @caseID=null 
	IF  @ssn = '' SET @ssn=null 
	IF  @name ='' SET @name=null 	
	IF  @compo = '' SET @compo=null 
	IF  @status = 0 SET @status=null
	IF  @maxCount = 0 SET @maxCount=null
	IF  @moduleId = 0 SET @moduleId=null
	IF  @unitId = 0 SET @unitId=null	
 
	DECLARE @userUnit AS INT, @scope TINYINT
	SELECT @userUnit = unit_id, @scope = accessscope FROM vw_Users WHERE userId=@userId	 
 
	IF @overridescope IS NOT NULL
	BEGIN 
		SET @scope=3 
		SET @unitId=NULL
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
		statusId INT,
		IsPostProcessingComplete BIT,
		caseUnitId INT,
		memberCurrentUnitId INT
	)

	-- Inserting data into a temporary table due to the complex joins of this query and its inefficient interactions with the WHERE conditions which need to be done. 
	-- Place the data into a temporary table AND THEN doing a select with the complex where conditions is easier for the SQL Server to create an efficient execution
	-- plan. 
	INSERT	INTO	#TempSearchData
			SELECT	sarc.appeal_sarc_id AS RefId,
					sarc.Case_Id AS CaseId,
					sarc.member_name AS Name,
					sarc.member_ssn AS SSN,
					sarc.member_compo AS Compo,
					cs.PAS_CODE AS Pascode,
					sarc.status AS WorkStatusId,
					s.description AS WorkStatus,
					s.isFinal AS IsFinal,
					sarc.workflow AS WorkflowId,
					w.title AS Workflow,
					w.moduleId AS ModuleId,
					CONVERT(CHAR(11), sarc.created_date, 101) DateCreated,
					cs.long_name AS UnitName,
					CONVERT(CHAR(11), ISNULL(t.ReceiveDate, sarc.created_date), 101) ReceiveDate,
					CASE s.isFinal WHEN 0 THEN DATEDIFF(D, ISNULL(t.ReceiveDate, sarc.created_date), GETDATE()) ELSE NULL END AS Days,
					ISNULL(l.id,0) AS lockId,
					CONVERT(VARCHAR, sarc.appeal_sarc_id) + ', ' + CONVERT(VARCHAR, w.moduleId) AS PrintId,
					s.statusId,
					sarc.is_post_processing_complete,
					sarc.member_unit_id,
					mCS.CS_ID
			FROM	Form348_AP_SARC sarc
					JOIN Form348 b ON b.lodid = sarc.initial_id AND b.workflow = sarc.initial_workflow
					JOIN core_WorkStatus ws ON ws.ws_id = sarc.status
					JOIN core_StatusCodes s ON s.statusId = ws.statusId
					JOIN core_workflow w ON w.workflowId = sarc.workflow
					JOIN COMMAND_STRUCT CS ON cs.cs_id = sarc.member_unit_id 
					LEFT JOIN MemberData md ON sarc.member_ssn = md.SSAN
					LEFT JOIN Command_Struct mCS ON md.PAS_NUMBER = mCS.PAS_CODE
					LEFT JOIN core_workflowLocks l ON l.refId = sarc.appeal_sarc_id AND l.module = (SELECT moduleId FROM core_lkupModule WHERE moduleName = 'Restricted SARC Appeal')
					LEFT JOIN (SELECT MAX(startDate) ReceiveDate, ws_id, refId FROM core_WorkStatus_Tracking GROUP BY ws_id, refId) t ON t.refId = sarc.appeal_sarc_id AND t.ws_id = sarc.status
			WHERE	s.moduleId = ISNULL(@moduleId, s.moduleId)


	INSERT	INTO	#TempSearchData
			SELECT	sarc.appeal_sarc_id AS RefId,
					sarc.Case_Id AS CaseId,
					sarc.member_name AS Name,
					sarc.member_ssn AS SSN,
					sarc.member_compo AS Compo,
					cs.PAS_CODE AS Pascode,
					sarc.status AS WorkStatusId,
					s.description AS WorkStatus,
					s.isFinal AS IsFinal,
					sarc.workflow AS WorkflowId,
					w.title AS Workflow,
					w.moduleId AS ModuleId,
					CONVERT(CHAR(11), sarc.created_date, 101) DateCreated,
					cs.long_name AS UnitName,
					CONVERT(CHAR(11), ISNULL(t.ReceiveDate, sarc.created_date), 101) ReceiveDate,
					CASE s.isFinal WHEN 0 THEN DATEDIFF(D, ISNULL(t.ReceiveDate, sarc.created_date), GETDATE()) ELSE NULL END AS Days,
					ISNULL(l.id,0) AS lockId,
					CONVERT(VARCHAR, sarc.appeal_sarc_id) + ', ' + CONVERT(VARCHAR, w.moduleId) AS PrintId,
					s.statusId,
					sarc.is_post_processing_complete,
					sarc.member_unit_id,
					mCS.CS_ID
			FROM	Form348_AP_SARC sarc
					JOIN Form348_SARC b ON b.sarc_id = sarc.initial_id AND b.workflow = sarc.initial_workflow
					JOIN core_WorkStatus ws ON ws.ws_id = sarc.status
					JOIN core_StatusCodes s ON s.statusId = ws.statusId
					JOIN core_workflow w ON w.workflowId = sarc.workflow
					JOIN COMMAND_STRUCT CS ON cs.cs_id = sarc.member_unit_id 
					LEFT JOIN MemberData md ON sarc.member_ssn = md.SSAN
					LEFT JOIN Command_Struct mCS ON md.PAS_NUMBER = mCS.PAS_CODE
					LEFT JOIN core_workflowLocks l ON l.refId = sarc.appeal_sarc_id AND l.module = (SELECT moduleId FROM core_lkupModule WHERE moduleName = 'Restricted SARC Appeal')
					LEFT JOIN (SELECT MAX(startDate) ReceiveDate, ws_id, refId FROM core_WorkStatus_Tracking GROUP BY ws_id, refId) t ON t.refId = sarc.appeal_sarc_id AND t.ws_id = sarc.status
			WHERE	s.moduleId = ISNULL(@moduleId, s.moduleId)


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
			r.PrintId
	FROM	#TempSearchData r
	WHERE	r.Name LIKE '%' + ISNULL(@Name,r.Name) + '%'
			AND r.CaseId LIKE ISNULL(@caseId,r.CaseId) + '%'
			AND r.SSN LIKE '%' + ISNULL(@ssn,r.SSN)
			AND r.Compo LIKE '%' + ISNULL(@compo,r.Compo) + '%'
			AND ISNULL(@status, r.WorkStatusId) = r.WorkStatusId
			AND ISNULL(@unitId, CASE r.IsPostProcessingComplete WHEN 1 THEN r.memberCurrentUnitId ELSE r.caseUnitId END) = (CASE r.IsPostProcessingComplete WHEN 1 THEN r.memberCurrentUnitId ELSE r.caseUnitId END)
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
						CASE r.IsPostProcessingComplete WHEN 1 THEN r.memberCurrentUnitId ELSE r.caseUnitId END IN
						(	
							SELECT child_id FROM Command_Struct_Tree WHERE parent_id=@userUnit AND view_type=@rptView 
						) 
					)
					AND dbo.fn_IsUserTheMember(@userId, r.SSN) = 0	-- Don't return cases which revolve around the user doing the search...
				)
			)
	ORDER	BY r.CaseId
END
GO

