--EXEC core_user_sp_GetMailingListByRoles '6', 20151553 ,'1'
--EXEC core_user_sp_GetMailingListByRoles '6', 20151552 ,'4,2'

-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	Selects user emails based on the passed in group Ids.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	11/21/2015
-- Description:		Modified so that the all of the roles a user has assigned
--					to them are taken into account when selecting the email
--					addresses.
-- ============================================================================
-- Modified By:		GitHub Copilot
-- Modified Date:	2025-10-11
-- Description:		Added pagination parameters and logic.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_user_sp_GetMailingListByRoles_pagination]
(
	@compo AS VARCHAR(10),
	@unitId AS INT,
	@roles AS VARCHAR(100),
	@PageNumber INT = 1,
	@PageSize INT = 10
)
AS
BEGIN
	SET NOCOUNT ON;

	WITH AllEmails(Email, Email2, Email3) AS
	(
		SELECT	a.Email ,a.Email2,a.Email3
		FROM	vw_users a
				JOIN core_UserRoles ur ON a.userID = ur.userID
		WHERE
				a.compo = @compo
			AND
				a.accessStatus=3
			AND
				(ur.groupid IN (SELECT value FROM dbo.split(@roles, ',')))
			AND
			(
			CASE
				WHEN ur.groupid = 1 THEN a.unit_Id
				ELSE @unitId
				END
				= a.unit_Id

			)
			AND    a.receiveEmail = 1
	)

	SELECT  Email FROM AllEmails WHERE Email IS NOT NULL
	UNION
	SELECT	Email2 FROM AllEmails WHERE Email2 IS NOT NULL
	UNION
	SELECT	Email3 FROM AllEmails WHERE Email3 IS NOT NULL
	ORDER BY Email
	OFFSET (@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
END
GO