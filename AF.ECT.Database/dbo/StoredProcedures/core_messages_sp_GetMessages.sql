
-- =============================================
-- Author:		Daniel West
-- Create date: 5/1/2008
-- Description:	Retrieves Messages for a particular GroupID
-- =============================================
CREATE PROCEDURE [dbo].[core_messages_sp_GetMessages] 
	@userID int,
	@groupID int,
	@popup bit
AS
BEGIN

	SET NOCOUNT ON;

	SELECT 
		message,name,startTime,title, isAdminMessage 
	FROM 
		core_Messages
	JOIN 
		core_MessagesGroups on core_Messages.MessageID = core_MessagesGroups.MessageID AND (core_MessagesGroups.GroupID = @groupID OR core_MessagesGroups.GroupID = 0)
	LEFT JOIN 
		core_MessagesRead on core_Messages.MessageID = core_MessagesRead.MessageID AND core_MessagesRead.userID = @userID
	WHERE 
		(popup = @popup or @popup = 0)
	AND 
		startTime < getdate()
	AND 
		endTime > getdate()
	AND 
		(core_MessagesRead.MessageID IS NULL or @popup = 0)
	ORDER BY StartTime DESC
END
GO

