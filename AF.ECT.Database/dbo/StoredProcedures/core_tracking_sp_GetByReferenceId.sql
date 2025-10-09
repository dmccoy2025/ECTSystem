-- =============================================
-- Author:		Nick McQuillen
-- Create date: 2008-5-30
-- Description:	Retrieves Tracking information 
-- =============================================
CREATE PROCEDURE [dbo].[core_tracking_sp_GetByReferenceId]
	@moduleId tinyint,
	@referenceId int,
	@showAll bit = 0
AS
BEGIN

	SET NOCOUNT ON;

	SELECT 
		actionDate, description, actionName, notes, lastname, actionid, logId, userId, logChanges,moduleId, referenceId 
	FROM vw_Tracking
	WHERE 
		moduleId = @moduleId
	AND
		referenceId = @referenceId
	AND
		(
			@showAll = 1
		OR
			hide = 0
		)
	ORDER BY
		actionDate desc

END
GO

