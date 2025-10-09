
CREATE VIEW [dbo].[vw_Tracking]
AS
SELECT     
	a.actionDate, s.description, a.actionid, la.actionName, la.hide, a.notes, vu.lastname, a.moduleId, a.referenceId, a.logId,a.userId, la.logChanges
FROM         
	dbo.core_LogAction AS a 
LEFT JOIN
	dbo.core_StatusCodes AS s ON s.statusId = a.status 
INNER JOIN
	dbo.core_lkupAction AS la ON la.actionId = a.actionId 
INNER JOIN
	dbo.core_users AS vu ON vu.userID = a.userId
GO

