

CREATE PROCEDURE [dbo].[core_workstatus_sp_GetByWorkflow]
	@workflow int

AS

SELECT 
	a.ws_id, a.workflowId, a.statusId, a.sortOrder
	,s.description, s.groupId, g.name
FROM core_WorkStatus a
JOIN vw_WorkStatus s ON s.ws_id = a.ws_id
LEFT JOIN core_UserGroups g ON g.groupId = s.groupId
WHERE a.workflowId = @workflow
ORDER BY a.sortOrder
GO

