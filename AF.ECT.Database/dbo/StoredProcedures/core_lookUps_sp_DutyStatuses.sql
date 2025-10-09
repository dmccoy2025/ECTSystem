
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 9/2/2016
-- Description:	Return all of the duty statuses
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookUps_sp_DutyStatuses]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT	ds.dutyDescription As [Description] , ds.dutyType As [Type], ds.sortOrder As [sort_order], ds.dutyId As [id]
	FROM	dbo.core_lkupDutyStatus ds
	ORDER	BY ds.sortOrder ASC
END
GO

