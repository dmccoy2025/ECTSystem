
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 2/2/2016
-- Description:	Returns all of the records in the core_CompletedByGroup table.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_CompletedByGroup_sp_GetAll]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT	cbg.Id, cbg.Name
	FROM	core_CompletedByGroup cbg
END
GO

