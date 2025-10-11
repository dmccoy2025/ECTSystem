-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 2/2/2016
-- Description:	Returns all of the records in the core_CompletedByGroup table with pagination.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_CompletedByGroup_sp_GetAll_pagination]
	@PageNumber INT = 1,
	@PageSize INT = 10
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT	cbg.Id, cbg.Name
	FROM	core_CompletedByGroup cbg
	ORDER BY cbg.Id
	OFFSET (@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
END
GO