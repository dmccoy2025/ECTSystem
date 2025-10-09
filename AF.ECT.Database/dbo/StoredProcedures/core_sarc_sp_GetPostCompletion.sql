
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 11/17/2016
-- Description:	Returns a Form348_SARC_PostProcessing record identified by the 
--				passed in sarcId parameter. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_sarc_sp_GetPostCompletion]
	@sarcId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Validate input...
	IF (ISNULL(@sarcId, 0) = 0)
	BEGIN
		RETURN	
	END

	SELECT	*
	FROM	Form348_SARC_PostProcessing fpp
	WHERE	fpp.sarc_id = @sarcId
END
GO

