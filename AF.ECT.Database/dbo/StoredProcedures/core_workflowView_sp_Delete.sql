CREATE PROCEDURE [dbo].[core_workflowView_sp_Delete]
(
	@pageId as int,
	@workflowId as int

)

AS
	SET NOCOUNT ON;

		DELETE FROM dbo.core_WorkflowViews
		WHERE (workflowId = @workflowId) AND (pageId = @pageId)
GO

