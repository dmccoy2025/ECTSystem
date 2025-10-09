-- =============================================
-- Author:		Kamal Singh
-- Create date: 06/26/2015
-- Description:	Gathers emails for inactive users to be sent
-- =============================================

-- =============================================
-- Modified By:		Kamal Singh
-- Modified date: 09/2/2015s.
-- Description:	added "AND users.accessstatus = 3" to the where clause of the last
--						select statements to fix the issue where emails were be sent to already disabled accounts.
-- =============================================

CREATE PROCEDURE [dbo].[ReminderGetInactiveEmails] 

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	--Gathers info from the last time the user had logged in
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
			

	
	Declare @templateId int
	DECLARE @inactiveInterv int
	DECLARE @notifInterv int
	DECLARE @active bit

	
	SELECT @inactiveInterv = interval, @notifInterv = notification_interval, @templateId = templateId, @active = active
	FROM alod.dbo.ReminderInactiveSettings
	WHERE i_id = 1

	SELECT	temp3.userId AS userID,users.Email AS Email, MAX(@templateId) As templateId,
			MAX(DATEDIFF(Day, actionDate, GETDATE())) As daysInactive,
			MAX(CASE WHEN (DATEADD(Day, @inactiveInterv, actionDate)) >= GETDATE() 
				THEN Convert(DATE, DATEADD(Day, @inactiveInterv, actionDate))
				ELSE CONVERT(DATE, GETDATE()) END) AS dateToDisable
	FROM	#temp3 as temp3 Inner JOIN 
			alod.dbo.core_Users AS USERS ON users.userID = temp3.userId INNER JOIN
			alod.dbo.core_lkupGrade As grade ON users.rank_code = grade.CODE LEFT JOIN
			alod.dbo.core_logEmail As eLog on temp3.userId = eLog.user_id AND eLog.templateId = @templateId
	WHERE DATEDIFF(DAY, actionDate, GETDATE() ) >= @inactiveInterv - @notifInterv AND @active = 1  AND users.accessStatus = 3 AND users.Email IS NOT NULL AND 1 =
		CASE WHEN 
				(SELECT elog.date_sent) IS NOT NULL
				AND DATEDIFF(Day, 
						(SELECT MAX(elog2.date_sent) FROM alod.dbo.core_logEmail as elog2
				 		 WHERE elog2.user_id = users.userID and templateId = @templateId)
					, GETDATE()) >=  @inactiveInterv - @notifInterv   THEN 1
			WHEN (SELECT elog.date_sent) IS NULL THEN 1
			ELSE 0 END
	GROUP BY temp3.userId, users.Email
	
	UNION
	
	SELECT	temp3.userId AS userID,users.Email2 AS Email, MAX(@templateId) As templateId,
			MAX(DATEDIFF(Day, actionDate, GETDATE())) As daysInactive,
			MAX(CASE WHEN (DATEADD(Day, @inactiveInterv, actionDate)) >= GETDATE() 
				THEN Convert(DATE, DATEADD(Day, @inactiveInterv, actionDate))
				ELSE CONVERT(DATE, GETDATE()) END) AS dateToDisable
	FROM	#temp3 as temp3 Inner JOIN 
			alod.dbo.core_Users AS USERS ON users.userID = temp3.userId INNER JOIN
			alod.dbo.core_lkupGrade As grade ON users.rank_code = grade.CODE LEFT JOIN
			alod.dbo.core_logEmail As eLog on temp3.userId = eLog.user_id AND eLog.templateId = @templateId
	WHERE DATEDIFF(DAY, actionDate, GETDATE() ) >= @inactiveInterv - @notifInterv AND @active = 1 AND users.accessStatus = 3 AND users.Email2 IS NOT NULL AND 1 =
		CASE WHEN 
				(SELECT elog.date_sent) IS NOT NULL
				AND DATEDIFF(Day, 
						(SELECT MAX(elog2.date_sent) FROM alod.dbo.core_logEmail as elog2
				 		 WHERE elog2.user_id = users.userID and templateId = @templateId)
					, GETDATE()) >=  @inactiveInterv - @notifInterv   THEN 1
			WHEN (SELECT elog.date_sent) IS NULL THEN 1
			ELSE 0 END
	GROUP BY temp3.userId, users.Email2
	
		UNION
	
	SELECT	temp3.userId AS userID,users.Email3 AS Email, MAX(@templateId) As templateId,
			MAX(DATEDIFF(Day, actionDate, GETDATE())) As daysInactive,
			MAX(CASE WHEN (DATEADD(Day, @inactiveInterv, actionDate)) >= GETDATE() 
				THEN Convert(DATE, DATEADD(Day, @inactiveInterv, actionDate))
				ELSE CONVERT(DATE, GETDATE()) END) AS dateToDisable
	FROM	#temp3 as temp3 Inner JOIN 
			alod.dbo.core_Users AS USERS ON users.userID = temp3.userId INNER JOIN
			alod.dbo.core_lkupGrade As grade ON users.rank_code = grade.CODE LEFT JOIN
			alod.dbo.core_logEmail As eLog on temp3.userId = eLog.user_id AND eLog.templateId = @templateId
	WHERE DATEDIFF(DAY, actionDate, GETDATE() ) >= @inactiveInterv - @notifInterv AND @active = 1  AND users.accessStatus = 3 AND users.Email3 IS NOT NULL AND 1 =
		CASE WHEN 
				(SELECT elog.date_sent) IS NOT NULL
				AND DATEDIFF(Day, 
						(SELECT MAX(elog2.date_sent) FROM alod.dbo.core_logEmail as elog2
				 		 WHERE elog2.user_id = users.userID and templateId = @templateId)
					, GETDATE()) >=  @inactiveInterv - @notifInterv   THEN 1
			WHEN (SELECT elog.date_sent) IS NULL THEN 1
			ELSE 0 END
	GROUP BY temp3.userId, users.Email3

END
GO

