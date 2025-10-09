
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 6/8/2016
-- Description:	Returns the smallest date year of the PH case reporting 
--				periods.
-- ============================================================================
CREATE PROCEDURE [dbo].[PH_Report_sp_GetSmallestReportingPeriodYear]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT	TOP 1 DATEPART(YEAR, s.ph_reporting_period)
	FROM	Form348_SC s
	WHERE	s.ph_reporting_period IS NOT NULL
	ORDER	BY s.ph_reporting_period ASC
END
GO

