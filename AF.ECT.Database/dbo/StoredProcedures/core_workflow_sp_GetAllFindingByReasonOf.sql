
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 2/17/2017
-- Description:	Gets all of the records in the core_lkUpFindingByReasonOf
--				table.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_workflow_sp_GetAllFindingByReasonOf]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT	r.Id, r.Description
	FROM	core_lkUpFindingByReasonOf r
	ORDER	BY r.Id
END
GO

