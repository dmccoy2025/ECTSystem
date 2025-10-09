
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 1/8/2015
-- Description:	Returns all of the final findings for each reinvestigation
--				case whose initial case is specified by the passed in LOD ID. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lod_sp_GetReinvestigationLodFindings]
	@initialLodId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT	f.FinalFindings
	FROM	Form348_RR r
			JOIN Form348 f ON r.ReinvestigationLodId = f.lodId
	WHERE	r.InitialLodId = @initialLodId
			AND r.ReinvestigationLodId IS NOT NULL
			AND ISNULL(f.FinalFindings, 0) <> 0
	
END
GO

