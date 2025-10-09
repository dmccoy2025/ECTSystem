
-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	Conducts a search to determine which LOD cases are in a user's
--				post completion cases to be worked queue.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	11/27/2015
-- Work Item:		TFS Task 289
-- Description:		Changed the size of @caseID from 10 to 50.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	5/5/2016
-- Description:		Added check for LOD Determination - Death (ILOD) memo. 
--					Cleaned up the stored procedure.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	5/12/2016
-- Description:		Modified the determination memo check to group the checks 
--					for the ILOD and ILOD Death memo's into one statement. This
--					was done to maintain backwards compatbility for older LOD
--					cases process before the introduction of the ILOD Death 
--					memo.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	7/29/2016
-- Description:		Updated to no longer use a hard coded value for the LOD
--					cancel status.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	1/10/2017
-- Description:		- Modified to no longer select cases which revolved around
--					the user calling this procedure (i.e. conducting the search).
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	1/26/2017
-- Description:		- Modified to no longer select restricted SARC cases.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	3/29/2017
-- Description:		- Cleaned up procedure. 
--					- Updated the selection statement to use the new
--					is_post_processing_complete flag instead of manually 
--					checking if post processing is completed. 
--					- Modified the procedure to now insert the data from the 
--					SELECT statement with complex JOINs into a temporary table.
--					The WHERE conditions where then moved to a select against
--					this temporary table. This was done in due to the
--					inefficient execution plan that was being created when the
--					complex JOINS and the WHERE conditions were occuring in the
--					same SELECT query. 
-- ============================================================================
-- Modified By:		Eric Kelley
-- Modified Date:	06/22/2021
-- Description:		Modified to give option to filter by WorkStatusId and Compo
-- ============================================================================
CREATE PROCEDURE [dbo].[form348_sp_PostCompletionSearch]
	 @caseID VARCHAR(50) = null
	,@ssn VARCHAR(10) = null
	,@name VARCHAR(10) = null
	,@userId INT
	,@rptView TINYINT 
	,@compo VARCHAR(10) = null
	,@maxCount INT = null
	,@moduleId TINYINT = null
	,@isFormal BIT = null
	,@unitId INT = null
	,@sarcpermission BIT = null
	,@searchAllCases BIT = null
	,@wsId int
AS

BEGIN
	IF @caseID = '' SET @caseID = null 
	IF @ssn = '' SET @ssn = null 
	IF @name = '' SET @name = null 	
	IF @compo = '' SET @compo = dbo.GetCompoForUser(@userId) 
	IF @maxCount = 0 SET @maxCount = null
	IF @moduleId = 0 SET @moduleId = null		 
	IF @unitId = 0 SET @unitId = NULL
	IF @sarcpermission IS NULL SET @sarcpermission = 0
	IF @searchAllCases IS NULL SET @searchAllCases = 0

	--SET @Compo = dbo.GetCompoForUser(@userId)

	DECLARE @userUnit INT, @groupId TINYINT, @scope TINYINT
	SELECT @userUnit = unit_id, @groupId = groupId, @scope = accessscope FROM vw_Users WHERE userId = @userId

	DECLARE @unitView INT = 1
	SELECT @unitView = unitView FROM core_Users WHERE userID = @userId

	CREATE TABLE #TempSearchData
	(
		RefId INT,
		CaseId VARCHAR(50),
		Name NVARCHAR(100),
		SSN NVARCHAR(9),
		Compo CHAR(1),
		Pascode VARCHAR(4),
		WorkStatusId INT,
		StatusDescription VARCHAR(50),
		IsFinal BIT,
		WorkflowId INT,
		Workflow VARCHAR(50),
		ModuleId INT,
		IsFormal BIT,
		DateCreated CHAR(11),
		UnitName NVARCHAR(100),
		ReceiveDate CHAR(11),
		DaysCompleted INT,
		memberNotified INT,
		lockId INT,
		sarc BIT,
		deleted BIT,
		statusId INT,
		IsPostProcessingComplete BIT,
		IsCancel BIT,
		restricted BIT,
		caseUnitId INT,
		isAttachPAS BIT,
		member_attached_unit_id INT,
		rawReceiveDate DATETIME
	)

	-- Inserting data into a temporary table due to the complex joins of this query and its inefficient interactions with the WHERE conditions which need to be done. 
	-- Place the data into a temporary table AND THEN doing a select with the complex where conditions is easier for the SQL Server to create an efficient execution
	-- plan. 
	If (@wsId is null or @wsId = 0)
	BEGIN
	INSERT	INTO	#TempSearchData
			SELECT	a.lodId AS RefId,
					a.case_id AS CaseId,
					a.member_name AS Name,
					a.member_ssn AS SSN,
					a.member_compo AS Compo,
					cs.PAS_CODE AS Pascode,
					a.status AS WorkStatusId,
					ws.description AS StatusDescription,
					ws.isFinal AS IsFinal,
					a.workflow AS WorkflowId,
					w.title AS Workflow,
					w.moduleId AS ModuleId,
					a.formal_inv AS IsFormal,
					CONVERT(CHAR(11), a.created_date, 101) DateCreated,
					cs.long_name + ' ('+ cs.PAS_CODE  +')' AS UnitName,
					CONVERT(CHAR(11), ISNULL(t.ReceiveDate, a.created_date), 101) ReceiveDate,
					DATEDIFF(d,  t.ReceiveDate , GETDATE()) DaysCompleted ,
					a.memberNotified,
					ISNULL(l.id,0) lockId,
					a.sarc,
					a.deleted,
					ws.statusId,
					a.is_post_processing_complete,
					ws.isCancel,
					a.restricted,
					a.member_unit_id,
					a.isAttachPAS,
					a.member_attached_unit_id,
					t.ReceiveDate
			FROM	Form348 a
					JOIN vw_workstatus ws ON ws.ws_id = a.status 
					JOIN Form348_Medical med ON med.lodid = a.lodid 	
					JOIN core_workflow w ON w.workflowId = a.workflow
					JOIN COMMAND_STRUCT CS ON cs.cs_id = a.member_unit_id 
					LEFT JOIN core_workflowLocks l ON l.refId = a.lodId AND l.module = @moduleId
					LEFT JOIN (SELECT MAX(startDate) ReceiveDate, ws_id, refId FROM	core_WorkStatus_Tracking GROUP BY ws_id, refId) t ON t.ws_id = a.status AND t.refId = a.lodId 
			
			WHERE	ws.isFinal = 1
					AND ws.isCancel = 0
					AND a.is_post_processing_complete = 0
		END

	ELSE
	BEGIN
	INSERT	INTO	#TempSearchData
			SELECT	a.lodId AS RefId,
					a.case_id AS CaseId,
					a.member_name AS Name,
					a.member_ssn AS SSN,
					a.member_compo AS Compo,
					cs.PAS_CODE AS Pascode,
					a.status AS WorkStatusId,
					ws.description AS StatusDescription,
					ws.isFinal AS IsFinal,
					a.workflow AS WorkflowId,
					w.title AS Workflow,
					w.moduleId AS ModuleId,
					a.formal_inv AS IsFormal,
					CONVERT(CHAR(11), a.created_date, 101) DateCreated,
					cs.long_name + ' ('+ cs.PAS_CODE  +')' AS UnitName,
					CONVERT(CHAR(11), ISNULL(t.ReceiveDate, a.created_date), 101) ReceiveDate,
					DATEDIFF(d,  t.ReceiveDate , GETDATE()) DaysCompleted ,
					a.memberNotified,
					ISNULL(l.id,0) lockId,
					a.sarc,
					a.deleted,
					ws.statusId,
					a.is_post_processing_complete,
					ws.isCancel,
					a.restricted,
					a.member_unit_id,
					a.isAttachPAS,
					a.member_attached_unit_id,
					t.ReceiveDate
			FROM	Form348 a
					JOIN vw_workstatus ws ON ws.ws_id = a.status 
					JOIN Form348_Medical med ON med.lodid = a.lodid 	
					JOIN core_workflow w ON w.workflowId = a.workflow
					JOIN COMMAND_STRUCT CS ON cs.cs_id = a.member_unit_id 
					LEFT JOIN core_workflowLocks l ON l.refId = a.lodId AND l.module = @moduleId
					LEFT JOIN (SELECT MAX(startDate) ReceiveDate, ws_id, refId FROM	core_WorkStatus_Tracking GROUP BY ws_id, refId) t ON t.ws_id = a.status AND t.refId = a.lodId 
			
			WHERE	ws.isFinal = 1
					AND ws.isCancel = 0
					AND a.is_post_processing_complete = 0
					AND a.status = @wsId
		END

	SELECT	r.RefId,
			r.CaseId,
			r.Name,
			RIGHT(r.SSN, 4) AS SSN,
			r.Pascode,
			r.WorkStatusId,
			r.StatusDescription,
			r.IsFinal,
			r.WorkflowId,
			r.Workflow,
			r.ModuleId,
			r.IsFormal,
			r.DateCreated,
			r.UnitName,
			r.ReceiveDate,
			r.DaysCompleted,
			r.memberNotified,
			r.lockId
	FROM	#TempSearchData r
	WHERE	r.Name LIKE '%' + ISNULL(@Name,r.Name) + '%'
			AND r.CaseId LIKE ISNULL(@caseId,r.CaseId) + '%' 
			AND r.SSN LIKE '%' + ISNULL(@ssn,r.SSN)
			AND r.Compo LIKE '%' + ISNULL(@compo,r.Compo) + '%'
			AND ISNULL(@isFormal, r.IsFormal) = r.IsFormal
			AND
			(
				@scope > 1
				OR
				dbo.fn_IsUserTheMember(@userId, r.SSN) = 0	-- Don't return cases which revolve around the user doing the search...
			)
			AND		      	
			(
				(@searchAllCases=0 AND r.rawReceiveDate > '2010-01-29')
				OR 
				(@searchAllCases=1)
			)
			AND
			(
				r.sarc = 0
				OR
				(
					r.sarc = 1
					AND r.restricted = 0
				)
			)
			AND
			(
				CASE 
					WHEN (r.sarc = 1 AND r.restricted = 1) THEN 1  
					ELSE @sarcpermission	
				END
				= @sarcpermission
			)
			AND
			(
				ISNULL(@unitId, r.caseUnitId) = r.caseUnitId
				OR
				(
					r.isAttachPAS = 1
					AND
					ISNULL(@unitId, r.member_attached_unit_id) = r.member_attached_unit_id
				)
			)	
			AND
			(
				(
					@unitView = 1
					AND
					(
						r.caseUnitId IN
						(
							SELECT child_id FROM Command_Struct_Tree WHERE parent_id = @userUnit AND view_type = @rptView
						)
						OR
						(
							r.isAttachPAS = 1
							AND
							r.member_attached_unit_id IN (SELECT child_id FROM Command_Struct_Tree WHERE parent_id=@userUnit and view_type=@rptView )
						)
					)
				)
				OR
				(
					@unitView = 0
					AND
					(
						r.caseUnitId = @userUnit
						OR
						(
							r.isAttachPAS = 1
							AND
							r.member_attached_unit_id  IN (SELECT child_id FROM Command_Struct_Tree WHERE parent_id=@userUnit and view_type=@rptView )
						)
					)
				)
			) 
	ORDER	BY r.rawReceiveDate
END

--[dbo].[form348_sp_PostCompletionSearch] null, null ,null , 123, 2,'6', 0, 2, null, 0, false, false, 330
GO

