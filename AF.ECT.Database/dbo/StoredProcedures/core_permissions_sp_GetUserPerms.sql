

CREATE PROCEDURE [dbo].[core_permissions_sp_GetUserPerms]
	@userId int

AS

SELECT 
	a.permId, permName, permDesc, status, exclude
FROM 
	core_Permissions a
JOIN 
	core_UserPermissions b ON b.permId = a.permId
WHERE 
	b.userId = @userId
GO

