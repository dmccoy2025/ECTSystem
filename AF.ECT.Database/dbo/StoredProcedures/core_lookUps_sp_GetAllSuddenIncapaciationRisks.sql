
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 4/24/2017
-- Description:	Returns all of the records in the 
--				core_lkupSuddenIncapacitationRisk table.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookUps_sp_GetAllSuddenIncapaciationRisks]
AS
BEGIN
	SELECT	l.Id, l.RiskLevel
	FROM	core_lkupSuddenIncapacitationRisk l
END
GO

