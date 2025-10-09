
-- =============================================
-- Author:		
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[ReminderEmailSettingsDelete] 
		@pid bigint

	
AS
BEGIN

DELETE FROM ReminderEmails
Where settingId = @pid

DELETE From ReminderEmailSettings
Where id = @pid




END
GO

