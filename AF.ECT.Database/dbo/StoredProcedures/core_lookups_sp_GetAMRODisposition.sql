-- ============================================================================
-- Author:		Eric Kelley
-- Create date: 12/28/2021
-- Description:	Returns all AMRO Disposition
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookups_sp_GetAMRODisposition]
AS
SELECT disposition_Id
      ,[description]
  FROM [ALOD].[dbo].[core_lkupAMRODisposition]
GO

