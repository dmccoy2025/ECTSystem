-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	Performs a save app auth user id search on Form348 table (Pagination version)
-- ============================================================================
-- Modified By:		AI Assistant
-- Modified Date:	10/11/2025
-- Description:		Added pagination parameters @PageNumber and @PageSize, and applied OFFSET FETCH to the final SELECT.
-- ============================================================================
CREATE PROCEDURE [dbo].[form348_sp_SaveAppAuthUserId_pagination]
		  @caseID VARCHAR(50) = null
		, @ssn VARCHAR(9) = null
		, @name VARCHAR(50) = null
		, @statusCode INT = null
		, @userId  INT
		, @rptView TINYINT
		, @compo VARCHAR(10) = null
		, @maxCount INT = null
		, @moduleId TINYINT = null
		, @isFormal BIT = null
		, @unitId INT = null
		, @sarcpermission BIT = null
		, @includeDeleted BIT = null
		, @overridescope BIT = null
		, @PageNumber INT = 1
		, @PageSize INT = 50
AS
BEGIN
	IF  @caseID = '' SET @caseID = null 
	IF  @ssn = '' SET @ssn = null 
	IF  @name = '' SET @name = null 	
	IF  @compo = '' SET @compo = null 
	IF  @statusCode = 0 SET @statusCode = null
	IF  @maxCount = 0 SET @maxCount = null
	IF  @moduleId = 0 SET @moduleId = null		 
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
		IsFormal BIT,
		DateCreated CHAR(11),
		UnitName NVARCHAR(100),
		ReceiveDate CHAR(11),
		Days INT,
		sarc BIT,
		lockId INT,
		deleted BIT,
		statusId INT,
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
					a.member_ssn AS SSN,
					a.member_compo AS Compo,
					cs.PAS_CODE AS Pascode,
					a.status AS WorkStatusId,
					s.description AS WorkStatus,
					s.isFinal AS IsFinal,
					a.workflow AS WorkflowId,
					w.title AS Workflow,
					w.moduleId AS ModuleId,
					a.formal_inv  AS IsFormal,
					CONVERT(CHAR(11), a.created_date, 101) DateCreated,
					cs.long_name as UnitName,
					CONVERT(CHAR(11), ISNULL(t.ReceiveDate, a.created_date), 101) ReceiveDate,
					CASE s.isFinal WHEN 0 THEN DATEDIFF(D, ISNULL(t.ReceiveDate, a.created_date), GETDATE()) ELSE NULL END AS Days,
					a.sarc,
					ISNULL(l.id,0) lockId,
					a.deleted,
					s.statusId,
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
					LEFT JOIN core_workflowLocks l ON l.refId = a.lodId AND l.module = @moduleId
					LEFT JOIN (SELECT MAX(startDate) ReceiveDate, refId, module FROM core_WorkStatus_Tracking GROUP BY refId, module) t ON t.refId = a.lodId AND t.module = w.moduleId
			WHERE	a.restricted = 0
					AND s.moduleId = @moduleId
	
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
			r.IsFormal, 
			r.DateCreated, 
			r.UnitName, 
			r.ReceiveDate, 
			r.Days, 
			r.sarc, 
			r.lockId
	FROM	#TempSearchData r
	WHERE	r.Name LIKE '%' + ISNULL(@Name,r.Name) + '%'
			AND r.CaseId LIKE ISNULL(@caseId,r.CaseId) + '%' 
			AND r.SSN LIKE '%' + ISNULL(@ssn,r.SSN)
			AND r.Compo LIKE '%' + ISNULL(@compo,r.Compo) + '%'
			AND ISNULL(@statusCode, r.statusId) = r.statusId
			AND ISNULL(@isFormal, r.IsFormal) = r.IsFormal
			AND ISNULL(@includeDeleted, r.deleted) = r.deleted
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
					AND	dbo.fn_IsUserTheMember(@userId, r.SSN) = 0
				)
			)
	ORDER	BY r.CaseId
	OFFSET (@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
END
GO