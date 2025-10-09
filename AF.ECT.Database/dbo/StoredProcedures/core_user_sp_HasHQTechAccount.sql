
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 9/18/2015
-- Description:	Counts the number of accounts associated with the specified
--				EDIPIN ID value that has an HQ AFRC Technician role associated
--				with the account. This does not include the origin account
--				this call is originating from. NOTE: Account is equivalent to
--				user. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_user_sp_HasHQTechAccount]
	@originUserId INT,
	@userEDIPIN VARCHAR(100)
AS
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT	COUNT(DISTINCT u.userID)
	FROM	core_Users u
			JOIN core_UserRoles ur ON u.userID = ur.userID
			JOIN core_UserGroups ug ON ur.groupId = ug.groupId
	WHERE	u.userID <> @originUserId
			AND u.EDIPIN = @userEDIPIN
			AND ug.name = 'HQ Medical Technician'

END
GO

