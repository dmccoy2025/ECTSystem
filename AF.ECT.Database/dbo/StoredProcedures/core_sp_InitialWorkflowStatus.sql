create PROCEDURE [dbo].[core_sp_InitialWorkflowStatus]
(
	@compo INT,
	@moduleId INT
)

AS

SELECT     workflowId, initialStatus
FROM         dbo.core_Workflow
WHERE     (compo = @compo) AND (moduleId = @moduleId) AND (active = 1)
GO

