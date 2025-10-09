CREATE procedure [dbo].[core_workflow_sp_GetAllLocks]

AS
/* LOD Workflow */
select 
	a.id lockId, a.refId, a.module, a.userId, a.lockTime
	,u.RANK + ' ' + UPPER(u.lastName) + ', ' + UPPER(u.firstName) userName
	,u.LastName
	,m.moduleName, f.case_id
from 
	core_WorkflowLocks a
join 
	vw_Users u on u.userID = a.userId
join 
	core_lkupModule m on m.moduleId = a.module
join 
	Form348 f on f.lodId = a.refId
	
UNION ALL
/* ReInvestigation Workflow */
select 
	a.id lockId, a.refId, a.module, a.userId, a.lockTime
	,u.RANK + ' ' + UPPER(u.lastName) + ', ' + UPPER(u.firstName) userName
	,u.LastName
	,m.moduleName, f.case_id
from 
	core_WorkflowLocks a
join 
	vw_Users u on u.userID = a.userId
join 
	core_lkupModule m on m.moduleId = a.module
join 
	Form348_RR f on f.request_id = a.refId

UNION ALL
/* Special Cases Workflow(s) */
select 
	a.id lockId, a.refId, a.module, a.userId, a.lockTime
	,u.RANK + ' ' + UPPER(u.lastName) + ', ' + UPPER(u.firstName) userName
	,u.LastName
	,m.moduleName, f.case_id
from 
	core_WorkflowLocks a
join 
	vw_Users u on u.userID = a.userId
join 
	core_lkupModule m on m.moduleId = a.module
join 
	Form348_SC f on f.SC_Id = a.refId
GO

