-- ============================================================================
-- Author: Ken Barnett
-- Create date: 1/25/2016
-- Description: Returns all of the records in the core_CaseType table with pagination.
-- ============================================================================
-- Modified By: Eric Kelley
-- Modified Date: 2/5/2021
-- Description: Hides ID 15 (PC/PF) so it can no longer be selected.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_CaseType_sp_GetAll_pagination]
	@PageNumber INT = 1,
	@PageSize INT = 10
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON;
SELECT Id, Name
FROM (
SELECT ct.Id, ct.Name
FROM core_CaseType ct
Except
SELECT ct.Id, ct.Name
FROM core_CaseType ct
WHERE ct.Id = 15
) AS sub
ORDER BY Id
OFFSET (@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
END

--core_CaseType_sp_GetAll_pagination
GO