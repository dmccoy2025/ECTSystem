
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 3/31/2017
-- Description:	Get Workflow id from Module id
-- ============================================================================
-- Edited:		Evan Morrison
-- Edited Date:	4/12/2017
-- Description:	if moduleId of 0 is passed in it does not return an error
-- ============================================================================
CREATE PROCEDURE [dbo].[core_workflow_sp_GetWorkflowFromModule]
	@moduleId INT
AS
	
	DECLARE @workflow INT

	DECLARE @WF TABLE( 
		workflowId int)
	
	INSERT INTO @WF (workflowId)
		SELECT workflowId FROM core_workflow WHERE moduleId  = @moduleId ORDER BY workflowId DESC


	SELECT TOP 1 * FROM @WF ORDER BY workflowId DESC
GO

