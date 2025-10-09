
-- =============================================
-- Author:		Kamal Singh
-- Create date: 8/21/2014
-- Description:	Updates Count value after email are sent ount
-- =============================================
CREATE PROCEDURE [dbo].[ReminderEmailUpdate]
	@pId bigint
	
AS
BEGIN

	UPDATE dbo.ReminderEmails
	SET sentCount = sentCount + 1,
		lastSentDate = getdate()
	Where id = @pId

END
GO

