
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 2/23/2017
-- Description:	Returns the results of the GetFromAndDirection scalar function.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lod_sp_GetFromAndDirection]
	@refId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT	dbo.GetFromAndDirection(@refId)
END
GO

