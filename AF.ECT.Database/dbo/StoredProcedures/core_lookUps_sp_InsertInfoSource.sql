
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 9/2/2016
-- Description:	Update a Info Source in the info source table given the id
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookUps_sp_InsertInfoSource]
	@description NVARCHAR(100),
	@type NVARCHAR(100),
	@sort_order INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO core_lkupInfoSource(source_description, sort_order)
	VALUES (@description, @sort_order)

END
GO

