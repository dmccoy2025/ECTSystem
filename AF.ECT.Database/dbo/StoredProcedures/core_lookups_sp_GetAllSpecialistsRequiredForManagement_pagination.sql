-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 4/24/2017
-- Description:	Returns all of the records in the
--				core_lkupSpecialistsRequiredForManagement table with pagination.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookUps_sp_GetAllSpecialistsRequiredForManagement_pagination]
	@PageNumber INT = 1,
	@PageSize INT = 10
AS
BEGIN
	SELECT	l.Id, l.AmountPerYear
	FROM	core_lkupSpecialistsRequiredForManagement l
	ORDER BY l.Id
	OFFSET (@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
END
GO