-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	Selects user emails for a SC case by group Id.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	11/21/2015
-- Description:		Modified so that the all of the roles a user has assigned
--					to them are taken into account when selecting the email
--					addressed.
-- ============================================================================
-- Modified By:		GitHub Copilot
-- Modified Date:	2025-10-11
-- Description:		Added pagination parameters and logic.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_user_sp_GetMailingListByGroup_SC_pagination]
	@refId INT,
	@groupId INT,
	@PageNumber INT = 1,
	@PageSize INT = 10
AS

DECLARE @member_unit_id INT, @viewType AS TINYINT;

SELECT @member_unit_id = member_unit_id FROM Form348_SC WHERE SC_id = @refId;

SET @viewType = (SELECT reportView FROM core_userGroups WHERE groupId = @groupId);

DECLARE @MemberParentUnits TABLE
(
	cs_id INT
)

INSERT	INTO	@MemberParentUnits (cs_id)
		SELECT	DISTINCT parent_id  FROM Command_Struct_Tree WHERE child_id = @member_unit_id AND view_type = @viewType;

WITH AllEmails(Email, Email2, Email3) AS
(
	SELECT	a.Email ,a.Email2,a.Email3
	FROM	vw_users a
			JOIN core_UserRoles ur ON a.userID = ur.userID
	WHERE
		a.accessStatus =  3
	AND
		a.receiveEmail = 1
	AND
		ur.groupId = @groupId
	--Scope filtering
	AND
	(
		a.unit_id in (SELECT cs_id FROM @MemberParentUnits)
	)
)
SELECT  Email FROM AllEmails WHERE Email is not NULL and LEN(email) > 0
	UNION
SELECT	 Email2 FROM AllEmails WHERE Email2 is not NULL and LEN(email2) > 0
	UNION
SELECT	 Email3 FROM AllEmails WHERE Email3 is not NULL and LEN(email3) > 0
ORDER BY Email
OFFSET (@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
GO