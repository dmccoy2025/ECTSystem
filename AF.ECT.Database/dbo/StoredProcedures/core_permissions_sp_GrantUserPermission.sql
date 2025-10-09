
CREATE PROCEDURE [dbo].[core_permissions_sp_GrantUserPermission]
	@userId int,
	@permId smallint

AS

IF EXISTS (SELECT userId FROM core_UserPermissions WHERE userId = @userId AND permId = @permId)
	UPDATE core_UserPermissions SET status = 'G' WHERE userId = @userId AND permId = @permId
ELSE
	INSERT INTO core_userPermissions (userId, permId, status) VALUES (@userId, @permId, 'G')
GO

