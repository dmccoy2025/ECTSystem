
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 9/2/2016
-- Description:	Update a Info Source in the info source table given the id
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookUps_sp_UpdateInfoSource]
	@id INT,
	@description NVARCHAR(100),
	@type NVARCHAR(100),
	@sort_order INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE core_lkupInfoSource
	SET source_description = @description, sort_order = @sort_order
	WHERE source_id = @id

END
GO

