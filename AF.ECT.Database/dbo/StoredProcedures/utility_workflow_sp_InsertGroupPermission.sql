
-- ============================================================================
-- Author:		Kenneth Barnett
-- Create date: 4/11/2014
-- Description:	Insert a new record into the core_GroupPermissions table. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	9/22/2015
-- Work Item:		TFS Task 354
-- Description:		Cleaned up the stored procedure. Added input validation
--					and some additional validation.  
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	11/16/2015
-- Work Item:		TFS Task 377
-- Description:		Changed the size of the @groupName parameter from 50 to
--					100. 
-- ============================================================================
CREATE PROCEDURE [dbo].[utility_workflow_sp_InsertGroupPermission]
	 @groupName VARCHAR(100)
	,@permName VARCHAR(50)
AS

IF (ISNULL(@groupName, '') = '')
	BEGIN
	RAISERROR('Invalid Parameter: @groupName cannot be NULL or zero.', 18, 0)
	RETURN
	END

IF (ISNULL(@permName, '') = '')
	BEGIN
	RAISERROR('Invalid Parameter: @permName cannot be NULL or zero.', 18, 0)
	RETURN
	END


DECLARE @groupId TINYINT = 0
DECLARE @permId SMALLINT = 0


SELECT	@groupId = groupId
FROM	core_UserGroups
WHERE	name = @groupName

IF (ISNULL(@groupId, 0) = 0)
	BEGIN
	PRINT 'Could not find User Group with name ' + @groupName + '.'
	RETURN
	END


SELECT	@permId = permId 
FROM	core_Permissions
WHERE	permName = @permName

IF (ISNULL(@permId, 0) = 0)
	BEGIN
	PRINT 'Could not find Permission with name ' + @permName + '.'
	RETURN
	END


IF NOT EXISTS(SELECT * FROM core_GroupPermissions WHERE groupId = @groupId AND permId = @permId)
	BEGIN
		INSERT	INTO core_GroupPermissions ([groupId], [permId]) 
		VALUES	(@groupId, @permId)
		
		PRINT 'Inserted new values (' + CONVERT(VARCHAR(3), @groupId) + ', ' + 
										CONVERT(VARCHAR(3), @permId) + ') into core_GroupPermissions table.'
										
		-- VERIFY NEW GROUP PERMISSION
		SELECT GP.*
		FROM core_GroupPermissions GP
		WHERE GP.groupId = @groupId AND GP.permId = @permId
	END
ELSE
	BEGIN
		PRINT 'The group permission pair (' + @groupName + ', ' + @permName + ') already exist in core_GroupPermissions table.'
	END
GO

