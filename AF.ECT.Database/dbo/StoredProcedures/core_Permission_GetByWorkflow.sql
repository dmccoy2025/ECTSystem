
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 6/9/2017
-- Description:	Selects the Permission names associated with the specified
--				User Group for the specified Workflow.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_Permission_GetByWorkflow]
	@groupId int,
	@workflowId int
AS
BEGIN
	SELECT	p.permName
	FROM	core_GroupPermissions gp
			JOIN core_Permissions p ON p.permId = gp.permId
			JOIN core_WorkflowPermissions wp ON wp.permId = p.permId
	WHERE	gp.groupId = @groupId
			AND wp.workflowId = @workflowId

END
GO

