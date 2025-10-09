

CREATE PROCEDURE [dbo].[core_workflow_sp_GetViewableByGroup]
	@groupId tinyint,
	@module tinyint

AS

SELECT 
	a.workFlowId, a.compo, title, formal, a.moduleId, active, initialStatus, b.description 
FROM 
	core_Workflow a 
LEFT JOIN 
	core_StatusCodes b ON b.statusId = a.initialStatus 
JOIN 
	core_workflowperms c ON c.workflowId = a.workflowId 
		AND c.groupId = @groupId 
		AND c.canView = 1
WHERE 
	a.moduleId = @module
ORDER BY a.workflowId
GO

