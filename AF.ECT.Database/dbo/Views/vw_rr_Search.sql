
CREATE VIEW [dbo].[vw_rr_Search]
AS
SELECT	a.request_id AS RefId,
		a.InitialLodId AS InitialLODId,
		a.ReinvestigationLodId AS ReinvestigationLODId,
		a.Case_Id AS CaseId,
		a.member_name As MemberFullName,
		m.LAST_NAME AS MemberLastName,
		m.FIRST_NAME AS MemberFirstName,
		m.MIDDLE_NAMES AS MemberMiddleNames,
		a.member_ssn AS MemberSSN,
		a.member_compo AS Compo,
		a.member_unit_id AS MemberUnitId,
		b.member_unit_id AS LODMemberUnitId,
		b.isAttachPAS AS IsAttachPAS,
		b.member_attached_unit_id AS MemberAttachedUnitId,
		rrCS.PAS_CODE as Pascode,
		lodCS.PAS_CODE as LODPascode,
		a.status as WorkStatusId,
		s.description as WorkStatus,
		s.isFinal as IsFinal,
		a.workflow  as WorkflowId,
		w.title as Workflow,
		w.moduleId as ModuleId,
		b.formal_inv  as IsLODFormal,
		rrCS.long_name as UnitName,
		lodCS.long_name as LODUnitName,
		a.CreatedDate AS CreatedDate,
		t.ReceiveDate AS ReceiveDate,
		b.sarc AS IsLODSARC,
		b.restricted AS IsLODRestricted,
		ISNULL(l.id,0) lockId,
		b.deleted AS IsLODDeleted,
		s.filter AS Filter
FROM	Form348_RR a
		JOIN Form348 b ON b.lodid = a.InitialLodId
		JOIN core_WorkStatus ws ON ws.ws_id = a.status
		JOIN core_StatusCodes s ON s.statusId = ws.statusId
		JOIN core_workflow w ON w.workflowId = a.workflow
		JOIN Command_Struct rrCS ON a.member_unit_id = rrCS.CS_ID
		JOIN Command_Struct lodCS ON lodCS.cs_id = b.member_unit_id 
		LEFT JOIN core_workflowLocks l on l.refId = a.request_id AND l.module = (SELECT moduleId FROM core_lkupModule WHERE moduleName = 'Reinvestigation Request')
		LEFT JOIN (SELECT max(startDate) ReceiveDate, ws_id, refId FROM core_WorkStatus_Tracking GROUP BY ws_id, refId) t ON t.refId = a.request_id AND t.ws_id = a.status
		LEFT JOIN MemberData m ON m.SSAN = a.member_ssn
WHERE	a.status NOT IN (
							SELECT	ws_id
							FROM	core_StatusCodes sc
									JOIN core_WorkStatus ws ON sc.statusId = ws.statusId
							WHERE	sc.description = 'Reinvestigation Request Cancelled'
						)
GO

