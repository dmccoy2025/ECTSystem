
CREATE PROCEDURE [dbo].[utility_workflow_sp_InsertWorkflow] 
	@workflowId TINYINT, 
	@moduleId TINYINT, 
	@compo CHAR(1), 
	@title VARCHAR(50), 
	@formal BIT, 
	@active BIT, 
	@initialStatus INT

AS

IF NOT EXISTS (SELECT * FROM core_Workflow WHERE workflowId = @workflowId AND moduleId = @moduleId)

	BEGIN

		SET NOCOUNT ON;		
		SET IDENTITY_INSERT [dbo].[core_Workflow] ON
		INSERT [dbo].[core_Workflow] ([workflowId], [moduleId], [compo], [title], [formal], [active], [initialStatus]) 
			VALUES (@workflowId, @moduleId, @compo, @title, @formal, @active, @initialStatus)
		SET IDENTITY_INSERT [dbo].[core_Workflow] OFF	
		
		PRINT 'Inserted new values ' + CONVERT(VARCHAR(2),@workflowId) + ',' +
									CONVERT(VARCHAR(2),@moduleId) + ',' +
									CONVERT(VARCHAR(2),@compo) + ',' +
									CONVERT(VARCHAR(50),@title) + ',' +
									CONVERT(VARCHAR(2),@formal) + ',' +
									CONVERT(VARCHAR(2),@active) + ',' +
									CONVERT(VARCHAR(3),@initialStatus)

	END
	
ELSE
	BEGIN
		PRINT @title + 'already exists in core_Workflow'
	END
GO

