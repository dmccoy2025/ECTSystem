 --exec core_tracking_sp_GetByReferenceId 4,2,0
 --action
CREATE PROCEDURE [dbo].[core_tracking_sp_GetByUserId]
	@userId int,
	@showAll bit = 0
AS
BEGIN

	SET NOCOUNT ON;

SELECT     actionDate, description, actionName, notes, lastname, actionId, logId, userId, logChanges, moduleId,moduleName, referenceId ,caseId
FROM         dbo.vw_UserTracking
WHERE     
		(userId = @userId) 	
    AND
		(
			@showAll = 1
		OR
			hide = 0
		)
ORDER BY  actionDate DESC, moduleId, caseId


END
GO

