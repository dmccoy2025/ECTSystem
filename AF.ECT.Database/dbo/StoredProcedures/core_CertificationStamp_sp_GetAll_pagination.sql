-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 1/20/2016
-- Description:	Returns all of the records in the core_CertificationStamp table with pagination.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_CertificationStamp_sp_GetAll_pagination]
	@PageNumber INT = 1,
	@PageSize INT = 10
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT	cs.Id, cs.Name, cs.Body, cs.IsQualified
	FROM	core_CertificationStamp cs
	ORDER BY cs.Id
	OFFSET (@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
END
GO