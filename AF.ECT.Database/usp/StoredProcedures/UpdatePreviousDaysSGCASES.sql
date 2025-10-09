create PROCEDURE [usp].[UpdatePreviousDaysSGCASES]
AS
-- ====================================================================================
-- Author:        Eric Kelley
-- Create date: 2/08/2021
-- Description:    Removes data from table [dbo].[PreviousWeeksECTCases] and runs
-- SP [usp].[GetLastWeekECTCases_Weekly] to update table [dbo].[PreviousWeeksECTCases]
-- ====================================================================================
DELETE FROM [dbo].[PreviousDaysECTCases]
INSERT INTO [dbo].[PreviousDaysECTCases]
EXEC [usp].[GetCurrentDay_SGCases]
GO

