-- ============================================================================
-- Author:		Kenneth Barnett
-- Create date: 7/28/2016
-- Description:	Inserts a new record into the core_WorkflowViews table.
-- ============================================================================
CREATE PROCEDURE [dbo].[utility_workflow_sp_InsertWorkflowViewByWorkflowId]
	 @workflowId INT
	,@page VARCHAR(50)
AS

DECLARE @pageId INT


SELECT @pageId = pageId FROM core_Pages
WHERE title = @page


IF NOT EXISTS(SELECT * FROM dbo.core_WorkflowViews WHERE workflowId = @workflowId AND pageId = @pageId)
	BEGIN
		INSERT INTO [dbo].core_WorkflowViews ([workflowId], [pageId]) 
			VALUES (@workflowId, @pageId)
		
		PRINT 'Inserted new values (' + CONVERT(VARCHAR(3), @workflowId) + ', ' + 
										CONVERT(VARCHAR(3), @pageId) + ') into core_WorkflowViews table.'
										
		-- VERIFY NEW PAGE
		SELECT WV.*
		FROM core_WorkflowViews WV
		WHERE WV.workflowId = @workflowId AND WV.pageId = @pageId
	END
ELSE
	BEGIN
		PRINT 'Values already exist in core_WorkflowViews table.'
	END
GO

