
-- =============================================
-- Author:		Daniel West
-- Create date: 5/7/2008
-- Description:	Stored procedure to insert a message.
-- =============================================
CREATE PROCEDURE [dbo].[core_messages_sp_InsertMessages] 
	-- Add the parameters for the stored procedure here
	@title varchar(50),
	@name varchar(50),
	@startTime datetime,
	@endTime datetime,
	@popup bit,
	@message varchar(1024),
	@userID int
AS
BEGIN

	SET NOCOUNT ON;

	INSERT INTO 
		core_Messages
			(message,title,name,createdBy,popup,startTime,endTime)
		Values
			(@message,@title,@name,@userID,@popup,@startTime,@endTime)

	SELECT SCOPE_IDENTITY()
	
END
GO

