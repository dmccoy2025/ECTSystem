--execute form348_GetNextStatus 415,5
-- ============================================================================
-- Created By:		?
-- Created Date:	?
-- Description:		
-- ============================================================================
CREATE PROCEDURE [dbo].[form348_GetInitialNextStep]
	@module AS TINYINT,
	@refId AS INT,
	@inStatus AS INT
AS

DECLARE @wst_id1 INT, 
		@wst_id2 INT, 
		@outStatus INT

SELECT	TOP (1) @wst_id1 = wst_id
FROM	core_WorkStatus_Tracking
WHERE	ws_id = @inStatus
		AND refid = @refId
		AND module = @module
ORDER	BY startDate ASC

SELECT	TOP (1) @outStatus = ws_id
FROM	core_WorkStatus_Tracking
WHERE	wst_id > @wst_id1
		AND refid = @refId
		AND module = @module
ORDER	BY startDate ASC

SELECT @outStatus
GO

