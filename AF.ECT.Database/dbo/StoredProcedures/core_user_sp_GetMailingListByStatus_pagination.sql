-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	Selects user emails for a LOD, RR, SC case by work status Id.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	11/21/2015
-- Description:		Modified so that the all of the roles a user has assigned
--					to them are taken into account when selecting the email
--					addresses.
--					Also made it so that the user group to work status mappings
--					in the core_StatusCodeSigners table are taken into account
--					when selecting user email addresses.
--					Also made is so that hard coded work status Id values are
--					no longer used when determining if the passed in work
--					status is part of the LOD, RR, or SC workflow.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	8/2/2016
-- Description:		Updated to use the module name instead of the workflow
--					title to find workstatus Ids.
-- ============================================================================
-- Modified By:		GitHub Copilot
-- Modified Date:	2025-10-11
-- Description:		Added pagination parameters and logic.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_user_sp_GetMailingListByStatus_pagination]
	@refId INT,
 	@status SMALLINT,
	@PageNumber INT = 1,
	@PageSize INT = 10
AS
BEGIN
	DECLARE @WorkflowWorkStatuses TABLE
	(
		wsId INT
	)

	-- Find LOD workflow work statuses...
	INSERT	INTO	@WorkflowWorkStatuses ([wsId])
			SELECT	vws.ws_id
			FROM	vw_WorkStatus vws
					JOIN core_lkupModule m ON vws.moduleId = m.moduleId
			WHERE	m.moduleName = 'LOD'

	-- LODs
	IF (@status IN (SELECT wsId FROM @WorkflowWorkStatuses))
	BEGIN
		DECLARE @MemberParentUnits TABLE
		(
			cs_id INT
		)

		DECLARE @AssociatedGroups TABLE
		(
			GroupId INT
		)

		--Member unit falls in the user's hirerchy
		DECLARE @member_unit_id INT, @aaUserId INT, @ioUserId INT, @compo CHAR(1), @groupId INT = 0, @view_type SMALLINT, @filter VARCHAR(50)

		DECLARE @isAttachedUnit BIT

		-- Get the viewtype from the status
		-- This is the group responsible for the status code
		SELECT	@groupId = a.groupId, @view_type = g.reportView, @filter = a.filter
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

		-- Get the member's unit and info
		SELECT @isAttachedUnit = f.isAttachPas FROM Form348 f WHERE f.lodId=@refId

		SELECT
			@member_unit_id = CASE @isAttachedUnit WHEN 1 THEN b.member_attached_unit_id ELSE b.member_unit_id END,
			@compo = b.member_compo,
			@ioUserId=f.IoUserId,
			@aaUserId=b.appAuthUserId
		FROM
			Form348  b
		LEFT JOIN
			form261 f ON f.lodid =b.lodid
		WHERE b.lodid = @refId;

		-- Get the parent units for the member's unit
		INSERT	INTO	@MemberParentUnits (cs_id)
				SELECT	DISTINCT parent_id
				FROM	Command_Struct_Tree
				WHERE	child_id=@member_unit_id
						AND view_type =@view_type;

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
				a.unit_id in (SELECT cs_id FROM @MemberParentUnits)
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
	ELSE
	BEGIN
		DELETE FROM @WorkflowWorkStatuses

		-- Find RR workflow work statuses...
		INSERT	INTO	@WorkflowWorkStatuses ([wsId])
				SELECT	vws.ws_id
				FROM	vw_WorkStatus vws
						JOIN core_lkupModule m ON vws.moduleId = m.moduleId
				WHERE	m.moduleName = 'Reinvestigation Request'

		-- Reinvestigation
		IF (@status IN (SELECT wsId FROM @WorkflowWorkStatuses))
			BEGIN
				EXEC core_user_sp_GetMailingListByStatus_RR @refId, @status
			END
		-- Special Case
		ELSE
			BEGIN
				EXEC core_user_sp_GetMailingListByStatus_SC @refId, @status
			END
	END
END
GO