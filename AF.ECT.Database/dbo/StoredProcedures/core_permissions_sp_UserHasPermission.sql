

CREATE PROCEDURE [dbo].[core_permissions_sp_UserHasPermission]
	@userId int,
	@permName varchar(50),
	@hasPerm bit out 

AS

SET NOCOUNT ON

--DECLARE @permName varchar(50)
--SET @permName = 'sysAdmin'
 
DECLARE @perms TABLE
(
	permId smallint,
	permName varchar(50),
	permDesc varchar(100),
	allowed bit,
	exclude bit
)

DECLARE @groupId int, @allowed bit
SET @allowed = 1 --this is added for consistency with the reader

--get the current role for this user
SET @groupId = (
	SELECT groupId FROM core_UserRoles WHERE userRoleId = 
		(SELECT TOP 1 userRoleId FROM dbo.core_userRoles WHERE userID = @userId AND active = 1)
)

--get the permissions for that role
INSERT INTO @perms
SELECT 
	a.permId, permName, permDesc, 1, exclude
FROM 
	core_Permissions a
JOIN 
	core_GroupPermissions b ON b.permId = a.permId
WHERE 
	b.groupId = @groupId
	
UNION 

--now add any permissions granted to this user
--(this is part of the above INSERT)
SELECT 
	a.permId, permName, permDesc, 1, exclude
FROM 
	core_Permissions a
JOIN 
	core_UserPermissions b ON b.permId = a.permId
WHERE 
	b.userId = @userId
AND
	b.status = 'G' --granted
AND
	a.exclude = 0

--now delete any revoked permissions
DELETE FROM @perms
WHERE permId IN (
	SELECT permId FROM core_userPermissions
	WHERE userId = @userId
	AND status = 'R' --revoked
)
 

IF EXISTS (SELECT permId FROM @perms WHERE permName LIKE @permName)
	BEGIN 
		SET @hasPerm = 1
	END 
ELSE
	BEGIN
		SET @hasPerm = 0
	END
GO

