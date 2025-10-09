
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 12/23/2015
-- Description:	Returns a Form348_PostCompletion record identified by the 
--				passed in lodId parameter. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lod_sp_GetLODPostCompletion]
	@lodId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Validate input...
	IF (ISNULL(@lodId, 0) = 0)
	BEGIN
		RETURN	
	END

	SELECT	*
	FROM	Form348_PostProcessing fpp
	WHERE	fpp.lodId = @lodId
END
GO

