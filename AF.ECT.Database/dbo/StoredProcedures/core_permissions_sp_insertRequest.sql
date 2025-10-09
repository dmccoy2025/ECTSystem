
CREATE PROCEDURE [dbo].[core_permissions_sp_insertRequest]
	@userId int,
	@permId smallint

AS

IF NOT EXISTS (SELECT reqId FROM core_permissionRequests WHERE userId = @userID AND permId = @permId)
BEGIN
	INSERT INTO core_permissionRequests (userId, permId) VALUES (@userId, @permId)
END
GO

