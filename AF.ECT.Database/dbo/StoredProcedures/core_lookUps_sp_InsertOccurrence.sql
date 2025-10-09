
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 9/2/2016
-- Description:	Update a Occurrence in the occurrence table given the id
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookUps_sp_InsertOccurrence]
	@description NVARCHAR(100),
	@type NVARCHAR(100),
	@sort_order INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO core_lkupOccurrenceDescription(occurrence_description, sort_order)
	VALUES (@description, @sort_order)

END
GO

