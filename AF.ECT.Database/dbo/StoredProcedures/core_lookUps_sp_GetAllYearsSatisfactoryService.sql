
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 4/24/2017
-- Description:	Returns all of the records in the 
--				core_lkupYearsSatisfactoryService table.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookUps_sp_GetAllYearsSatisfactoryService]
AS
BEGIN
	SELECT	l.Id, l.RangeCategory
	FROM	core_lkupYearsSatisfactoryService l
END
GO

