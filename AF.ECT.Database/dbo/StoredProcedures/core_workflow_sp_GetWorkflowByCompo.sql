CREATE PROCEDURE [dbo].[core_workflow_sp_GetWorkflowByCompo]
(
	@compo CHAR(2),
	@userId int
)

AS
	SET NOCOUNT ON;

DECLARE @module INT, @canView INT
SET @module = 2
SET @canView = 1

DECLARE @currentGroupId INT
SELECT TOP 1
			 @currentGroupId = groupId
		FROM 
			core_UserRoles 
		WHERE 
			userID = @userId
		AND
			active = 1


SELECT     dbo.core_WorkflowPerms.workflowId, dbo.core_Workflow.title
FROM         dbo.core_WorkflowPerms INNER JOIN
                      dbo.core_Workflow ON dbo.core_WorkflowPerms.workflowId = dbo.core_Workflow.workflowId
WHERE     (dbo.core_WorkflowPerms.canView = @canView) AND (dbo.core_Workflow.moduleId = @module) AND (dbo.core_WorkflowPerms.groupId = @currentGroupId)
ORDER BY 2
GO

