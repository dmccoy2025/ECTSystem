-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 2/29/2016
-- Description:	Gets all records from the PH_Section table with pagination.
-- ============================================================================
CREATE PROCEDURE [dbo].[PH_Section_sp_GetAll_pagination]
	@PageNumber INT = 1,
	@PageSize INT = 10
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT	s.Id, s.Name, s.ParentId, s.FieldColumns, s.IsTopLevel, s.DisplayOrder, s.PageBreak
	FROM	PH_Section s
	ORDER BY s.Id
	OFFSET (@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
END
GO