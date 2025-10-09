


CREATE VIEW [dbo].[vw_sc_workflows_subtypes]
AS

-- Create a common table expression (CTE) to act as a temporary table
WITH Import (workflowId, title)
	AS
	(
		-- Select special case workflows that do not have sub workflow types. 
		SELECT	w.workflowId, w.title
		FROM	dbo.core_Workflow AS w INNER JOIN
				dbo.core_lkupModule AS m ON m.moduleId = w.moduleId
		WHERE	m.isSpecialCase = 1 -- (w.title != 'Line of Duty' AND w.title != 'Reinvestigation Request' AND w.title != 'MEPS (MP)'
				AND m.moduleName <> 'MEPS'
				AND w.title NOT IN (select s.subTypeTitle from core_lkupSCSubType AS s)
				
		UNION
		
		-- Select special case sub type workflows
		SELECT	s.subTypeId, s.subTypeTitle
		FROM	core_lkupSCSubType as s
	)


SELECT	*
FROM	Import
GO

