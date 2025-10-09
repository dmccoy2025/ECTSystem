-- =============================================
-- Author:		Andy Cooper
-- Create date: 31 March 2008
-- Description:	Updates the permissions for the specified user
-- =============================================
CREATE PROCEDURE [dbo].[core_permissions_sp_UpdateUserPermissions] 
	@userId smallint,
	@XmlDocument text
AS
BEGIN

	SET NOCOUNT ON;

	SET XACT_ABORT ON 
	BEGIN TRANSACTION

	-- delete existing mappings for this group
	DELETE FROM core_UserPermissions 
	WHERE userId = @userId

	DECLARE @hDoc int 

	EXEC sp_xml_preparedocument @hDoc OUTPUT, @XmlDocument

	--now insert the ones with permission
	INSERT INTO core_UserPermissions (permId, status, userId ) 
	SELECT  permId, status, @userId FROM OPENXML (@hDoc, '/list/item', 1)  
		WITH (permId  smallint, status char(1))

	COMMIT TRANSACTION

	--clean up
	EXEC sp_xml_removedocument @hDoc

    
END
GO

