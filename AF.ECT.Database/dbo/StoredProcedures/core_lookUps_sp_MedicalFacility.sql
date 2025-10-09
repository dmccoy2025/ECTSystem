
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 9/7/2016
-- Description:	Return all of the Medical Facility
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookUps_sp_MedicalFacility]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT	mf.facility_description AS [description] , mf.facility_id As [id], mf.sort_order As [sort_order], mf.facility_type AS [type]
	FROM	dbo.core_lkupMedicalFacility mf
	ORDER	BY mf.sort_order ASC
END
GO

