-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 3/17/2016
-- Description:	Gets all of the records in the core_lkupsRMUs table with pagination.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookups_sp_GetAllRMUs_pagination]
	@PageNumber INT = 1,
	@PageSize INT = 10
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT	r.Id, r.RMU, r.cs_id
	FROM	core_lkupRMUs r
	ORDER	BY r.RMU
	OFFSET (@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
END
GO