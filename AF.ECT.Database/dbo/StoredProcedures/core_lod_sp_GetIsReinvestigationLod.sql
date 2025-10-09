
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 11/30/2015
-- Description:	Returns whether or not the LOD specified by the passed in 
--				LOD ID is a reinvestigation LOD (ie. a LOD case spawened from
--				an approved RR case). It does this by checking if any RR cases
--				exist which have a ReinvestigationLodId value equal to the LOD
--				ID passed into the stored procedure. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lod_sp_GetIsReinvestigationLod]
	@lodId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @count INT = 0

	SELECT	@count = COUNT(*)
	FROM	Form348_RR
	WHERE	ReinvestigationLodId = @lodId
	
	IF (@count > 0)
	BEGIN
		SELECT 1	-- TRUE
	END
	ELSE
	BEGIN
		SELECT 0	-- FALSE
	END
END
GO

