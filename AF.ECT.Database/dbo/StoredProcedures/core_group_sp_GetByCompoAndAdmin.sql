
--core_group_sp_GetByCompo '2'

CREATE procedure [dbo].[core_group_sp_GetByCompoAndAdmin]
	@compo char(1),
	 @adminCurrentGroupId tinyInt

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
	(a.compo = @compo OR a.compo = '0')
AND
	a.abbr!='SA' --skip System Admin
AND  
  a.groupId IN (SELECT groupId FROM core_UserGroupsManagedBy WHERE managedBy = @adminCurrentGroupId)
ORDER BY 
	a.sortOrder
GO

