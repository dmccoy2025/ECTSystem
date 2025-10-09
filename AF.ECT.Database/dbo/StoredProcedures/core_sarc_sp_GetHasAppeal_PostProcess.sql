
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 1/12/2017
-- Description:	Returns 1 if the specified Id has an a post completion entry.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_sarc_sp_GetHasAppeal_PostProcess]
	@appealId INT
AS
BEGIN
	SET NOCOUNT ON;
	
	IF (ISNULL(@appealId, 0) = 0)
	BEGIN
		RETURN 0
	END
	
	DECLARE @count INT = 0

	SELECT	@count = COUNT(*)
	FROM	Form348_PostProcessing_Appeal_SARC ap
	WHERE	ap.appeal_id = @appealId
	
	IF (ISNULL(@count,0) > 0)
	BEGIN
		SELECT 1	-- TRUE
	END
	ELSE
	BEGIN
		SELECT 0	-- FALSE
	END
END
GO

