
-- =============================================
-- Author:		Daniel West
-- Create date: 5/7/2008
-- Description:	Stored procedure to insert a message.
-- =============================================
CREATE PROCEDURE [dbo].[core_messages_sp_UpdateMessageGroups] 
	@messageID int, 
	@xml as nText
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @hDoc int 

	DELETE FROM 
		core_MessagesGroups
	WHERE 
		messageId = @messageId

	EXEC sp_xml_preparedocument @hDoc OUTPUT, @xml

	-- Rollback the transaction if there were any errors
		INSERT INTO 
			core_MessagesGroups(messageId,groupId)
		SELECT  
			@messageId,Id 
		FROM 
			OPENXML (@hdoc, '/XML_Array/XMLList',1)  
		WITH   
			(ID varchar(20))

		EXEC sp_xml_removedocument @hDoc
END
GO

