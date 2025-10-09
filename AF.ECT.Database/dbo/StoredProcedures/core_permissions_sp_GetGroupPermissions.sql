CREATE PROCEDURE [dbo].[core_permissions_sp_GetGroupPermissions]
	@groupId smallint
AS

SELECT a.permid, a.permName, a.permDesc, 
	CAST (CASE b.groupId
		WHEN @groupId THEN 1
		ELSE 0
	END AS BIT) AS Allowed, 
	exclude, @groupId AS GroupId
FROM 
	[core_Permissions] a
JOIN 
	core_GroupPermissions b ON a.permId = b.permId AND b.groupId = @groupId
ORDER BY 
	a.permName

--[core_permissions_sp_GetByGroup] 4
GO

