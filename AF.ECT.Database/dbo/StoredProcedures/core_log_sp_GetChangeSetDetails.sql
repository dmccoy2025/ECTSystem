

CREATE PROCEDURE [dbo].[core_log_sp_GetChangeSetDetails]
	@logIds VARCHAR(100)
	
AS

SELECT 
	a.logId, a.actionDate, u.userId, u.lastname, u.FirstName, u.[rank] 
FROM
	core_logAction a
JOIN 
	vw_users u ON u.userID = a.userId
WHERE 
	logId IN (SELECT value FROM dbo.Split(@logIds,','))
GO

