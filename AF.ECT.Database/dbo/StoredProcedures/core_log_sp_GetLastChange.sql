
CREATE PROCEDURE [dbo].[core_log_sp_GetLastChange]

	@userId INT

AS

SELECT 
	Top 1 a.logId, a.actionDate, u.userId, u.lastname, u.FirstName, u.[rank] 
FROM
	core_logAction a
JOIN 
	vw_users u ON u.userID = a.userId
WHERE 
    (a.referenceId = @userId)
AND
	a.moduleId = 1
AND
	a.actionId = 18 --Modified Form Data 
ORDER BY a.actionDate DESC
GO

