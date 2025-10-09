-- =============================================
-- Author:		<Kamal Singh>
-- Create date: <7/1/2015>
-- Description:	<Returns UserIds of Users whos accounts that should be disabled.>
-- =============================================
CREATE PROCEDURE [dbo].[ReminderGetUsersToDisable]

AS
BEGIN
	
	SET NOCOUNT ON;
	
	CREATE TABLE #temp1(actionId int, userId int, actionDate Date)
	
	INSERT INTO #temp1
	SELECT actionId, logs.userId,MAX(actionDate)
	FROM alod.dbo.core_LogAction AS logs INNER JOIN alod.dbo.core_Users AS users ON logs.userId = users.userID
	where  actionId = 20 AND users.accessStatus = 3
	GROUP BY actionId, logs.userId, logs.referenceId
	
	--Gathers info when the last the users status had been changed and is approved
	CREATE TABLE #temp2(actionId int, userId int, actionDate Date)
	
	INSERT INTO #temp2
	SELECT actionId, logs.referenceId,MAX(actionDate)
	FROM alod.dbo.core_LogAction AS logs INNER JOIN alod.dbo.core_Users AS users ON logs.referenceId = users.userID 
	where logs.actionId = 2 AND users.accessStatus = 3
	GROUP BY actionid, logs.userId, logs.referenceId
	
	--Joins prev tables and gathers the largest date
	CREATE TABLE #temp3(userId int, actionDate Date)
	
	INSERT INTO #temp3
	SELECT users.userid, (SELECT CASE WHEN temp2.actionDate >= temp1.actionDate THEN temp2.actionDate ELSE temp1.actionDate END)
	FROM alod.dbo.core_Users As users LEFT JOIN  #temp1 AS temp1 ON users.userID = temp1.userId LEFT JOIN 
			#temp2 as temp2 ON users.userId = temp2.userId 
	WHERE users.accessStatus = 3
	
	DECLARE @templateId int
	DECLARE @inactiveInterv int
	DECLARE @active bit
	
	SELECT @inactiveInterv = interval, @templateId = templateId, @active = active
	FROM alod.dbo.ReminderInactiveSettings
	WHERE i_id = 1
	
	SELECT temp.userId
	FROM #temp3 as temp INNER JOIN 
	alod.dbo.core_Users AS users ON temp.userId = users.userID
	WHERE DATEDIFF(DAY, actionDate, GETDATE() ) >= @inactiveInterv AND @active = 1 AND users.accessStatus = 3
	
	
END
GO

