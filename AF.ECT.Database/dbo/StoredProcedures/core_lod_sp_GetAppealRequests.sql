-- Exec core_lod_sp_GetReinvestigationRequests 2, 0

-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 7/28/2016
-- Description:	Returns the set of AP cases which are in a step that is 
---				associated with the group ID of the the specified user . 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	1/10/2017
-- Description:		- Modified to no use the fn_IsUserTheMember() function when
--					checking if the user and member match.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	6/9/2017
-- Description:		- Modified to use the AP cases member unit info instead of
--					the LODs member unit info.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lod_sp_GetAppealRequests]
	@userId int,
	@sarc bit 
AS
BEGIN
	DECLARE @roleId INT = 0, @groupId INT
	DECLARE @userUnit INT, @scope INT, @rptView INT, @userSSN CHAR(9)
	DECLARE @moduleId INT = 0

	SELECT @roleId = currentRole FROM core_Users WHERE userID = @userId
	SELECT @userUnit = unit_id, @scope = accessscope, @rptView = ViewType, @userSSN = SSN FROM vw_users WHERE userID = @userId
	SELECT @groupId = groupId FROM core_UserRoles WHERE userRoleID = @roleId
	SELECT @moduleId = moduleId FROM core_lkupModule WHERE moduleName = 'Appeal Request'

	DECLARE @unitView int = 1
	SELECT @unitView = unitView FROM core_Users WHERE userID = @userId

	IF(dbo.TRIM(ISNULL(@userSSN, '')) = '')
	BEGIN
		SET @userSSN = NULL
	END

	-- If user Id passed in was actually the role Id
	IF @roleId = 0
		SET @roleId = @userId

			DECLARE @Compo int
	SET @Compo = dbo.GetCompoForUser(@userId)


	SELECT	DISTINCT appeal_id
			, SubString(ap.member_ssn, 6, 4) AS Protected_SSN
			, ap.member_name AS Member_Name
			, ap.member_unit AS Unit_Name
			, ap.Case_Id
			, vws.description AS Status
			, Convert(char(11), ISNULL(t.ReceiveDate, ap.created_date), 101) AS Receive_Date
			, DateDiff(d, ISNULL(t.ReceiveDate, ap.created_date), GetDate()) AS Days
	FROM	Form348_AP ap
			Inner Join Form348 lod ON lod.lodId = ap.initial_lod_id
			Inner Join vw_WorkStatus vws ON ap.status = vws.ws_id
			INNER JOIN vw_WorkStatus lodVWS ON lod.status = lodVWS.ws_id
			Inner Join core_UserRoles cur ON cur.groupId = vws.groupId
			LEFT JOIN (
				SELECT Max(startDate) ReceiveDate, ws_id, refId 
				FROM core_WorkStatus_Tracking 
				GROUP BY ws_id, refId
			) t ON t.refId = ap.appeal_id AND t.ws_id = ap.status
	WHERE 
		(lodVWS.isFinal = 1 AND lodvws.isCancel = 0)  -- Initial LOD is Complete
		--AND cur.userRoleID = @roleId
		AND ap.Member_Compo = @Compo
		AND 
		(
			(
				@unitView = 1
				AND (
						ap.member_unit_id In 
						(
							SELECT child_id FROM Command_Struct_Tree WHERE parent_id = @userUnit and view_type = @rptView
						)
						Or
						@scope > 1
					)
			)
		
			OR
			(
				@unitView = 0
				AND (ap.member_unit_id = @userUnit
					 OR @scope > 1)
			)		
		)
		AND  	 
		( 
			ap.status IN (SELECT ws_id FROM vw_WorkStatus WHERE moduleId = @moduleId AND groupId = @groupId)
			OR
			ap.status IN (SELECT status FROM core_StatusCodeSigners WHERE groupId = @groupId)
		)
		AND
		(
			@scope > 1
			OR
			dbo.fn_IsUserTheMember(@userId, ap.member_ssn) = 0	-- Don't return cases which revolve around the user doing the search...
		)
	ORDER BY Days DESC
END
GO

