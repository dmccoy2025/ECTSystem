
-- =============================================
-- Author:		Daniel West
-- Create date: 5/6/2008
-- Description:	Retrieve the details for a specific message.
-- =============================================
CREATE PROCEDURE [dbo].[core_messages_sp_GetMessagesDetails] 
	@messageID int
AS
BEGIN

	SET NOCOUNT ON;
	SELECT 
		title,message,name,popup,startTime,endTime 
	FROM 
		core_Messages
	WHERE 
		messageID = @messageID
END
GO

