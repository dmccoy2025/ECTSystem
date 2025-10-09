
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 4/24/2017
-- Description:	Returns all of the records in the
--				core_lkupSpecialistsRequiredForManagement table.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookUps_sp_GetAllSpecialistsRequiredForManagement]
AS
BEGIN
	SELECT	l.Id, l.AmountPerYear
	FROM	core_lkupSpecialistsRequiredForManagement l
END
GO

