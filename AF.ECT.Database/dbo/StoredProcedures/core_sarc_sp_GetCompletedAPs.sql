
-- ============================================================================
-- Author:			Evan Morrison
-- Created Date:	1/10/2017
-- Description:		Used to get all completed SARC appeals
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
--					- Cleaned up the procedure.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	6/9/2017
-- Description:		- Modified to use the APSA cases member unit info instead of
--					the LODs/SARCs member unit info.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_sarc_sp_GetCompletedAPs]
	@userId INT,
	@sarc BIT 
AS
BEGIN
	DECLARE @roleId INT, @isAdmin INT = 0, @groupId INT, @scope INT, @rptView INT, @userUnit INT, @userSSN CHAR(9)

	SELECT  @roleId = currentRole FROM core_Users WHERE userID = @userId
	SELECT  @groupId = groupId FROM core_UserRoles WHERE userRoleID = @roleId
	SELECT  @userUnit = unit_id, @scope = accessscope, @rptView = ViewType, @userSSN = SSN FROM vw_Users WHERE userId = @userId

	DECLARE @unitView INT = 1
	SELECT @unitView = unitView FROM core_Users WHERE userID = @userId

	IF(dbo.TRIM(ISNULL(@userSSN, '')) = '')
	BEGIN
		SET @userSSN = NULL
	END

	IF @groupId = 1  -- System Admin
		SET @isAdmin = 1
	IF @groupId = 8  -- Board Legal
		SET @isAdmin = 1
	IF @groupId = 9  -- Board Medical
		SET @isAdmin = 1
	IF @groupId = 97  -- Board Admin
		SET @isAdmin = 1
	IF @groupId = 103	-- SARC Admin
		SET @isAdmin = 1
	IF @groupId = 100  -- Appellate Auth
		SET @isAdmin = 1


	CREATE TABLE #TempSearchData
	(
		appeal_sarc_id INT,
		SSN NVARCHAR(9),
		Name NVARCHAR(100),
		UnitName NVARCHAR(100),
		CaseId VARCHAR(50),
		WorkStatus VARCHAR(50),
		ReceiveDate CHAR(11),
		Days INT,
		caseUnitId INT,
		IsPostProcessingComplete BIT,
		memberCurrentUnitId INT
	)

	-- Inserting data into a temporary table due to the complex joins of this query and its inefficient interactions with the WHERE conditions which need to be done. 
	-- Place the data into a temporary table AND THEN doing a select with the complex where conditions is easier for the SQL Server to create an efficient execution
	-- plan. 
	INSERT	INTO	#TempSearchData
			SELECT	DISTINCT appeal_sarc_id,
					ap.member_ssn,
					ap.member_name,
					ap.member_unit,
					ap.Case_Id,
					vws.description AS Status,
					CONVERT(char(11), ISNULL(t.ReceiveDate, ap.created_date), 101) AS Receive_Date,
					CASE vws.isFinal WHEN 0 THEN DATEDIFF(D, ISNULL(t.ReceiveDate, ap.created_date), GETDATE()) ELSE NULL END AS Days,
					ap.member_unit_id,
					ap.is_post_processing_complete,
					mCS.CS_ID
			FROM	Form348_AP_SARC ap
					JOIN Form348 lod On lod.lodId = ap.initial_id
					JOIN vw_WorkStatus vws On ap.status = vws.ws_id
					LEFT JOIN MemberData md ON ap.member_ssn = md.SSAN
					LEFT JOIN Command_Struct mCS ON md.PAS_NUMBER = mCS.PAS_CODE
					LEFT JOIN (
						SELECT Max(startDate) ReceiveDate, ws_id, refId 
						FROM core_WorkStatus_Tracking 
						GROUP BY ws_id, refId) t 
							ON t.refId = ap.appeal_sarc_id AND t.ws_id = ap.status
			WHERE	ap.initial_workflow = lod.workflow
					AND vws.isFinal = 1
			
			UNION ALL

			SELECT	DISTINCT appeal_sarc_id,
					ap.member_ssn,
					ap.member_name,
					ap.member_unit,
					ap.Case_Id,
					vws.description,
					CONVERT(CHAR(11), ISNULL(t.ReceiveDate, ap.created_date), 101),
					CASE vws.isFinal WHEN 0 THEN DATEDIFF(D, ISNULL(t.ReceiveDate, ap.created_date), GETDATE()) ELSE NULL END,
					ap.member_unit_id,
					ap.is_post_processing_complete,
					mCS.CS_ID
			FROM	Form348_AP_SARC ap
					JOIN Form348_SARC sarc On sarc.sarc_id = ap.initial_id
					JOIN vw_WorkStatus vws On ap.status = vws.ws_id
					LEFT JOIN MemberData md ON ap.member_ssn = md.SSAN
					LEFT JOIN Command_Struct mCS ON md.PAS_NUMBER = mCS.PAS_CODE
					LEFT JOIN (
						SELECT Max(startDate) ReceiveDate, ws_id, refId 
						FROM core_WorkStatus_Tracking 
						GROUP BY ws_id, refId) t 
							ON t.refId = ap.appeal_sarc_id AND t.ws_id = ap.status
			WHERE	ap.initial_workflow = sarc.workflow
					AND vws.isFinal = 1


	SELECT	r.appeal_sarc_id,
			SUBSTRING(r.SSN, 6, 4) AS Protected_SSN,
			r.Name AS Member_Name,
			r.UnitName AS Unit_Name,
			r.CaseId AS Case_Id,
			r.WorkStatus AS Status,
			r.ReceiveDate AS Receive_Date,
			r.Days
	FROM	#TempSearchData r
	WHERE	(
				@scope > 1
				OR
				dbo.fn_IsUserTheMember(@userId, r.SSN) = 0	-- Don't return cases which revolve around the user doing the search...
			)
			AND
			(
				(
					@unitView = 1
					AND 
					(
						CASE r.IsPostProcessingComplete WHEN 1 THEN r.memberCurrentUnitId ELSE r.caseUnitId END IN
						(
							SELECT child_id FROM Command_Struct_Tree WHERE parent_id = @userUnit AND view_type = @rptView
						)
						OR
						@isAdmin = 1 -- Admin View, See all
					)
				)
				OR
				(
					@unitView = 0
					AND 
					(
						CASE r.IsPostProcessingComplete WHEN 1 THEN r.memberCurrentUnitId ELSE r.caseUnitId END = @userUnit 
						OR
						@isAdmin = 1 -- Admin View, See all
					)
				)
			)
	ORDER	BY r.Days DESC
END
GO

