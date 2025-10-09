
-- =============================================
-- Author:		Daniel West
-- Create date: 5/7/2008
-- Description:	Stored procedure to insert a message.
-- =============================================
CREATE PROCEDURE [dbo].[core_messages_sp_UpdateMessages] 
	-- Add the parameters for the stored procedure here
	@messageID int,
	@title varchar(50),
	@name varchar(50),
	@startTime datetime,
	@endTime datetime,
	@popup bit,
	@message varchar(1024)
AS
BEGIN

	SET NOCOUNT ON;

	UPDATE 
		core_Messages
	SET message = @message,
		title = @title,
		name = @name,
		popup = @popup,
		startTime = @startTime,
		endTime = @endTime
	WHERE messageID = @messageID 
END
GO

