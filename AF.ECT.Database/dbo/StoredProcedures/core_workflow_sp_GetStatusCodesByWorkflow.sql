


CREATE PROCEDURE [dbo].[core_workflow_sp_GetStatusCodesByWorkflow]
	@workflowId tinyint

AS

SET NOCOUNT ON


SELECT 
	a.statusId, a.description, a.moduleId, a.compo, 
	ISNULL(a.groupId,0) groupId, a.isFinal, a.isApproved, a.canAppeal 
	,ISNULL(b.name,'None') groupName, c.moduleName, d.compo_descr
	,CASE WHEN a.groupId IS NULL THEN 
		a.description
	ELSE
		a.description + ' (' + b.name + ')' 
	END 
		fullStatus, displayOrder
FROM 
	core_StatusCodes a 
LEFT JOIN
	core_UserGroups b ON a.groupId = b.groupId
JOIN
	core_lkupModule c ON c.moduleId = a.moduleId
JOIN
	core_lkupCompo d ON d.compo = a.compo
WHERE 
	a.statusId IN (
		SELECT DISTINCT statusIn FROM core_workflowsteps WHERE workflowId = @workflowId
		UNION
		SELECT DISTINCT statusOut FROM core_workflowsteps WHERE workflowId = @workflowId
	)
ORDER BY displayOrder
GO

