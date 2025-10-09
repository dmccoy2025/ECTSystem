
-- =============================================
-- Author:		Daniel West
-- Create date: 5/5/2008
-- Description:	Stored procedure to retrieve all messages for MsgAdmin.aspx

CREATE PROCEDURE [dbo].[core_messages_sp_GetAllMessages] 
	@compo char(1),
	@isAdmin bit
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
		messageId,core_Messages.title,startTime,endTime,popup, isAdminMessage
	FROM 
		core_Messages
	JOIN 
		vw_users u ON u.userID = core_Messages.createdBy 
	WHERE 
		((u.compo = @compo) or (@compo = '0'))
		and ((@isAdmin = 0 and isAdminMessage = 0) or (@isAdmin = 1))
	ORDER BY startTime ASC
END
GO

