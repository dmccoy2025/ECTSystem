
CREATE VIEW [dbo].[vw_sc_workflows]
AS
SELECT	w.workflowId, w.title, m.moduleId, m.moduleName, w.compo
FROM	dbo.core_Workflow AS w INNER JOIN
		dbo.core_lkupModule AS m ON m.moduleId = w.moduleId
WHERE	m.isSpecialCase = 1 -- (w.title != 'Line of Duty' AND w.title != 'Reinvestigation Request' AND w.title != 'Military Medical Support Office (MMSO)')
		AND m.moduleName <> 'Military Medical Support Office'
GO

