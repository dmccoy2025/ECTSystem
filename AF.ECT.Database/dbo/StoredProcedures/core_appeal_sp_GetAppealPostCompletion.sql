
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 6/5/2017
-- Description:	Get post processing for an appealId
-- ============================================================================
CREATE PROCEDURE [dbo].[core_appeal_sp_GetAppealPostCompletion]
	@appealId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Validate input...
	IF (ISNULL(@appealId, 0) = 0)
	BEGIN
		RETURN	
	END

	SELECT	*
	FROM	Form348_PostProcessing_appeal 
	WHERE	appeal_id = @appealId
END
GO

