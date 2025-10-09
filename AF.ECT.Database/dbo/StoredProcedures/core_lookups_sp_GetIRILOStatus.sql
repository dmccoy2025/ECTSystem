-- ============================================================================
-- Author:		Eric Kelley
-- Create date: 12/28/2021
-- Description:	Returns all IRILO Status
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookups_sp_GetIRILOStatus]
AS
SELECT id
      ,[description]
  FROM [ALOD].[dbo].[core_lkupIRILOStatus]
GO

