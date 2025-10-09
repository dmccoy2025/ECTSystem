
CREATE VIEW [dbo].[vw_rr_WorkStatuses]
AS
SELECT	a.ws_id, a.workflowId, a.statusId, a.sortOrder, s.description, s.moduleId, s.compo, s.isApproved, s.isFinal, s.canAppeal, s.displayOrder, s.groupId, s.filter
FROM	dbo.core_WorkStatus AS a INNER JOIN
		dbo.core_StatusCodes AS s ON s.statusId = a.statusId INNER JOIN
		dbo.core_lkupModule AS m ON m.moduleId = s.moduleId
WHERE	m.moduleName = 'Reinvestigation Request'
GO

