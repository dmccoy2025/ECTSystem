
CREATE VIEW [dbo].[vw_lod_Search]
AS
SELECT	 a.lodId AS RefId
		,a.case_id AS CaseId
		,a.member_name As MemberFullName
		,m.LAST_NAME AS MemberLastName
		,m.FIRST_NAME AS MemberFirstName
		,m.MIDDLE_NAMES AS MemberMiddleNames
		,a.member_ssn AS MemberSSN
		,a.member_compo AS Compo
		,a.member_unit_id AS MemberUnitId
		,a.isAttachPAS AS IsAttachPAS
		,a.member_attached_unit_id AS MemberAttachedUnitId
		,cs.PAS_CODE as Pascode
		,a.status as WorkStatusId
		,s.description as WorkStatus
		,s.isFinal as IsFinal
		,a.workflow  as WorkflowId
		,w.title as Workflow
		,w.moduleId as ModuleId
		,a.formal_inv  as IsFormal
		,cs.long_name as UnitName
		,a.created_date AS CreatedDate
		,t.ReceiveDate AS ReceiveDate
		,a.sarc AS IsSARC
		,a.restricted AS IsRestricted
		,ISNULL(l.id,0) lockId
		,a.deleted AS IsDeleted
		,s.filter AS Filter
		,b.IoUserId AS IOUserId
FROM	Form348 a
		LEFT JOIN FORM261 b ON b.lodid = a.lodid
		JOIN core_WorkStatus ws ON ws.ws_id = a.status
		JOIN core_StatusCodes s ON s.statusId = ws.statusId
		JOIN core_workflow w ON w.workflowId = a.workflow
		JOIN COMMAND_STRUCT CS ON cs.cs_id = a.member_unit_id 
		JOIN Form348_Medical fm ON fm.lodid = a.lodid
		LEFT JOIN core_workflowLocks l on l.refId = a.lodId AND l.module = (SELECT moduleId FROM core_lkupModule WHERE moduleName = 'LOD')
		LEFT JOIN (SELECT max(startDate) ReceiveDate, ws_id, refId FROM core_WorkStatus_Tracking GROUP BY ws_id, refId) t ON t.refId = a.lodId AND t.ws_id = a.status
		LEFT JOIN MemberData m ON m.SSAN = a.member_ssn
WHERE	(s.isCancel = 0)
GO

