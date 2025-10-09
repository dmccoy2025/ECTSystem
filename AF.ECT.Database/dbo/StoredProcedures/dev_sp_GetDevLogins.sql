--dev_sp_GetDevLogins '6'

CREATE PROCEDURE [dbo].[dev_sp_GetDevLogins]
	@compo char(1),
	@unit int,
	@board bit

AS


--DECLARE @compo char(1), @unit int
--
--SET @compo = '6'
--SET @unit = 20151550

DECLARE @users TABLE
(
	userId int,
	edipin varchar(50),
	name varchar(50)
)

INSERT INTO @users
SELECT 
	b.userID, b.edipin, g.name + ' - ' + b.username 
FROM 
	core_users b 
JOIN 
	core_userRoles r ON r.userRoleID = b.currentRole
JOIN
	core_userGroups g ON g.groupId = r.groupId
WHERE 
	b.workCompo = @compo
AND
	b.cs_id IN (SELECT child_id FROM Command_Struct_Tree WHERE parent_id = @unit AND view_type = 2)
AND
	g.accessScope = 1
AND 
	b.accessStatus = 3

IF (@board = 1)
BEGIN

	INSERT INTO @users
	SELECT 
		b.userID, b.edipin, g.name + ' - ' + b.username 
	FROM 
		dev_logins a
	JOIN 
		core_users b ON b.userID = a.userId
	JOIN 
		core_userRoles r ON r.userRoleID = a.roleId
	JOIN
		core_userGroups g ON g.groupId = r.groupId
	WHERE 
		a.compo = @compo
	AND
		g.accessScope = 2

END 

SELECT userId, edipin , name + '('+ edipin +')'  'name'  FROM @users ORDER BY name
GO

