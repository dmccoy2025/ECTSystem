
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 9/7/2016
-- Description:	Return all of the From Location
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookUps_sp_FromLocation]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT	fl.location_description As [Description], fl.location_id AS [Id], fl.sort_order AS [sort_order]
	FROM	dbo.core_lkupFromLocation fl
	ORDER	BY fl.sort_order ASC
END
GO

