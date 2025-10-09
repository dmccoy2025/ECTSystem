-- =============================================
-- Author:		
-- Create date: 
-- Description:	
-- =============================================
Create PROCEDURE [dbo].[ReminderEmailSettingsDeleteByStatus] 
		@pworkflowId bigint,
		@pstatusId bigint

	
AS
BEGIN

DELETE FROM ReminderEmails
Where settingId IN (SELECT setting.id From ReminderEmailSettings As setting Inner Join
[dbo].[core_WorkStatus] AS workStatus ON workStatus.ws_id = setting.wsID
Where setting.workflowId = @pworkflowId AND workStatus.statusId = @pstatusId
 )


DELETE ReminderEmailSettings From ReminderEmailSettings As setting Inner Join
[dbo].[core_WorkStatus] AS workStatus ON workStatus.ws_id = setting.wsID
Where setting.workflowId = @pworkflowId AND workStatus.statusId = @pstatusId




END
GO

