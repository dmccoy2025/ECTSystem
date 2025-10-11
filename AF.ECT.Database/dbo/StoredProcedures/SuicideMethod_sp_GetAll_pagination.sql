-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 4/5/2016
-- Description:	Gets all record from the SuicideMethod table with pagination.
-- ============================================================================
CREATE PROCEDURE [dbo].[SuicideMethod_sp_GetAll_pagination]
	@PageNumber INT = 1,
	@PageSize INT = 10
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT	s.Id, s.Name, s.Active
	FROM	SuicideMethod s
	ORDER	BY s.Name ASC
	OFFSET (@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
END
GO