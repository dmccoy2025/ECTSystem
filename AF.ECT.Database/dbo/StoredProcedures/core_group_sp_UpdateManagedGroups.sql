-- =============================================
-- Author:		Andy Cooper
-- Create date: 28 March 2008
-- Description:	Updates the groups managed by the specified group
-- =============================================
CREATE PROCEDURE [dbo].[core_group_sp_UpdateManagedGroups] 
	@groupId smallint = 0,
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
	DELETE FROM core_UserGroupsManagedBy 
	WHERE managedby = @groupId

	DECLARE @hDoc int 

	EXEC sp_xml_preparedocument @hDoc OUTPUT, @XmlDocument

	--now insert the ones with permission
	INSERT INTO core_UserGroupsManagedBy (groupId, managedBy, notify) 
	SELECT  groupId, @groupId, notify FROM OPENXML (@hDoc, '/GroupList/Group', 1)  
		WITH (groupId  smallint, managed bit, notify bit)
	WHERE managed = 1

	COMMIT TRANSACTION

	--clean up
	EXEC sp_xml_removedocument @hDoc

    
END
GO

