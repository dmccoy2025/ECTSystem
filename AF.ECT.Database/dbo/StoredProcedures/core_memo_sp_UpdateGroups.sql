-- =============================================
-- Author:		Nick McQuillen
-- Create date: 20 May 2008
-- Description:	updates groups used by a template
-- =============================================
CREATE PROCEDURE [dbo].[core_memo_sp_UpdateGroups] 
	@templateId tinyint,
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
	DELETE FROM core_MemoGroups
	WHERE templateId = @templateId

	DECLARE @hDoc int 

	EXEC sp_xml_preparedocument @hDoc OUTPUT, @XmlDocument

	--now insert the ones with permission
	INSERT INTO core_MemoGroups(templateId, groupId, canView, canEdit, canDelete, canCreate) 
	SELECT  templateId, groupId, canView, canEdit, canDelete, canCreate FROM OPENXML (@hDoc, '/RoleList/Role', 1)  
		WITH (templateId tinyint, groupId tinyint, canView bit, canEdit bit, canDelete bit, canCreate bit)
	
	COMMIT TRANSACTION

	--clean up
	EXEC sp_xml_removedocument @hDoc

    
END
GO

