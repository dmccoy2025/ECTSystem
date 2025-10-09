
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 8/2/2016
-- Description:	Returns 1 if the specified Id has an a post completion entry.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lod_sp_GetHasAppeal_PostProcess]
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
	FROM	Form348_PostProcessing_Appeal ap
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

