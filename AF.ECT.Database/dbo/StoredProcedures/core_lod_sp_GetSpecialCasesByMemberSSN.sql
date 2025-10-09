
-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	Executes the Special Case portion of the Case History Canned 
--				Report. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	3/2/2017
-- Description:		Cleaned up the procedure. 
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	4/12/2017
-- Description:		Returns the workflow title 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lod_sp_GetSpecialCasesByMemberSSN]
	@member_ssn VARCHAR(12),
	@userId INT
AS
BEGIN
	SELECT	DISTINCT sc_id AS RefId,
			SUBSTRING(fsc.member_ssn, 6, 4) AS Protected_SSN,
			fsc.member_name AS Member_Name,
			fsc.member_unit AS Unit_Name,
			fsc.Case_Id,
			vws.description AS Status,
			fsc.Module_Id,
			mods.moduleName AS Module,
			CONVERT(char(11), ISNULL(t.ReceiveDate, fsc.created_date), 101) AS ReceiveDate,
			DATEDIFF(d, ISNULL(t.ReceiveDate, fsc.created_date), GETDATE()) AS Days,
			sub_workflow_type AS Sub_Type,
			(CASE fsc.sub_workflow_type WHEN 0 Then w.title ELSE sub.subTypeTitle END) AS workflow_title
	FROM	Form348_SC fsc
			INNER JOIN vw_WorkStatus vws ON fsc.status = vws.ws_id
			INNER JOIN core_lkupModule mods ON mods.moduleId = fsc.Module_Id
			LEFT JOIN (
				SELECT	MAX(startDate) ReceiveDate, ws_id, refId 
				FROM	core_WorkStatus_Tracking 
				GROUP	BY ws_id, refId
			) t ON t.refId = fsc.sc_id AND t.ws_id = fsc.status
			LEFT JOIN core_lkupSCSubType sub ON sub.subTypeId = fsc.sub_workflow_type
			JOIN core_Workflow w ON W.workflowId = fsc.workflow
	WHERE	fsc.Member_ssn = @member_ssn
	ORDER	BY mods.moduleName, Status, ReceiveDate DESC
END
GO

