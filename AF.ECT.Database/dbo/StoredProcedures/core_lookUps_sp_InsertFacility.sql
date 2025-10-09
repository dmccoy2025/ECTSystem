
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 9/2/2016
-- Description:	Update a Facility in the facility table given the id
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookUps_sp_InsertFacility]
	@description NVARCHAR(100),
	@type NVARCHAR(100),
	@sort_order INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO core_lkupMedicalFacility(facility_description,facility_type, sort_order)
	VALUES (@description, @type, @sort_order)

END
GO

