


-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 6/19/2017
-- Description:	Insert a new record into the core_WorkflowPermissions table. 
-- ============================================================================
-- ============================================================================
-- Author:		Darel Johnson
-- Create date: 02/05/2020
-- Description:	Wrong table was referenced in Insert statement. Change from core_GroupPermissions to core_WorkflowPermissions table. 
-- ============================================================================
CREATE PROCEDURE [dbo].[utility_workflow_sp_InsertWorkflowPermission]
	 @workflow VARCHAR(100)
	,@permName VARCHAR(50)
AS

IF (ISNULL(@workflow, '') = '')
	BEGIN
	RAISERROR('Invalid Parameter: @workflow cannot be NULL or zero.', 18, 0)
	RETURN
	END

IF (ISNULL(@permName, '') = '')
	BEGIN
	RAISERROR('Invalid Parameter: @permName cannot be NULL or zero.', 18, 0)
	RETURN
	END


DECLARE @workflowId TINYINT = 0
DECLARE @permId SMALLINT = 0


SELECT	@workflowId = workflowId
FROM	core_Workflow
WHERE	title = @workflow

IF (ISNULL(@workflowId, 0) = 0)
	BEGIN
	PRINT 'Could not find User Group with name ' + @workflow + '.'
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


IF NOT EXISTS(SELECT * FROM core_WorkflowPermissions WHERE workflowId = @workflowId AND permId = @permId)
	BEGIN
		INSERT	INTO core_WorkflowPermissions ([workflowId], [permId]) 
		VALUES	(@workflowId, @permId)
		
		PRINT 'Inserted new values (' + CONVERT(VARCHAR(3), @workflowId) + ', ' + 
										CONVERT(VARCHAR(3), @permId) + ') into core_WorkflowPermissions table.'
										
		-- VERIFY NEW GROUP PERMISSION
		SELECT GP.*
		FROM core_WorkflowPermissions GP
		WHERE GP.workflowId = @workflowId AND GP.permId = @permId
	END
ELSE
	BEGIN
		PRINT 'The group permission pair (' + @workflow + ', ' + @permName + ') already exist in core_WorkflowPermissions table.'
	END
GO

