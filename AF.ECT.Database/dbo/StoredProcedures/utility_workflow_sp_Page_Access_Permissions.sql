
-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	9/9/2016
-- Description:		Altered to take in the ID of the workflow instead of the
--					workflow title. This is to allow the proper selection of
--					the workstatus for modules which have multiple workflows
--					that share the same Status Codes.
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	6/5/2017
-- Description:		PageId is selected based on pages in workflow
-- ============================================================================
CREATE PROCEDURE [dbo].[utility_workflow_sp_Page_Access_Permissions] 
	@workflowId INT
	,@statusTitle VARCHAR(50)
	,@userGroup VARCHAR(50)
    ,@pageTitle VARCHAR(50)
    ,@access INT
AS
BEGIN
	-- Get workflow id
	--DECLARE @workFlowId INT
	--SELECT @workFlowId = workflowId
	--FROM core_Workflow
	--WHERE title = @workFlowTitle
	PRINT '@workFlowId = ' + CONVERT(VARCHAR(50), @workFlowId)

	DECLARE @workflow VARCHAR(50) = (SELECT w.title FROM core_Workflow w WHERE w.workflowId = @workflowId)

	-- Get workstatusId
	DECLARE @statusId INT

	SELECT	@statusId = WS.ws_id
	FROM	core_WorkStatus WS
			INNER JOIN core_Workflow WF
				ON WF.workflowId = WS.workflowId
			INNER JOIN core_StatusCodes SC
				ON SC.statusId = WS.statusId
	WHERE	wf.workflowId = @workflowId
			AND SC.description = @statusTitle
	
	PRINT '@statusId = ' + CONVERT(VARCHAR(50), @statusId)


	-- Get UserGroupId
	DECLARE @userGroupId INT
	SELECT @userGroupId = groupId
	FROM core_UserGroups 
	WHERE name = @userGroup
	PRINT '@userGroupId = ' + CONVERT(VARCHAR(50), @userGroupId)


	-- Get PageId
	DECLARE @pageId INT
	SELECT @pageId = p.pageId
	FROM core_Pages p
	JOIN core_WorkflowViews wv on wv.pageId = p.pageId AND wv.workflowId = @workflowId
	WHERE title = @pageTitle
	PRINT '@pageId = ' + CONVERT(VARCHAR(50), @pageId)
	

	-- Check for existing permissions unchanged
	DECLARE @exists INT
	DECLARE @changed INT
	DECLARE @permId INT
	SET @exists = 0
	SET @changed = 0


	SELECT @exists = COUNT(*) FROM core_PageAccess
	WHERE workflowId = @workFlowId
	AND statusId = @statusId
	AND groupId = @userGroupId
	AND pageId = @pageId

	IF @exists = 1 
		BEGIN
			-- Check for permissions that changed	
			SELECT @changed = COUNT(*) FROM core_PageAccess
			WHERE workflowId = @workFlowId
			AND statusId = @statusId
			AND groupId = @userGroupId
			AND pageId = @pageId
			AND access <> @access

			IF @changed = 1
				BEGIN						
					UPDATE core_PageAccess SET access = @access
						WHERE workflowId = @workFlowId
						AND statusId = @statusId
						AND groupId = @userGroupId
						AND pageId = @pageId
					PRINT 'Page access permissions changed: ' + @workflow + ', ' + @statusTitle + ', ' +@userGroup + ', ' +
																@pageTitle + ', '+ CONVERT(VARCHAR(1), @access) + CHAR(13)
				END
			ELSE
				BEGIN
					PRINT 'Page access permissions already exists: ' + @workflow + ', ' + @statusTitle + ', ' +@userGroup + ', ' + 
															@pageTitle + ', ' + CONVERT(VARCHAR(1), @access) + CHAR(13)
				END
		END
	
	ELSE		
		BEGIN
			-- Else insert new permission	
			INSERT INTO core_PageAccess(workflowId, statusId, groupId, pageId, access) 
								 VALUES(@workFlowId, @statusId, @userGroupId, @pageId, @access)
							 
			PRINT 'New page access permissions added: ' + @workflow + ', ' + @statusTitle + ', ' +@userGroup + ', ' + 
															@pageTitle + ', ' + CONVERT(VARCHAR(1), @access) + CHAR(13)
	
		END
END
GO

