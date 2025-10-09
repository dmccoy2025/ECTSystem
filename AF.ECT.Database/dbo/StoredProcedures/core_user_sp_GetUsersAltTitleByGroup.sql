
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 11/11/2015
-- Description:	Selects users and their titles based on the passed in user
--				group Id. If a user does not have a title then null is selected.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_user_sp_GetUsersAltTitleByGroup]
	@groupId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- Validate input...
	IF (ISNULL(@groupId, 0) = 0)
	BEGIN
		--RAISERROR('Invalid parameter: @groupId cannot be NULL or zero!', 18, 0)
		RETURN
	END
	
	DECLARE @accessStatus INT = 0
	
	SELECT	@accessStatus = a.statusId
	FROM	core_lkupAccessStatus a
	WHERE	a.description = 'Approved'

	SELECT	u.userID, u.username, u.FirstName + ' ' + u.LastName AS Name, t.Title
	FROM	core_Users u
			JOIN core_UserRoles ur ON u.userID = ur.userID
			LEFT JOIN core_UsersAltTitles t ON u.userID = t.UserId
	WHERE	ur.groupId = @groupId
			AND u.accessStatus = @accessStatus
	ORDER BY u.LastName
END
GO

