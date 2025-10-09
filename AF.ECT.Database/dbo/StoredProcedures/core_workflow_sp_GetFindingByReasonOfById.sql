
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 2/17/2017
-- Description:	Gets a record from the core_lkUpFindingByReasonOf table by its
--				Id.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_workflow_sp_GetFindingByReasonOfById]
	@id INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT	r.Id, r.Description
	FROM	core_lkUpFindingByReasonOf r
	WHERE	r.Id = @id
END
GO

