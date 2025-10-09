
-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	Returns the set of RR cases which are in a step that is 
---				associated with the group ID of the the specified user . 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	12/9/2015
-- Work Item:		TFS Task 386
-- Description:		Added a new condition to the where clause where the status
--					of the RR case is checked against the set of RR statuses. 
--					This is done in other similar stored procedures for the LOD
--					and SC workflows. 
--					Removed the where condition which checks the users current
--					role, but it is no longer necessary given that the group
--					ID is being used to match cases.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	6/28/2016
-- Description:		- Modified to no longer select cases which revolved around
--					the user calling this procedure (i.e. conducting the search). 
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	7/8/2016
-- Description:		Ordered the results based on the days field, from 
--					oldest to newest
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	8/2/2016
-- Description:		Updated to no longer use a hard coded value for the LOD
--					statuses.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	1/10/2017
-- Description:		- Modified to no use the fn_IsUserTheMember() function when
--					checking if the user and member match.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	6/9/2017
-- Description:		- Modified to use the RR cases member unit info instead of
--					the LODs member unit info.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lod_sp_GetReinvestigationRequests]
	@userId INT,
	@sarc BIT 
AS
BEGIN
	Declare @Test INT
	--Declare @sarc bit

	--SET @userId = 295  -- 469 is system admin, 295 is MPF  , 316 = Wing JA, 2 = Approving Auth
	--SET @sarc = 1

	DECLARE @roleId INT = 0, @groupId INT
	DECLARE @userUnit INT, @scope INT, @rptView INT, @userSSN CHAR(9)
	DECLARE @moduleId INT = 0

	SELECT @roleId = currentRole FROM core_Users WHERE userID = @userId
	SELECT @userUnit = unit_id, @scope = accessscope, @rptView = ViewType, @userSSN = SSN FROM vw_users WHERE userID = @userId
	SELECT @groupId = groupId FROM core_UserRoles WHERE userRoleID = @roleId
	SELECT @moduleId = moduleId FROM core_lkupModule WHERE moduleName = 'Reinvestigation Request'

	DECLARE @unitView int = 1
	SELECT @unitView = unitView FROM core_Users WHERE userID = @userId

	IF(dbo.TRIM(ISNULL(@userSSN, '')) = '')
	BEGIN
		SET @userSSN = NULL
	END
	
	DECLARE @Compo int
	SET @Compo = dbo.GetCompoForUser(@userId)

	-- If user Id passed in was actually the role Id
	IF @roleId = 0
		SET @roleId = @userId

	SELECT	DISTINCT request_id
			, SubString(frr.member_ssn, 6, 4) AS Protected_SSN
			, frr.member_name AS Member_Name
			, frr.member_unit AS Unit_Name
			, frr.Case_Id
			, vws.description AS Status
			, Convert(char(11), ISNULL(t.ReceiveDate, frr.createddate), 101) AS Receive_Date
			, DateDiff(d, ISNULL(t.ReceiveDate, frr.createddate), GetDate()) AS Days
	FROM	Form348_RR frr
			INNER JOIN Form348 lod ON lod.lodId = frr.InitialLodId
			INNER JOIN vw_WorkStatus vws ON frr.status = vws.ws_id
			INNER JOIN vw_WorkStatus lodVWS ON lod.status = lodVWS.ws_id
			INNER JOIN core_UserRoles cur ON cur.groupId = vws.groupId
			LEFT JOIN (
				SELECT Max(startDate) ReceiveDate, ws_id, refId 
				FROM core_WorkStatus_Tracking 
				GROUP BY ws_id, refId
			) t ON t.refId = frr.request_id AND t.ws_id = frr.status
	WHERE	(lodVWS.isFinal = 1 AND lodvws.isCancel = 0)
			AND frr.Member_Compo = @Compo
			AND 
			(
				(
					@unitView = 1
					AND (
							frr.member_unit_id In 
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
					AND (frr.member_unit_id = @userUnit
						 OR @scope > 1)
				)		
			)
			AND  	 
			( 
				frr.status IN (SELECT ws_id FROM vw_WorkStatus WHERE moduleId = @moduleId AND groupId = @groupId)
				OR
				frr.status IN (SELECT status FROM core_StatusCodeSigners WHERE groupId = @groupId)
			)
			AND
			(
				@scope > 1
				OR
				dbo.fn_IsUserTheMember(@userId, frr.member_ssn) = 0	-- Don't return cases which revolve around the user doing the search...
			)
	ORDER	BY Days DESC
END
GO

