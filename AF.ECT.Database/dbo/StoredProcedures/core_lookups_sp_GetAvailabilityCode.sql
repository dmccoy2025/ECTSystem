-- ============================================================================
-- Author:		Eric Kelley
-- Create date: 12/28/2021
-- Description:	Returns all Availability Codes
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookups_sp_GetAvailabilityCode]
AS
SELECT [availabilityCode]
      ,[description]
  FROM [ALOD].[dbo].[core_lkupAvailabilityCode]
GO

