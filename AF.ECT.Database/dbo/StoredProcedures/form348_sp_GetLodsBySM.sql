
-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	Executes the LOD portion of the Case History Canned Report. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	3/2/2017
-- Description:		Cleaned up the procedure. 
-- ============================================================================
CREATE PROCEDURE [dbo].[form348_sp_GetLodsBySM]
	@ssn CHAR(9) = NULL,
	@sarcpermission BIT = NULL,
	@includeDeleted BIT = NULL
AS
BEGIN
	SET @includeDeleted = COALESCE(@includeDeleted, 0);

	SELECT	a.lodId as RefId,a.case_id  as CaseId,
			a.member_name  as Name,
			RIGHT(a.member_ssn, 4) SSN,
			a.member_compo as Compo,
			cs.PAS_CODE as Pascode,
			a.status as WorkStatusId,
			s.description as WorkStatus,
			s.isFinal as IsFinal,
			a.workflow  as WorkflowId,
			w.title as Workflow,
			w.moduleId as ModuleId,
			a.formal_inv  as IsFormal,
			convert(char(11), a.created_date, 101) DateCreated,
			cs.long_name as UnitName,
			convert(char(11), ISNULL(t.ReceiveDate, a.created_date), 101) ReceiveDate,
			datediff(d, ISNULL(t.ReceiveDate, a.created_date), getdate()) Days,
			a.sarc
	FROM	Form348  a
			JOIN core_WorkStatus ws ON ws.ws_id = a.status
			JOIN core_StatusCodes s ON s.statusId = ws.statusId
			JOIN core_workflow w ON w.workflowId = a.workflow
			JOIN COMMAND_STRUCT CS ON cs.cs_id = a.member_unit_id 
			JOIN Form348_Medical fm ON fm.lodid = a.lodid
			LEFT JOIN (
				SELECT	max(startDate) ReceiveDate, 
						refId 
				FROM	core_WorkStatus_Tracking 
				GROUP	BY refId
			) t ON t.refId = a.lodId
	WHERE	a.member_ssn = @ssn 
			AND (
				CASE 
					WHEN (a.sarc = 1 AND a.restricted=1) Then 1  
					ELSE @sarcpermission	
				END
				= @sarcpermission
			)	
			AND (
				CASE 
					WHEN @includeDeleted is null THEN a.deleted 
					ELSE @includeDeleted 
				END
				= a.deleted   
			)
	ORDER	BY a.case_id
END
GO

