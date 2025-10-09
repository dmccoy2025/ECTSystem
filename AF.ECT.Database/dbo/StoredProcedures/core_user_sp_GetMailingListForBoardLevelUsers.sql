
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 8/12/2016
-- Description:	Selects email addresses for all users which have a role that
--				matches the groupId parameter.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_user_sp_GetMailingListForBoardLevelUsers]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- SELECT EMAIL ADDRESSES -- 
	;WITH AllEmails(Email, Email2, Email3) AS
	( 
		SELECT	a.Email ,a.Email2,a.Email3 
		FROM	vw_users a 
				JOIN core_UserRoles ur ON a.userID = ur.userID 
		WHERE	a.accessStatus =  3
				AND a.receiveEmail = 1
				AND ur.groupId IN (SELECT ug.groupId FROM core_UserGroups ug WHERE ug.name IN ('Board Technician', 'Board Legal', 'Board Medical', 'Approving Authority', 'Approving Authority Read Only', 'Board Administrator'))
	)
	SELECT	Email	FROM AllEmails	WHERE Email is not NULL and LEN(email) > 0
		UNION
	SELECT	Email2	FROM AllEmails	WHERE Email2 is not NULL and LEN(email2) > 0
		UNION
	SELECT	Email3	FROM AllEmails	WHERE Email3 is not NULL and LEN(email3) > 0
END
GO

