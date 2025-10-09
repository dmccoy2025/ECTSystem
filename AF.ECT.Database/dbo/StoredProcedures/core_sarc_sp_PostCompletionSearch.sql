
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 12/1/2016
-- Description:	Returns the number of Restricted SARC cases in their Post
--				Completion step that can be worked on by the specified user.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	3/29/2017
-- Description:		- Updated the selection statement to use the new
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
CREATE PROCEDURE [dbo].[core_sarc_sp_PostCompletionSearch]
	@userId INT,
	@caseID VARCHAR(50) = NULL,
	@ssn VARCHAR(10) = NULL,
	@name VARCHAR(10) = NULL,
	@rptView TINYINT,
	@compo VARCHAR(10) = NULL,
	@unitId INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- Validate input...
	IF (ISNULL(@userId, 0) = 0)
	BEGIN
		SELECT 0
	END

	IF  @caseID = '' SET @caseID = NULL 
	IF  @ssn = '' SET @ssn = NULL 
	IF  @name = '' SET @name = NULL 	
	IF  @compo = '' SET @compo = NULL 
	IF  @unitId = 0 SET @unitId = NULL

	DECLARE	@userUnit INT, 
			@groupId TINYINT, 
			@scope TINYINT ,
			@userSSN CHAR(9),
			@viewSubUnits INT = 1,
			@sarcModuleId INT = 25,
			@lodModuleId INT = 2,
			@sarcILODMemoTemplateId TINYINT = 72,
			@sarcNILODMemoTemplateId TINYINT = 73,
			@ilodFindingId TINYINT = 1,
			@approvingAuthorityPTypeId TINYINT = 10

	SELECT	@userUnit = unit_id, 
			@groupId = groupId, 
			@scope = accessscope, 
			@userSSN = SSN
	FROM	vw_Users
	WHERE	userId = @userId

	SELECT	@viewSubUnits = unitView 
	FROM	core_Users
	WHERE	userID = @userId

	CREATE TABLE #TempSearchData
	(
		RefId INT,
		CaseId VARCHAR(50),
		SSN NVARCHAR(9),
		Name NVARCHAR(100),
		UnitName NVARCHAR(100),
		WorkStatus VARCHAR(50),
		DaysCompleted INT,
		ModuleId INT,
		caseUnitId INT,
		WorkStatusId INT,
		Compo CHAR(1),
		isAttachPAS BIT,
		member_attached_unit_id INT
	)

	-- Inserting data into a temporary table due to the complex joins of this query and its inefficient interactions with the WHERE conditions which need to be done. 
	-- Place the data into a temporary table AND THEN doing a select with the complex where conditions is easier for the SQL Server to create an efficient execution
	-- plan. 
	INSERT	INTO	#TempSearchData
			SELECT	sr.sarc_id AS RefId,
					sr.case_id AS CaseId,
					sr.member_ssn AS MemberSSN,
					sr.member_name AS MemberName, 
					sr.member_unit AS MemberUnit,
					sc.description AS Status,
					DATEDIFF(d, comp.CompletedDate, GETDATE()) AS DaysCompleted,
					@sarcModuleId AS [ModuleId],
					sr.member_unit_id,
					sr.status,
					sr.member_compo,
					0, 
					0
			FROM	Form348_SARC sr
					JOIN core_WorkStatus ws ON sr.status = ws.ws_id
					JOIN core_StatusCodes sc ON ws.statusId = sc.statusId
					JOIN core_lkupComponent mc ON sr.duty_status = mc.component_id
					JOIN FORM348_SARC_findings srf ON sr.sarc_id = srf.SARC_ID AND srf.ptype = @approvingAuthorityPTypeId
					LEFT JOIN Form348_SARC_PostProcessing srpp ON sr.sarc_id = srpp.sarc_id
					LEFT OUTER JOIN (
						SELECT	refId, MAX(endDate) AS CompletedDate
						FROM	dbo.core_WorkStatus_Tracking AS core_WorkStatus_Tracking_1 
								INNER JOIN dbo.vw_WorkStatus w ON core_WorkStatus_Tracking_1.ws_id = w.ws_id
						WHERE	module = @sarcModuleId AND w.isFinal = 1 AND w.isCancel = 0
						GROUP BY refId
					) AS comp ON comp.refId = sr.sarc_id
			WHERE	sc.isFinal = 1
					AND sc.isCancel = 0
					AND sr.is_post_processing_complete = 0
					AND mc.component_description NOT LIKE '%cadet%'

			UNION ALL

			SELECT	a.lodId AS RefId,
					a.case_id AS CaseId,
					a.member_ssn AS MemberSSN,
					a.member_name AS MemberName,
					a.member_unit AS MemberUnit,
					ws.description AS Status,
					DATEDIFF(d,  t.ReceiveDate , GETDATE()) DaysCompleted,
					@lodModuleId AS ModuleId,
					a.member_unit_id,
					a.status,
					a.member_compo,
					a.isAttachPAS,
					a.member_attached_unit_id
			FROM	Form348 a
					JOIN vw_workstatus ws ON ws.ws_id = a.status 
					JOIN Form348_Medical med ON med.lodid = a.lodid 	
					LEFT JOIN (SELECT max(startDate) ReceiveDate, ws_id, refId FROM	core_WorkStatus_Tracking GROUP BY ws_id, refId) t ON t.ws_id = a.status AND t.refId = a.lodId 
			WHERE	ws.isFinal = 1
					AND ws.isCancel = 0
					AND a.is_post_processing_complete = 0
					AND a.sarc = 1
					AND a.restricted = 1
					


	SELECT	r.RefId,
			r.CaseId,
			RIGHT(r.SSN, 4) AS MemberSSN,
			r.Name AS MemberName, 
			r.UnitName AS MemberUnit,
			r.WorkStatus AS Status,
			r.DaysCompleted,
			r.ModuleId
	FROM	#TempSearchData r
	WHERE	r.Name LIKE '%' + ISNULL(@Name,r.Name) + '%'
			AND r.CaseId LIKE ISNULL(@caseId,r.CaseId) + '%' 
			AND r.SSN LIKE '%' + ISNULL(@ssn,r.SSN)
			AND r.Compo LIKE '%' + ISNULL(@compo,r.Compo) + '%'
			AND
			(
				@scope > 1
				OR
				dbo.fn_IsUserTheMember(@userId, r.SSN) = 0	-- Don't return cases which revolve around the user doing the search...
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
					@viewSubUnits = 1
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
							r.member_attached_unit_id IN (SELECT child_id FROM Command_Struct_Tree WHERE parent_id=@userUnit AND view_type=@rptView )
						)
						OR
						@scope > 1
					)
				)
				OR
				(
					@viewSubUnits = 0
					AND
					(
						r.caseUnitId = @userUnit
						OR
						(
							r.isAttachPAS = 1
							AND
							r.member_attached_unit_id  IN (SELECT child_id FROM Command_Struct_Tree WHERE parent_id=@userUnit AND view_type=@rptView )
						)
					)
				)
			)
			AND  	 
			( 
				r.WorkStatusId IN (SELECT ws_id FROM vw_WorkStatus WHERE moduleId = r.ModuleId)
				OR
				r.WorkStatusId IN (SELECT status FROM core_StatusCodeSigners WHERE groupId = @groupId)
			)
	ORDER	BY r.DaysCompleted
END
GO

