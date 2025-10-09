CREATE PROCEDURE [dbo].[core_workflow_sp_GetCreatableByGroup]
	@compo char(1),
	@module tinyint,
	@groupId tinyint

AS

SELECT 
	a.workFlowId, a.compo, title, formal, a.moduleId, active, initialStatus, b.description 
FROM 
	core_Workflow a 
LEFT JOIN 
	vw_workstatus b ON b.ws_id = a.initialStatus 
JOIN 
	core_Permissions p ON p.permName = 'lodCreate'
JOIN 
	core_GroupPermissions gp ON gp.permId = p.permId
WHERE 
	a.compo = @compo
AND 
	a.moduleId = @module
AND
	gp.groupId = @groupId
AND
	a.workflowId = 27
GO

