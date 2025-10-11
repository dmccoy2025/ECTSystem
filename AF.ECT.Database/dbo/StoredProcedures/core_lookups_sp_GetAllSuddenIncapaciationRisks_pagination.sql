-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 4/24/2017
-- Description:	Returns all of the records in the 
--				core_lkupSuddenIncapacitationRisk table with pagination.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookUps_sp_GetAllSuddenIncapaciationRisks_pagination]
	@PageNumber INT = 1,
	@PageSize INT = 10
AS
BEGIN
	SELECT	l.Id, l.RiskLevel
	FROM	core_lkupSuddenIncapacitationRisk l
	ORDER BY l.Id
	OFFSET (@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
END
GO