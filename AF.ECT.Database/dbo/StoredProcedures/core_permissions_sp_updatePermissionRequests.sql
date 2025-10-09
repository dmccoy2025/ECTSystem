-- =============================================
-- Author:		Andy Cooper
-- Create date: 29 October 2008
-- Description:	Processes a list of user permission requests
-- =============================================
CREATE PROCEDURE [dbo].[core_permissions_sp_updatePermissionRequests] 
	@XmlDocument text
AS
BEGIN

	SET NOCOUNT ON;

	SET XACT_ABORT ON 
	BEGIN TRANSACTION

	DECLARE @hDoc int 

	EXEC sp_xml_preparedocument @hDoc OUTPUT, @XmlDocument

	--grant the ones that are approved
	INSERT INTO core_UserPermissions (permId, status, userId ) 
	SELECT  permId, 'G', userId  FROM OPENXML (@hDoc, '/List/Request', 1)  
		WITH (permId  smallint, userId int, reqGranted bit)
	WHERE reqGranted = 1

	--now remove all of them from the pending table
	DELETE FROM core_permissionRequests WHERE reqId IN (
	SELECT id FROM OPENXML (@hDoc, '/List/Request', 1)
		WITH (id int)
	)
	

	COMMIT TRANSACTION

	--clean up
	EXEC sp_xml_removedocument @hDoc

    
END
GO

