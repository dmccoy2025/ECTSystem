

CREATE PROCEDURE [dbo].[core_log_sp_GetChangeSetByLogId]
	@logId INT
	
AS

SELECT 
	a.logId, a.section, a.field, a.old, a.new, b.actionId, c.actionName
	,b.userId, u.username, b.actionDate
FROM 
	dbo.core_LogChangeSet  a
INNER JOIN
	dbo.core_LogAction AS b ON a.logId = b.logId
INNER JOIN 
	dbo.core_lkupAction c ON c.actionId = b.actionId
JOIN 
	core_users u ON u.userID = b.userId
WHERE 
	a.logId = @logId
ORDER BY 
	section
GO

