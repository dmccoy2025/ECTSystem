
-- =============================================
-- Author:		Daniel West
-- Create date: 5/6/2008
-- Description:	Return a list of group names

CREATE PROCEDURE [dbo].[core_messages_sp_GetMessagesGroups] 
	-- Add the parameters for the stored procedure here
	@messageID int,
	@compo int,
	@groupId smallint
AS
BEGIN

	SET NOCOUNT ON;

	SELECT 
		core_UserGroups.groupID, name, 
		CASE 
			WHEN core_MessagesGroups.groupID IS NULL 
			THEN 0 
			ELSE 1 
		END AS assigned
	FROM 
		core_UserGroups
		LEFT JOIN 
			core_MessagesGroups on core_MessagesGroups.MessageID = @messageID 
			AND 
				(core_UserGroups.groupID = core_MessagesGroups.groupID OR core_MessagesGroups.groupID = 0) 		
		WHERE 
			(messageId = @messageID or messageID is null) and (compo = @compo or @groupId = 2) --sys admin sees all
END
GO

