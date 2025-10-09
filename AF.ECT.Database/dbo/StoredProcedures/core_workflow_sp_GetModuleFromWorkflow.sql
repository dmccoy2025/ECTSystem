
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 3/31/2017
-- Description:	Get Module id from Workflow id
-- ============================================================================
CREATE PROCEDURE [dbo].[core_workflow_sp_GetModuleFromWorkflow]
	@workflowId INT
AS

DECLARE @module INT

DECLARE @WF TABLE( 
	moduleId int)
	
INSERT INTO @WF (moduleId)
	SELECT moduleId FROM core_workflow WHERE workflowId  = @workflowId ORDER BY workflowId DESC


SET @module = (SELECT TOP 1 * FROM @WF ORDER BY moduleId DESC)

SELECT @module
GO

