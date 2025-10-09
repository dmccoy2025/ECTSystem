
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 12/3/2016
-- Description:	Returns a search on the Restricted SARC cases.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	3/1/2017
-- Description:		- Modified to select legacy restricted SARC LOD cases.
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
-- Modified Date:	7/31/2017
-- Description:		- statusId now takes in an int
-- ============================================================================
CREATE PROCEDURE [dbo].[core_sarc_sp_Search]
	@userId INT,
	@caseID VARCHAR(50) = NULL,
	@ssn VARCHAR(10) = NULL,
	@name VARCHAR(10) = NULL,
	@rptView TINYINT,
	@compo VARCHAR(10) = NULL,
	@statusId INT = NULL,
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
	IF  @statusId = 0 SET @statusId = NULL	 
	IF  @unitId = 0 SET @unitId = NULL

	DECLARE	@userUnit INT, 
			@groupId TINYINT, 
			@scope TINYINT ,
			@userSSN CHAR(9),
			@viewSubUnits INT = 1,
			@moduleId INT = 25,
			@lodModuleId INT = 2

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
		Days INT,
		DaysCompleted INT,
		lockId INT,
		ModuleId INT,

		Compo CHAR(1),
		StatusCodeId INT,
		IsPostProcessingComplete BIT,
		caseUnitId INT,
		memberCurrentUnitId INT
	)


	-- Inserting data into a temporary table due to the complex joins of this query and its inefficient interactions with the WHERE conditions which need to be done. 
	-- Place the data into a temporary table AND THEN doing a select with the complex where conditions is easier for the SQL Server to create an efficient execution
	-- plan. 
	INSERT	INTO	#TempSearchData
			SELECT	f.lodId AS RefId,
					f.case_id AS CaseId,
					f.member_ssn AS MemberSSN,
					f.member_name AS MemberName, 
					f.member_unit AS MemberUnit,
					sc.description AS Status,
					CASE sc.isFinal WHEN 0 THEN DATEDIFF(D, ISNULL(track.ReceiveDate, f.created_date), GETDATE()) ELSE NULL END AS Days, 
					DATEDIFF(d, comp.CompletedDate, GETDATE()) AS DaysCompleted,
					ISNULL(l.id,0) lockId,
					@lodModuleId AS ModuleId,
					f.member_compo,
					sc.statusId,
					f.is_post_processing_complete,
					f.member_unit_id,
					mCS.CS_ID
			FROM	Form348 f
					JOIN core_WorkStatus ws ON f.status = ws.ws_id
					JOIN core_StatusCodes sc ON ws.statusId = sc.statusId
					LEFT JOIN MemberData md ON f.member_ssn = md.SSAN
					LEFT JOIN Command_Struct mCS ON md.PAS_NUMBER = mCS.PAS_CODE
					LEFT JOIN core_workflowLocks l ON l.refId = f.lodId AND l.module = @lodModuleId
					LEFT OUTER JOIN (
						SELECT	MAX(startDate) AS ReceiveDate, ws_id, refId
						FROM	dbo.core_WorkStatus_Tracking
						WHERE	module = @lodModuleId
						GROUP BY ws_id, refId
					) AS track ON track.refId = f.lodId AND track.ws_id = f.status
					LEFT OUTER JOIN (
						SELECT	refId, MAX(endDate) AS CompletedDate
						FROM	dbo.core_WorkStatus_Tracking AS core_WorkStatus_Tracking_1 
								INNER JOIN dbo.vw_WorkStatus w ON core_WorkStatus_Tracking_1.ws_id = w.ws_id
						WHERE	module = @lodModuleId AND w.isFinal = 1 AND w.isCancel = 0
						GROUP BY refId
					) AS comp ON comp.refId = f.lodId
			WHERE	f.sarc = 1
					AND f.restricted = 1

			UNION ALL
	
			SELECT	sr.sarc_id AS RefId,
					sr.case_id AS CaseId,
					RIGHT(sr.member_ssn, 4) AS MemberSSN,
					sr.member_name AS MemberName, 
					sr.member_unit AS MemberUnit,
					sc.description AS Status,
					CASE sc.isFinal WHEN 0 THEN DATEDIFF(D, ISNULL(track.ReceiveDate, sr.created_date), GETDATE()) ELSE NULL END AS Days, 
					DATEDIFF(d, comp.CompletedDate, GETDATE()) AS DaysCompleted,
					ISNULL(l.id,0) lockId,
					@moduleId AS ModuleId,
					sr.member_compo,
					sc.statusId,
					sr.is_post_processing_complete,
					sr.member_unit_id,
					mCS.CS_ID
			FROM	Form348_SARC sr
					JOIN core_WorkStatus ws ON sr.status = ws.ws_id
					JOIN core_StatusCodes sc ON ws.statusId = sc.statusId
					LEFT JOIN MemberData md ON sr.member_ssn = md.SSAN
					LEFT JOIN Command_Struct mCS ON md.PAS_NUMBER = mCS.PAS_CODE
					LEFT JOIN core_workflowLocks l ON l.refId = sr.sarc_id AND l.module = @moduleId
					LEFT OUTER JOIN (
						SELECT	MAX(startDate) AS ReceiveDate, ws_id, refId
						FROM	dbo.core_WorkStatus_Tracking
						WHERE	module = @moduleId
						GROUP BY ws_id, refId
					) AS track ON track.refId = sr.sarc_id AND track.ws_id = sr.status
					LEFT OUTER JOIN (
						SELECT	refId, MAX(endDate) AS CompletedDate
						FROM	dbo.core_WorkStatus_Tracking AS core_WorkStatus_Tracking_1 
								INNER JOIN dbo.vw_WorkStatus w ON core_WorkStatus_Tracking_1.ws_id = w.ws_id
						WHERE	module = @moduleId AND w.isFinal = 1 AND w.isCancel = 0
						GROUP BY refId
					) AS comp ON comp.refId = sr.sarc_id



	SELECT	r.RefId,
			r.CaseId,
			RIGHT(r.SSN, 4) AS MemberSSN,
			r.Name AS MemberName, 
			r.UnitName AS MemberUnit,
			r.WorkStatus AS Status,
			r.Days, 
			r.DaysCompleted,
			r.lockId,
			r.ModuleId
	FROM	#TempSearchData r
	WHERE	r.Name LIKE '%' + ISNULL(@Name,r.Name) + '%'
			AND r.CaseId LIKE ISNULL(@caseId,r.CaseId) + '%'
			AND r.SSN LIKE '%' + ISNULL(@ssn,r.SSN)
			AND r.Compo LIKE '%' + ISNULL(@compo,r.Compo) + '%'
			AND ISNULL(@statusId, r.StatusCodeId) = r.StatusCodeId
			AND ISNULL(@unitId, CASE r.IsPostProcessingComplete WHEN 1 THEN r.memberCurrentUnitId ELSE r.caseUnitId END) = (CASE r.IsPostProcessingComplete WHEN 1 THEN r.memberCurrentUnitId ELSE r.caseUnitId END)
			AND
			(
				@scope > 1
				OR
				dbo.fn_IsUserTheMember(@userId, r.SSN) = 0	-- Don't return cases which revolve around the user doing the search...
			)
			AND
			(  
				(
					@viewSubUnits = 1
					AND
					(
						CASE r.IsPostProcessingComplete WHEN 1 THEN r.memberCurrentUnitId ELSE r.caseUnitId END IN
						(
							SELECT child_id FROM Command_Struct_Tree WHERE parent_id = @userUnit AND view_type = @rptView
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
						CASE r.IsPostProcessingComplete WHEN 1 THEN r.memberCurrentUnitId ELSE r.caseUnitId END = @userUnit
					)
				)
			)
	ORDER	BY r.CaseId
END
GO

