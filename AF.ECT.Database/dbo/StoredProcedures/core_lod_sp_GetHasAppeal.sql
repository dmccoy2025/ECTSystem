
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 7/20/2016
-- Description:	Returns whether or not the LOD specified by the passed in 
--				LOD ID is being or has been appealed. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lod_sp_GetHasAppeal]
	@lodId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @count INT = 0

	SELECT	@count = COUNT(*)
	FROM	Form348_AP a
			JOIN core_WorkStatus ws ON a.status = ws.ws_id
			JOIN core_StatusCodes sc ON ws.statusId = sc.statusId
	WHERE	a.initial_lod_id = @lodId
			AND sc.isCancel = 0
	
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

