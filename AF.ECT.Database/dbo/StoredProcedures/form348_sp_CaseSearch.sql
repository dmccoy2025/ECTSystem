
-- ============================================================================
-- Author:			Evan Morrison
-- Create date:		9/15/2016
-- Description:		Get LOD and Appeal cases
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	1/10/2017
-- Description:		- Modified to no use the fn_IsUserTheMember() function when
--					checking if the user and member match.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	1/27/2017
-- Description:		- Modified to select the create_date time in addition to
--					the date portion. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	2/23/2017
-- Description:		Removed the ReceivedFrom field from being selected.
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
-- Modified Date:	6/06/2017
-- Description:		Modified the CreatedDate for the Appeal cases to include
--					the time in addition to the date.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	6/9/2017
-- Description:		- Modified to use the AP cases member info instead of
--					the LODs member info.
-- ============================================================================
CREATE PROCEDURE [dbo].[form348_sp_CaseSearch]
		  @ssn VARCHAR(9) = null
		, @userId  INT
		, @rptView TINYINT
		, @compo VARCHAR(10) = null	 
		, @unitId INT = null
		, @sarcpermission BIT = null
		, @includeDeleted BIT = null
		, @overridescope BIT = null
AS
BEGIN
	IF  @ssn = '' SET @ssn = null 	
	IF  @compo = '' SET @compo = null 	 
	IF  @unitId = 0 SET @unitId = NULL
	 
	SET @includeDeleted = COALESCE(@includeDeleted, 0);

	DECLARE @userUnit AS INT, @scope TINYINT, @userSSN CHAR(9)

	SELECT	@userUnit = u.unit_id, @scope = u.accessScope, @userSSN = u.SSN
	FROM	vw_Users u 
	WHERE	userId=@userId
	
	IF(dbo.TRIM(ISNULL(@userSSN, '')) = '')
	BEGIN
		SET @userSSN = NULL
	END

	IF (@overridescope IS NOT NULL)
	BEGIN 
		SET @scope = 3 
		SET @unitId = NULL
		SET @userSSN = NULL
	END 

	DECLARE @LODmoduleId AS INT = 2,
			@APmoduleId AS INT = 24

	CREATE TABLE #TempSearchData
	(
		RefId INT,
		CaseId VARCHAR(50),
		Name VARCHAR(100),
		SSN INT,
		Compo INT,
		WorkStatusId INT,
		WorkStatus VARCHAR(50),
		IsFinal INT,
		WorkflowId INT,
		Workflow VARCHAR(50),
		ModuleId INT,
		IsFormal INT,
		DateCreated DATETIME,
		UnitName VARCHAR(100),
		ReceiveDate DATETIME,
		Days INT,
		sarc INT,
		restricted BIT,
		lockId INT,
		deleted BIT,
		IsPostProcessingComplete BIT,
		caseUnitId INT,
		memberCurrentUnitId INT,
		isAttachPAS BIT,
		member_attached_unit_id INT
	)

	INSERT	INTO	#TempSearchData
			SELECT	a.lodId AS RefId,
					a.case_id  AS CaseId,
					a.member_name  AS Name,
					a.member_ssn,
					a.member_compo AS Compo,
					a.status AS WorkStatusId,
					s.description AS WorkStatus,
					s.isFinal AS IsFinal,
					a.workflow AS WorkflowId,
					w.title AS Workflow,
					w.moduleId AS ModuleId,
					a.formal_inv  AS IsFormal,
					CONVERT(CHAR(11), a.created_date, 101) + ' ' + CONVERT(CHAR(25), a.created_date, 108) DateCreated,
					cs.long_name as UnitName,
					CONVERT(CHAR(11), ISNULL(t.ReceiveDate, a.created_date), 101) ReceiveDate,
					CASE s.isFinal WHEN 0 THEN DATEDIFF(D, ISNULL(t.ReceiveDate, a.created_date), GETDATE()) ELSE NULL END AS Days,
					a.sarc,
					a.restricted,
					ISNULL(l.id,0) lockId,
					a.deleted,
					a.is_post_processing_complete,
					a.member_unit_id,
					mCS.CS_ID,
					a.isAttachPAS,
					a.member_attached_unit_id
			FROM	Form348  a
					JOIN core_WorkStatus ws ON ws.ws_id = a.status
					JOIN core_StatusCodes s ON s.statusId = ws.statusId
					JOIN core_workflow w ON w.workflowId = a.workflow
					JOIN COMMAND_STRUCT CS ON cs.cs_id = a.member_unit_id 
					JOIN Form348_Medical fm ON fm.lodid = a.lodid
					LEFT JOIN MemberData md ON a.member_ssn = md.SSAN
					LEFT JOIN Command_Struct mCS ON md.PAS_NUMBER = mCS.PAS_CODE
					LEFT JOIN core_workflowLocks l ON l.refId = a.lodId and l.module = @LODmoduleId
					LEFT JOIN (SELECT max(startDate) ReceiveDate, refId, module FROM core_WorkStatus_Tracking GROUP BY refId, module) t ON t.refId = a.lodId AND t.module = w.moduleId
			WHERE	a.member_ssn LIKE '%' + IsNull(@ssn,a.member_ssn)
					AND s.moduleId = @LODmoduleId


	INSERT	INTO	#TempSearchData
			SELECT	ap.appeal_id AS RefId,
					ap.case_id  AS CaseId,
					ap.member_name  AS Name,
					ap.member_ssn,
					ap.member_compo AS Compo,
					ap.status AS WorkStatusId,
					s.description AS WorkStatus,
					s.isFinal AS IsFinal,
					ap.workflow AS WorkflowId,
					w.title AS Workflow,
					w.moduleId AS ModuleId,
					f.formal_inv  AS IsFormal,
					CONVERT(CHAR(11), ap.created_date, 101) + ' ' + CONVERT(CHAR(25), ap.created_date, 108) DateCreated,
					cs.long_name as UnitName,
					CONVERT(CHAR(11), ISNULL(t.ReceiveDate, ap.created_date), 101) ReceiveDate,
					CASE s.isFinal WHEN 0 THEN DATEDIFF(D, ISNULL(t.ReceiveDate, ap.created_date), GETDATE()) ELSE NULL END AS Days,
					f.sarc,
					f.restricted,
					ISNULL(l.id,0) lockId,
					f.deleted,
					ap.is_post_processing_complete,
					ap.member_unit_id,
					mCS.CS_ID,
					0,
					0
			FROM	Form348_AP  ap
					JOIN Form348 f ON f.lodId = ap.initial_lod_id
					JOIN core_WorkStatus ws ON ws.ws_id = ap.status
					JOIN core_StatusCodes s ON s.statusId = ws.statusId
					JOIN core_workflow w ON w.workflowId = ap.workflow
					JOIN COMMAND_STRUCT CS ON cs.cs_id = ap.member_unit_id 
					JOIN Form348_Medical fm ON fm.lodid = ap.initial_lod_id
					LEFT JOIN MemberData md ON ap.member_ssn = md.SSAN
					LEFT JOIN Command_Struct mCS ON md.PAS_NUMBER = mCS.PAS_CODE
					LEFT JOIN core_workflowLocks l ON l.refId = ap.initial_lod_id and l.module = @APmoduleId
					LEFT JOIN (SELECT max(startDate) ReceiveDate, refId, module FROM core_WorkStatus_Tracking GROUP BY refId, module) t ON t.refId = ap.appeal_id AND t.module = w.moduleId
			WHERE	ap.member_ssn LIKE '%' + IsNull(@ssn,ap.member_ssn)
					AND s.moduleId = @APmoduleId


	SELECT	r.RefId,
			r.CaseId,  
			r.Name,
			RIGHT(r.SSN, 4) AS SSN,
			r.Compo,
			r.WorkStatusId,
			r.WorkStatus,
			r.IsFinal,
			r.WorkflowId,
			r.Workflow,
			r.ModuleId,
			r.IsFormal,
			r.DateCreated,
			r.UnitName,
			r.ReceiveDate,
			r.Days,
			r.sarc,
			r.lockId
	FROM	#TempSearchData r
	WHERE	ISNULL(@includeDeleted, r.deleted) = r.deleted
			AND r.Compo LIKE '%' + ISNULL(@compo,r.Compo) + '%'
			AND
			(
				CASE 
					WHEN (r.sarc = 1 AND r.restricted=1) Then 1  
					ELSE @sarcpermission	
				END
				= @sarcpermission
			)
			AND
			(
				ISNULL(@unitId, CASE r.IsPostProcessingComplete WHEN 1 THEN r.memberCurrentUnitId ELSE r.caseUnitId END) = (CASE r.IsPostProcessingComplete WHEN 1 THEN r.memberCurrentUnitId ELSE r.caseUnitId END)
				OR
				(
					r.IsPostProcessingComplete = 0
					AND
					r.isAttachPAS = 1
					AND
					ISNULL(@unitId, r.member_attached_unit_id) = r.member_attached_unit_id
				)
			)
			AND
			(
				(@scope = 3)
				OR
				(@scope = 2)
				OR
				(	
					@scope = 1
					AND
					(
						(
							CASE r.IsPostProcessingComplete WHEN 1 THEN r.memberCurrentUnitId ELSE r.caseUnitId END IN (SELECT child_id FROM Command_Struct_Tree WHERE parent_id=@userUnit AND view_type=@rptView) 
							OR
							(
								r.IsPostProcessingComplete = 0
								AND
								r.isAttachPAS = 1
								AND 
								r.member_attached_unit_id IN (SELECT child_id FROM Command_Struct_Tree WHERE parent_id=@userUnit AND view_type=@rptView)
							)
						)
					)
					AND	dbo.fn_IsUserTheMember(@userId, r.SSN) = 0	-- Don't return cases which revolve around the user doing the search...
				)
			)
	ORDER	BY r.CaseId
END
GO

