
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 1/18/2016
-- Description:	Returns all of the records in the core_lkupDisposition table.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookups_sp_GetDispositions]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT	d.Id, d.Name
	FROM	core_lkupDisposition d
END
GO

