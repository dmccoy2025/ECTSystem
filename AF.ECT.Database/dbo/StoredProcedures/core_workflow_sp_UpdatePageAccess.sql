

--=========================================================
-- Modified By:		Evan Morrison
-- Modified Date:	7/24/2017
-- Description:		workflow workstatus now takes in an
--					integer
--=========================================================
CREATE PROCEDURE [dbo].[core_workflow_sp_UpdatePageAccess]
	@compo char(1),
	@workflow tinyint,
	@status INT,
	@XmlDocument text

AS

SET NOCOUNT ON
SET XACT_ABORT ON

BEGIN TRANSACTION

--first clear out all the pages for this workflow and status and groups
DELETE FROM 
	core_PageAccess 
WHERE 
	workflowId = @workflow 
AND 
	statusId = @status
AND
	groupId IN (SELECT groupId FROM core_UserGroups WHERE compo = @compo)

--now add all of the new ones
DECLARE @hDoc int 
EXEC sp_xml_preparedocument @hDoc OUTPUT, @XmlDocument

INSERT INTO core_PageAccess ( workflowId, statusId, groupId, pageId, access )
SELECT  @workflow, @status, groupId, pageId, access FROM OPENXML (@hDoc, '/pages/group', 1)  
	WITH (groupId tinyint, pageId smallint, access tinyint)

--clean up
EXEC sp_xml_removedocument @hDoc

COMMIT TRANSACTION
GO

