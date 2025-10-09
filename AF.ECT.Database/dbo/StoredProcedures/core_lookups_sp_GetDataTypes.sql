
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 3/9/2016
-- Description:	Gets all records from the core_lkupDataType table. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookups_sp_GetDataTypes]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT	dt.Id, dt.Name
	FROM	core_lkupDataType dt
END
GO

