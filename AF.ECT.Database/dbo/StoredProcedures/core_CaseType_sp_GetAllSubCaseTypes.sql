
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 1/25/2016
-- Description:	Returns all of the records in the core_SubCaseType table.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_CaseType_sp_GetAllSubCaseTypes]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT	sct.Id, sct.Name
	FROM	core_SubCaseType sct
END
GO

