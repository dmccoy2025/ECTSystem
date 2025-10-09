
--core_group_sp_GetByCompo '6'

CREATE PROCEDURE [dbo].[core_group_sp_GetByCompoOnly]
	@compo char(1)

AS

SELECT	a.groupId, a.name, a.abbr, a.active
		,a.compo,b.compo_descr,a.accessscope, d.description as 'accessscopedescr'
		,a.partialMatch,a.showInfo,a.hipaaRequired,a.canRegister,a.sortOrder
FROM 
	core_UserGroups a 
join 
	core_lkupCompo b on a.compo = b.compo 
join
	core_lkupAccessScope d on d.scopeId = a.accessscope
WHERE 
	(a.compo = @compo)
AND
	a.abbr !='SA'  --skip  System Admin for all he compos currentl
ORDER BY 
	a.sortOrder
GO

