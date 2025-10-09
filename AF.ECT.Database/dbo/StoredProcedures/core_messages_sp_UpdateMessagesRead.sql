
-- =============================================
-- Author:		Daniel West
-- Create date: 5/1/2008
-- Description:	Mark all messages as read.
-- =============================================
CREATE PROCEDURE [dbo].[core_messages_sp_UpdateMessagesRead] 
	@userID int,
	@groupID int
AS
BEGIN

	SET NOCOUNT ON;

	INSERT INTO 
		core_MessagesRead
			(MessageId,UserID,DateRead)
		SELECT 
			core_Messages.MessageID,@userID,getdate() 
		FROM core_Messages 
		JOIN 
			core_MessagesGroups on core_Messages.MessageID = core_MessagesGroups.MessageID 
			AND 
				(core_MessagesGroups.GroupID = @groupID 
				OR 
				core_MessagesGroups.GroupID = 0)
		LEFT JOIN 
			core_MessagesRead on core_Messages.MessageID = core_MessagesRead.MessageID 
			AND 
				core_MessagesRead.userID = @userID
		WHERE popup = 1
		AND 
			StartTime < getdate()
		AND 
			EndTime > getdate()
		AND 
			core_MessagesRead.MessageID IS NULL
END
GO

