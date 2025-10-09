

--EXEC core_user_sp_GetWhois 1

CREATE PROCEDURE [dbo].[core_user_sp_GetWhois]
	@userId int

AS

SET NOCOUNT ON

DECLARE @show bit, @roleId int
SET @roleId = (SELECT TOP 1 userRoleId FROM core_userRoles WHERE userId = @userId AND active = 1)

SET @show = (SELECT showInfo FROM core_UserGroups WHERE groupId = (
	SELECT groupId FROM core_userRoles WHERE userRoleId = @roleId)
)

IF (@show = 0)
	BEGIN 
		--do not show all this users info
		SELECT 
			a.UserId , Rank, FirstName, LastName 
			,'' 'Address', '' 'City'
			,'' 'State', '' 'Zip'
			,'' 'Phone', '' 'DSN', '' 'Email'
			,c.name 'Role', '' 'Region'
		FROM 
			vw_users a
		JOIN 
			core_UserRoles b ON b.userRoleID = @roleId
		JOIN 
			core_UserGroups c ON c.groupId = b.groupId
		WHERE 
			a.userID = @userid
	END
ELSE
	BEGIN

		SELECT 
			a.UserId , Rank, FirstName, LastName 
			,a.work_street 'Address', a.work_city 'City'
			,a.work_State 'State', a.work_zip 'Zip'
			,Phone 'Phone', DSN 'DSN', a.Email 'Email'
			,c.name 'Role'
		FROM 
			vw_users a
		JOIN 
			core_UserRoles b ON b.userRoleID = @roleId
		JOIN 
			core_UserGroups c ON c.groupId = b.groupId
		WHERE 
			a.userID = @userid
	END
GO

