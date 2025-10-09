

CREATE PROCEDURE [dbo].[core_workstatus_sp_GetOptionsByWorkStatus]
	@workstatus int,
	@compo int
AS

SELECT 
	a.wso_id, a.ws_id, a.ws_id_out, a.displayText, a.active
	,g.groupId, g.name, a.sortOrder, a.template, s.description AS statusOutText, a.compo
FROM core_WorkStatus_Options a
JOIN core_WorkStatus w ON w.ws_id = a.ws_id
JOIN vw_WorkStatus s ON s.ws_id = a.ws_id_out
LEFT JOIN core_usergroups g ON g.groupId = s.groupId
WHERE a.ws_id = @workstatus and (a.compo = @compo or a.compo = 0)
GO

