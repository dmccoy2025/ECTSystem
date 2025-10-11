-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 10/27/2016
-- Description:	Returns all of the records from the core_lkupModules table with pagination.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookups_GetAllModules_pagination]
	@PageNumber INT = 1,
	@PageSize INT = 10
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT	m.moduleId, m.moduleName, m.isSpecialCase
	FROM	core_lkupModule m
	ORDER BY m.moduleId
	OFFSET (@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
END
GO