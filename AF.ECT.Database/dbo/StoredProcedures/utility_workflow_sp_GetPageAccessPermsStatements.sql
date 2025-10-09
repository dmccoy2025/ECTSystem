-- ============================================================================
-- Author:		Steve Kennedy (original script), Ken Barnett (stored procedure)
-- Create date: 7/17/2015
-- Description:	Outputs EXEC statements for inserting/updating the page access
--				permissions for a specific workflow. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	9/9/2016
-- Description:		Altered to take in the ID of the workflow instead of the
--					workflow title. This is to allow the proper selection of
--					the workstatus for modules which have multiple workflows
--					that share the same Status Codes.
-- ============================================================================
CREATE PROCEDURE [dbo].[utility_workflow_sp_GetPageAccessPermsStatements]
	@workflowId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT	'EXEC utility_workflow_sp_Page_Access_Permissions ',      
			'''' + @workflowId + ''',' AS WorkflowId  -- USE THIS LINE TO GET ONE WORKFLOW        
			,'''' + SC.description + ''','  AS StatusTitle      
			,'''' + UG.name + ''','  AS UserGroup     
			,'''' + P.title + ''','  AS PageTitle      
			,PA.[access]        

	FROM	core_PageAccess PA
			LEFT JOIN core_Workflow WF ON WF.workflowId = PA.workflowId
			INNER JOIN core_WorkStatus WS ON WS.ws_id = PA.statusId  	
			INNER JOIN core_StatusCodes SC ON SC.statusId = WS.statusId
			LEFT JOIN core_Pages P ON P.pageId = PA.pageId  	
			LEFT JOIN core_UserGroups UG ON UG.groupId = PA.groupId  

	WHERE	WF.workflowId = @workflowId
END
GO

