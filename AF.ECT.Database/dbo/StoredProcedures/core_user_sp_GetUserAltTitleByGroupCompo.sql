-- =============================================
-- Author:		Michael van Diest
-- Create date: 7/30/2020
-- Description:	Modified version of core_user_sp_GetUsersAltTitleByGroup to also check for Work Compo
-- =============================================
CREATE PROCEDURE [dbo].[core_user_sp_GetUserAltTitleByGroupCompo]
	@groupId INT,
	@workCompo INT
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

	IF (ISNULL(@workCompo, 0) = 0)
	BEGIN
		--RAISERROR('Invalid parameter: @workCompo cannot be NULL or zero!', 18, 0)
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
			AND u.workCompo = @workCompo
	ORDER BY u.LastName
END
GO

