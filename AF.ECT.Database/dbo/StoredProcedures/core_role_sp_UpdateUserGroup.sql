

--exec core_role_sp_UpdateUserGroup 11,11,3,11
CREATE PROCEDURE [dbo].[core_role_sp_UpdateUserGroup]
	@roleId int,
	@groupId tinyint,
	@status tinyint,
	@userId int
AS


SET NOCOUNT ON;

SET XACT_ABORT ON 
BEGIN
	BEGIN TRANSACTION 
	--The user roles should only be allowed two roles IO and anyother role
	DECLARE  @existRoleId int 
	
	IF @groupId <> 14 
		BEGIN 
			--if  the new role required is non io and that exist before but is not active
			--get that role and make it active so we do not create extra role
			SELECT @existRoleId=( SELECT userRoleId from core_UserRoles where userId=@userId and groupId<>14 )
		END
	ELSE
		BEGIN
		--if  the new role required is io and that exist via assign io or otherwise	get that role
			SELECT @existRoleId=( SELECT userRoleId from core_UserRoles where userId=@userId and groupId=@groupId )
		END
	--if the role does not exist ,create it with the reqd status
	IF @existRoleId IS NULL 
	BEGIN
		---If there is no other role which is non io create one.
				INSERT INTO CORE_USERROLES(groupId,userId,status,active)
				VALUES (@groupId,@userId,@status,1)
	--Now get the role id
			 SELECT @existRoleId=( SELECT userRoleId from core_UserRoles where userId=@userId and groupId=@groupId)

	 END 
	 
	 
		
	--All groups should have the same status as the account status
 			UPDATE CORE_USERROLES 
				SET 
				 status=@status
			WHERE 
				userId=@userId

    --Now a role must exist  so update the currentRoleId with that and also update the other roles as inactive 
			UPDATE CORE_USERS
				SET currentRole=@existRoleId 		 
			WHERE 
				userId=@userId	 
		--Make sure this role is  active --if we did not create this it will be inactive in the system
			UPDATE CORE_USERROLES
					SET
					 groupId=@groupId,
					 active=1					
				WHERE 
					userRoleId= @existRoleId
					AND  userId=@userId	 

			UPDATE CORE_USERROLES
					SET					 
					 active=0					
				WHERE 
					userRoleId <> @existRoleId
					AND  userId=@userId	 

		 
		 
  COMMIT   
END --END STORED PROC
GO

