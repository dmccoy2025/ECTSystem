-- ============================================================================
-- Author:		Eric Kelley
-- Create date: Oct 12 2021
-- Description:	Returns all of the records in the core_lkupRMUs table.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_LookUps_sp_RMU]
AS
SELECT  
	  Id AS [Id],
      RMU AS [description]
  FROM [ALOD].[dbo].[core_lkupRMUs]
  Where id < 100

--[dbo].[core_LookUps_sp_RMU]
GO

