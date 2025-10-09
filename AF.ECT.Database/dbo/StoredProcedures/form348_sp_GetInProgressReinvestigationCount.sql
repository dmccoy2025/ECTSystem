
-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	Returns the number of active RR cases.
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	11/17/2016
-- Description:		Fixed a bug
-- ============================================================================
CREATE PROCEDURE [dbo].[form348_sp_GetInProgressReinvestigationCount]
		 @LodId int 
AS
BEGIN
	SELECT	COUNT(*)
	FROM	Form348_RR rr
			RIGHT JOIN Form348 a ON a.lodId = rr.ReinvestigationLodId
			RIGHT JOIN core_WorkStatus WS ON WS.ws_id = a.status
			RIGHT JOIN core_StatusCodes SC ON SC.statusId = WS.statusId
	WHERE	rr.InitialLodId = @LodId
			AND SC.isFinal = 0
			AND SC.isCancel = 0
END
GO

