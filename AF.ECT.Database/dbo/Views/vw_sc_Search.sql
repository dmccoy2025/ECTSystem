
CREATE VIEW [dbo].[vw_sc_Search]
AS
SELECT	 a.SC_Id AS RefId
		,a.case_id  AS CaseId
		,a.member_name As MemberFullName
		,m.LAST_NAME AS MemberLastName
		,m.FIRST_NAME AS MemberFirstName
		,m.MIDDLE_NAMES AS MemberMiddleNames
		,a.member_ssn AS MemberSSN
		,a.member_compo AS Compo
		,cs.PAS_CODE AS Pascode
		,a.Member_Unit_Id AS MemberUnitId
		,a.status AS WorkStatusId
		,s.description AS WorkStatus
		,s.isFinal AS IsFinal
		,a.workflow AS WorkflowId
		,w.title AS Workflow
		,a.Module_Id AS ModuleId
		,a.created_date AS DateCreated
		,cs.long_name as UnitName
		,a.created_date AS CreatedDate
		,t.ReceiveDate AS ReceiveDate
		,ISNULL(l.id,0) lockId
		,sub_workflow_type as Sub_Type
FROM	Form348_SC a
		JOIN core_WorkStatus ws ON ws.ws_id = a.status
		JOIN core_StatusCodes s ON s.statusId = ws.statusId
		JOIN core_workflow w ON w.workflowId = a.workflow
		JOIN COMMAND_STRUCT CS ON cs.cs_id = a.member_unit_id 
		LEFT JOIN core_workflowLocks l ON l.refId = a.sc_id AND (l.module = IsNull(w.moduleId,l.module))
		LEFT JOIN (SELECT max(startDate) ReceiveDate, ws_id, refId FROM core_WorkStatus_Tracking GROUP BY ws_id, refId) t ON t.refId = a.SC_Id AND t.ws_id = a.status
		LEFT JOIN MemberData m ON m.SSAN = a.member_ssn
GO

