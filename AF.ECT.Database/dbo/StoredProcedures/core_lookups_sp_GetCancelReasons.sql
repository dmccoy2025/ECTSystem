
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 7/8/2016
-- Description:	Returns all of the records in the core_lkupCancelReason table.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookups_sp_GetCancelReasons]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT	d.Id, d.Description, d.DisplayOrder
	FROM	core_lkupCancelReason d
END
GO

