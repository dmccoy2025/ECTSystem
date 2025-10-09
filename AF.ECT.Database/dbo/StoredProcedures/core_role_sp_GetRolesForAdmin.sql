-- =============================================
-- Author:		Nandita Srivastava
-- Create date: 26 March 2008
-- Description:	Retrieves user role information for a userId based on the current administrator 
-- =============================================
CREATE PROCEDURE [dbo].[core_role_sp_GetRolesForAdmin] 
	-- Add the parameters for the stored procedure here
	@userId int,
    @adminUserId int 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
SET NOCOUNT ON;
--Get current role, groupid, and access scope for the administrator 
DECLARE @adminCurrentRoleId int, @adminCurrentGroupId int, @adminCurrentScope tinyint

SELECT TOP 1
	@adminCurrentRoleId = b.userRoleId, @adminCurrentGroupId = b.groupId
	,@adminCurrentScope = c.accessscope
FROM 
	core_userroles b
JOIN
	core_UserGroups c on b.groupId = c.groupId
WHERE 
	b.userID = @adminUserID   
AND
	b.active = 1



--unit level users
IF (@adminCurrentScope =1)
	BEGIN 
		SELECT a.userRoleId, a.groupId, f.name, f.abbr,a.status, f.accessScope,0 as UicCount
			FROM dbo.core_UserRoles a  
		
		INNER JOIN 
			dbo.core_UserGroups f ON f.groupId = a.groupID

			WHERE a.userRoleID IN (
				SELECT userRoleId FROM core_UserRoleUICs
				WHERE (													--Gets the user role ids which falls in the approved regions for the admin 's current role 
						UIC IN (SELECT DISTINCT UIC FROM dbo.core_fn_UICs(@adminCurrentGroupId,@adminCurrentRoleId)) 
					
					/*(
						SELECT DISTINCT UIC FROM core_UserRoleUICs
						WHERE userRoleId = @adminCurrentRoleId
						AND Status = 2
					)*/
				)
			)
			AND a.groupId IN (										--Gets only the roles which can be managed by the current user
				SELECT groupId FROM core_UserGroupsManagedBy 
				WHERE managedBy = @adminCurrentGroupId
			)
			AND (	a.userID=@userId  )AND (	a.status!=5  )		 --Get the user role ids for the userid and  --Get the user roles which are not deleted 
	END
---Region level users
IF (@adminCurrentScope =2)
	BEGIN 
		SELECT a.userRoleId, a.groupId, f.name, f.abbr,a.status, f.accessScope,0 as UicCount
			FROM dbo.core_UserRoles a  
		
		INNER JOIN 
			dbo.core_UserGroups f ON f.groupId = a.groupID

			WHERE a.userRoleID IN (
				SELECT userRoleId FROM core_UserRoleUICs
			WHERE (													--Gets the user role ids which falls in the approved regions for the admin 's current role 
					Region IN (
						SELECT DISTINCT region FROM core_UserRoleUICs
						WHERE userRoleId = @adminCurrentRoleId
						AND Status = 2
					)
				)
			)
			AND a.groupId IN (										--Gets only the roles which can be managed by the current user
				SELECT groupId FROM core_UserGroupsManagedBy 
				WHERE managedBy = @adminCurrentGroupId
			)
			AND (	a.userID=@userId  )AND (	a.status!=5  )		 --Get the user role ids for the userid and  --Get the user roles which are not deleted 
	END
--Global users 
IF (@adminCurrentScope IN (3,4))
	BEGIN
		SELECT a.userRoleId, a.groupId, f.name, f.abbr,a.status, f.accessScope,0 as UicCount
				FROM dbo.core_UserRoles a  
			INNER JOIN 
				dbo.core_UserGroups f ON f.groupId = a.groupID

				WHERE a.groupId IN 
				(
					SELECT DISTINCT groupId FROM core_UserGroupsManagedBy WHERE managedBy = @adminCurrentGroupId
				)
				AND a.groupId != 1

--				WHERE a.userRoleID IN 
--				(
--					SELECT DISTINCT userRoleId FROM core_UserRoleUICs
--						WHERE  
--							a.groupId IN (										--Gets only the roles which can be managed by the current user
--							SELECT groupId FROM core_UserGroupsManagedBy 
--							WHERE managedBy = @adminCurrentGroupId
--						)
				AND (	a.userID=@userId  )AND (	a.status!=5  )		 --Get the user role ids for the userid and  --Get the user roles which are not deleted 

--				)

	 END 
	 
END
GO

