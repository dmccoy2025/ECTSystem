-- ============================================================================
-- Author:		Andy Cooper
-- Create date: 27 March 2008
-- Description:	Inserts a new permission
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	6/22/2015
-- Work Item:		TFS Task 319
-- Description:		The system admin group permission insert was using a hard
--					coded value of 2 for the groupId which is bad practice and
--					in this case was the incorrect id for the system admin group.
--					Altered the stored procedure to lookup the id of the system
--					admin user group and use that value for the group permissions
--					record.  
-- ============================================================================
CREATE PROCEDURE [dbo].[core_permissions_sp_Insert] 
	-- Add the parameters for the stored procedure here
	@name varchar(50),
	@description varchar(100),
	@exclude bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO dbo.core_Permissions
	(permName, permDesc, exclude)
	VALUES
	(@name, @description, @exclude)
	
	DECLARE @groupId INT = 0
	SELECT @groupId = groupId FROM core_UserGroups WHERE name = 'System Administrator'

	--sys admin always gets permissions
	INSERT INTO dbo.core_GroupPermissions
	(groupId, permId)
	VALUES
	(@groupId, SCOPE_IDENTITY())

END
GO

