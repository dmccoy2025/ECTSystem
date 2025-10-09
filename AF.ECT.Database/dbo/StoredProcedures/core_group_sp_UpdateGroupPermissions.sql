-- =============================================
-- Author:		Andy Cooper
-- Create date: 31 March 2008
-- Description:	Updates the permissions for the specified group
-- =============================================
CREATE PROCEDURE [dbo].[core_group_sp_UpdateGroupPermissions] 
	@groupId smallint,
	@XmlDocument text
AS
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @DocHandle int
		
	-- Create an internal representation of the XML document.
	EXEC sp_xml_preparedocument @DocHandle OUTPUT, @XmlDocument


	SET XACT_ABORT ON 
	BEGIN TRANSACTION

	-- delete existing mappings for this group
	DELETE FROM core_GroupPermissions 
	WHERE groupId = @groupId

	DECLARE @hDoc int 

	EXEC sp_xml_preparedocument @hDoc OUTPUT, @XmlDocument

	--now insert the ones with permission
	INSERT INTO core_GroupPermissions (permId, groupId ) 
	SELECT  permId, @groupId FROM OPENXML (@hDoc, '/list/item', 1)  
		WITH (permId  smallint)

	COMMIT TRANSACTION

	--clean up
	EXEC sp_xml_removedocument @hDoc

    
END
GO

