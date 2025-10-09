
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 9/2/2016
-- Description:	Return all of the Occurrence Descriptions
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookUps_sp_Occurrences]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT	os.occurrence_description As [description], os.occurrence_id As [id], os.sort_order As [sort_order]
	FROM	dbo.core_lkupOccurrenceDescription os
	ORDER	BY os.sort_order ASC
END
GO

