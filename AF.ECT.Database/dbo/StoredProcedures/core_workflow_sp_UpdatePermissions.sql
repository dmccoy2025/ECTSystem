-- =============================================
-- Author:		Andy Cooper
-- Create date: 9 May 2008
-- Description:	Updates the permissions for the specified group
-- =============================================
CREATE PROCEDURE [dbo].[core_workflow_sp_UpdatePermissions] 
	@workflowId smallint,
	@compo char(1),
	@XmlDocument text
AS
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @DocHandle int

	SET XACT_ABORT ON 
	BEGIN TRANSACTION

	-- delete existing mappings for this compo
	DELETE FROM core_WorkflowPerms 
	WHERE workflowId = @workflowId
	AND groupId IN (SELECT groupId FROM core_UserGroups WHERE compo = @compo)

	DECLARE @hDoc int 

	EXEC sp_xml_preparedocument @hDoc OUTPUT, @XmlDocument

	--now insert the ones with permission
	INSERT INTO core_WorkflowPerms ( workflowId, groupId, canCreate, canView) 
	SELECT  @workflowId, groupId, canCreate, canView FROM OPENXML (@hDoc, '/list/item', 1)  
		WITH (groupId smallint, canCreate bit, canView bit)

	COMMIT TRANSACTION

	--clean up
	EXEC sp_xml_removedocument @hDoc

    
END
GO

