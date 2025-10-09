-- Exec core_lod_sp_GetSpecialCasesByMemberSSN '020000002'

-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	8/1/2016
-- Description:		Updated to no longer use the @statusCodes parameter.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	6/7/2017
-- Description:		- Removed the comment out code related to the old 
--					@statusCodes parameter.
--					- Cleaned up the procedure.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lod_sp_GetCompletedSpecialCasesByMemberSSN]
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
			DATEDIFF(d, ISNULL(t.ReceiveDate, fsc.created_date), GETDATE()) AS Days
	FROM	Form348_SC fsc
			INNER JOIN vw_WorkStatus vws ON fsc.status = vws.ws_id
			INNER JOIN core_lkupModule mods ON mods.moduleId = fsc.Module_Id
			LEFT JOIN 
			(
				SELECT Max(startDate) ReceiveDate, ws_id, refId 
				FROM core_WorkStatus_Tracking 
				GROUP BY ws_id, refId
			) t ON t.refId = fsc.sc_id AND t.ws_id = fsc.status
	WHERE	fsc.Member_ssn = @member_ssn 
			AND vws.isFinal = 1
			AND vws.isCancel = 0
	ORDER	BY mods.moduleName, Status, ReceiveDate DESC
END
GO

