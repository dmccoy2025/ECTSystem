
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 10/27/2016
-- Description:	Returns all of the records from the core_lkupModules table.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookups_GetAllModules]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT	m.moduleId, m.moduleName, m.isSpecialCase
	FROM	core_lkupModule m
END
GO

