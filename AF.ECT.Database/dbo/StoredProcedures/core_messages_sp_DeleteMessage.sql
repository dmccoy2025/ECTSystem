
-- =============================================
-- Author:		Nick McQuillen
-- Create date: 10/27/2008
-- Description:	Delete a message
-- =============================================
CREATE PROCEDURE [dbo].[core_messages_sp_DeleteMessage] 
	-- Add the parameters for the stored procedure here
	@messageId smallint
AS
BEGIN

	SET NOCOUNT ON;

	DELETE FROM core_Messages WHERE messageId=@messageId
	DELETE FROM core_MessagesGroups WHERE messageId=@messageId
	DELETE FROM core_MessagesRead WHERE messageId=@messageId
END
GO

