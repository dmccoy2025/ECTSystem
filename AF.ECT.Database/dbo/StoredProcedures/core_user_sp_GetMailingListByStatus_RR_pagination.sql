-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	Selects user emails for a RR case by work status Id.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	11/21/2015
-- Description:		Modified so that the all of the roles a user has assigned
--					to them are taken into account when selecting the email
--					addresses.
--					Also made it so that the user group to work status mappings
--					in the core_StatusCodeSigners table are taken into account
--					when selecting user email addresses.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	6/9/2017
-- Description:		- Modified to use the RR cases member unit instead of
--					the LODs member info.
-- ============================================================================
-- Modified By:		GitHub Copilot
-- Modified Date:	2025-10-11
-- Description:		Added pagination parameters and logic.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_user_sp_GetMailingListByStatus_RR_pagination]
	@refId INT,
 	@status SMALLINT,
	@PageNumber INT = 1,
	@PageSize INT = 10
AS
BEGIN
	DECLARE @MemberParentUnits TABLE
	(
		cs_id INT
	)

	DECLARE @AssociatedGroups TABLE
	(
		GroupId INT
	)

	DECLARE @member_unit_id INT,@aaUserId INT, @ioUserId INT, @compo CHAR(1), @groupId INT, @view_type SMALLINT, @filter VARCHAR(50)

	--get the viewtype from the status
	--this is the group responsible for the status code
	SELECT	@groupId = a.groupId,
			@view_type = g.reportView,
			@filter = a.filter
	FROM	vw_WorkStatus a
			JOIN core_UserGroups g ON g.groupId = a.groupId
	WHERE	a.ws_id = @status

	IF (@groupId > 0)
	BEGIN
		INSERT INTO @AssociatedGroups ([GroupId])
		VALUES (@groupId)
	END

	-- Find other user groups associated with the passed in work status...
	INSERT	INTO	@AssociatedGroups ([GroupId])
			SELECT	scs.groupId
			FROM	core_StatusCodeSigners scs
			WHERE	scs.status = @status

	SELECT	@member_unit_id = r.member_unit_id,
			@compo = r.member_compo,
			@ioUserId = f.IoUserId,
			@aaUserId = b.appAuthUserId
	FROM	Form348_RR r
			INNER JOIN Form348  b ON r.InitialLodId = b.lodId
			LEFT JOIN form261 f ON f.lodid = b.lodid
	WHERE	r.request_id = @refId;

	--get the parent units for the member's unit
	INSERT	INTO	@MemberParentUnits (cs_id)
			SELECT	DISTINCT parent_id
			FROM	Command_Struct_Tree
			WHERE	child_id = @member_unit_id
					AND view_type = @view_type;

	WITH AllEmails(Email, Email2, Email3) AS
	(
		SELECT	a.Email ,a.Email2,a.Email3
		FROM	vw_users a
				JOIN core_UserRoles ur ON a.userID = ur.userID
		WHERE
			ur.groupId IN (SELECT GroupId FROM @AssociatedGroups)
		AND
			a.accessStatus =  3
		AND
			a.receiveEmail = 1
		AND
			a.compo = @compo
		--Scope filtering
		AND
		(
			a.unit_id in (SELECT cs_id from @MemberParentUnits)
		)
		--User ID Filtering
		AND
		(
			a.userId = CASE
						WHEN @filter = 'io' THEN @ioUserId
						WHEN @filter = 'aa' THEN @aaUserId
						ELSE a.userId
					END
		)
	)
	SELECT	Email FROM AllEmails WHERE Email IS NOT NULL AND LEN(email) > 0
	UNION
	SELECT	Email2 FROM AllEmails WHERE Email2 IS NOT NULL AND LEN(email2) > 0
	UNION
	SELECT	Email3 FROM AllEmails WHERE Email3 IS NOT NULL AND LEN(email3) > 0
	ORDER BY Email
	OFFSET (@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
END
GO