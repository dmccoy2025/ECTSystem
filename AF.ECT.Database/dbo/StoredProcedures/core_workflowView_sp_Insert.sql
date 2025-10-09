CREATE PROCEDURE [dbo].[core_workflowView_sp_Insert]
(
	@pageId as int,
	@workflowId as int

)

AS
	SET NOCOUNT ON;


IF EXISTS (SELECT 1 FROM dbo.core_WorkflowViews WHERE (pageId = @pageId) AND (workflowId = @workflowId))
RETURN 0
ELSE
	BEGIN
		INSERT INTO dbo.core_WorkflowViews
							  (pageId, workflowId)
		VALUES     (@pageId,@workflowId)
	END
GO

