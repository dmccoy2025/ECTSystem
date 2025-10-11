-- =============================================
-- Author:		Daniel West
-- Create date: 5/5/2008
-- Description:	Stored procedure to retrieve all messages for MsgAdmin.aspx with pagination

CREATE PROCEDURE [dbo].[core_messages_sp_GetAllMessages_pagination]
	@compo char(1),
	@isAdmin bit,
	@PageNumber INT = 1,
	@PageSize INT = 10
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
	OFFSET (@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
END
GO