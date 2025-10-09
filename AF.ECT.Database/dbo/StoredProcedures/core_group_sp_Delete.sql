-- =============================================
-- Author:		Nick McQuillen
-- Create date: 3/27/2008
-- Description:	Delete's a group record
-- =============================================
CREATE PROCEDURE [dbo].[core_group_sp_Delete]
	@id int
AS
BEGIN

	--this will fail if a user is currently assigned a role with this group
	--which is what we want to happen
	SET XACT_ABORT ON

	BEGIN TRANSACTION

	--first we have to delete this group from the managed by table
	DELETE FROM dbo.core_UserGroupsManagedBy
	WHERE groupId = @id
	OR managedBy = @id

	--now delete any roles that are using this group
	DELETE FROM dbo.core_UserRoleUICs 
	WHERE userRoleId IN (
		SELECT userRoleId FROM core_UserRoles WHERE groupId = @id)

	DELETE FROM dbo.core_UserRoles 
	WHERE groupId = @id

	--Delete from user group permissions
	DELETE FROM dbo.core_GroupPermissions
	WHERE groupid = @id

	DELETE FROM dbo.core_PageAccess 
	WHERE groupId = @id

	DELETE FROM dbo.core_WorkflowPerms
	WHERE groupId = @id

	--finally delete the group
	DELETE FROM dbo.core_UserGroups 
	WHERE groupId=@Id

	COMMIT TRANSACTION

END
GO

