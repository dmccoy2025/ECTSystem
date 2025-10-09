CREATE PROCEDURE [dbo].[core_workflow_sp_GetPagesByWorkflowId]
(
	@workflowId as int

)

AS
	SET NOCOUNT ON;


SELECT     dbo.core_WorkflowViews.pageId, dbo.core_Pages.title
FROM         dbo.core_WorkflowViews INNER JOIN
                      dbo.core_Pages ON dbo.core_WorkflowViews.pageId = dbo.core_Pages.pageId
WHERE     (dbo.core_WorkflowViews.workflowId = @workflowId)
ORDER BY dbo.core_WorkflowViews.pageId
GO

