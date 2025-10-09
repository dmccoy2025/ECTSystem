
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 4/24/2017
-- Description:	Returns all of the records in the core_lkupDAWGRecommendation
--				table.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookUps_sp_GetAllDAWGRecommendations]
AS
BEGIN
	SELECT	l.Id, l.Recommendation
	FROM	core_lkupDAWGRecommendation l
END
GO

