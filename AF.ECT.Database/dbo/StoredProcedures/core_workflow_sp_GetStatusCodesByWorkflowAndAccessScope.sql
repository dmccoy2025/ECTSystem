


CREATE PROCEDURE [dbo].[core_workflow_sp_GetStatusCodesByWorkflowAndAccessScope]
	@workflowId tinyint,
	@accessScope tinyint

AS

 

SET NOCOUNT ON


SELECT 
	a.statusId, a.description 
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
	AND 
	 ( 
		b.accessScope <= @accessScope
	 )
GO

