
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 11/27/2015
-- Description:	Returns the number of RR cases associated with the specified
--				initial LOD ID.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lod_sp_GetReinvestigationRequestsCount]
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
	
	SELECT	COUNT(r.request_id)
	FROM	Form348_RR r
	WHERE	r.InitialLodId = @initialLODId

END
GO

