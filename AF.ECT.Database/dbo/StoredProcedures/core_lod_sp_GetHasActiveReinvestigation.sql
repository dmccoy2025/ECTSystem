
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 1/24/2017
-- Description:	Returns whether or not the LOD specified by the passed in 
--				LOD ID has an active RR case
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lod_sp_GetHasActiveReinvestigation]
	@lodId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @count INT = 0

	SELECT	@count = COUNT(*)
	FROM	Form348_RR r
			JOIN core_WorkStatus ws ON r.status = ws.ws_id
			JOIN core_StatusCodes sc ON ws.statusId = sc.statusId
	WHERE	r.InitialLodId = @lodId
			AND sc.isCancel = 0
			AND sc.isFinal = 0
	
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

