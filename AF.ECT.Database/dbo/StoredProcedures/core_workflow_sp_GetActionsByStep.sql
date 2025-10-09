

CREATE PROCEDURE [dbo].[core_workflow_sp_GetActionsByStep]
	@stepId int

AS

SELECT 
	a.wso_id, a.wsa_id, a.actionType, a.target, a.data, text
FROM 
	core_WorkStatus_Actions a
JOIN 
	core_lkupWorkflowAction ON core_lkupWorkflowAction.type = a.actionType
WHERE 
	wso_id = @stepId
GO

