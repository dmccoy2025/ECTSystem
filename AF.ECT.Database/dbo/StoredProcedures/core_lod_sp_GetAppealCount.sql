
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 7/25/2016
-- Description:	Returns the number of AP cases associated with the specified
--				initial LOD ID.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lod_sp_GetAppealCount]
	@initialLODId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- Validate input...
	IF (ISNULL(@initialLODId, 0) = 0)
	BEGIN
		SELECT 0
	END
	
	SELECT	COUNT(a.appeal_id)
	FROM	Form348_AP a
	WHERE	a.initial_lod_id = @initialLODId

END
GO

