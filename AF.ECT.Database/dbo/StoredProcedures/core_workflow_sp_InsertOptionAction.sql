

CREATE PROCEDURE [dbo].[core_workflow_sp_InsertOptionAction]
	@type tinyint
	,@wsoid int
	,@target int
	,@data int

AS

INSERT INTO core_WorkStatus_Actions
(actionType, wso_id, target, data)
VALUES
(@type, @wsoid , @target, @data)

SELECT SCOPE_IDENTITY()
GO

