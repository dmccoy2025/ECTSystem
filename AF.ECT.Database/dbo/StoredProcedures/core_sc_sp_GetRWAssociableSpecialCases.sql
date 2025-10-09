
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 6/7/2017
-- Description:	Returns a members special cases that are capable of being
--				associated with a RW case.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_sc_sp_GetRWAssociableSpecialCases]
	@member_ssn VARCHAR(12)
AS
BEGIN
	SELECT	DISTINCT sc_id AS RefId,
			fsc.Case_Id AS CaseId,
			m.moduleName
	FROM	Form348_SC fsc
			JOIN vw_WorkStatus vws ON fsc.status = vws.ws_id
			JOIN core_lkupModule m ON m.moduleId = fsc.Module_Id
	WHERE	fsc.Member_ssn = @member_ssn 
			AND vws.isFinal = 1
			AND vws.isCancel = 0
			AND m.moduleName IN (
				'IRILO WD',
				'Medical Evaluation Board',
				'Recruiting Services',
				'Worldwide Duty'
			)
	ORDER	BY m.moduleName, fsc.case_id
END
GO

