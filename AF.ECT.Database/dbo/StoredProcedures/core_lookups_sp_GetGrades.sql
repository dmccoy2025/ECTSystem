
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 12/9/2016
-- Description:	Gets all records from the core_lkupGrade table. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookups_sp_GetGrades]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT	lg.Title, lg.Code
	FROM	core_lkupGrade lg
	Order By lg.DISPLAYORDER
END
GO

