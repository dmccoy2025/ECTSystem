-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 3/3/2016
-- Description:	Gets all records from the PH_Field table with pagination.
-- ============================================================================
CREATE PROCEDURE [dbo].[PH_Field_sp_GetAll_pagination]
	@PageNumber INT = 1,
	@PageSize INT = 10
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT	f.Id, f.Name
	FROM	PH_Field f
	ORDER	BY f.Name ASC
	OFFSET (@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
END
GO