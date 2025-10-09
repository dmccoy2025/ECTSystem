

CREATE PROCEDURE [dbo].[core_workflow_sp_InsertAction]
	@type tinyint
	,@stepId smallint
	,@target int
	,@data int

AS

INSERT INTO core_WorkflowActions
(type, stepId, target, data)
VALUES
(@type, @stepId, @target, @data)

SELECT SCOPE_IDENTITY()
GO

