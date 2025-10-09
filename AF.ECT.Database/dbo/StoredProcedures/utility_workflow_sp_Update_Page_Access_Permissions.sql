
CREATE PROCEDURE [dbo].[utility_workflow_sp_Update_Page_Access_Permissions] 
	@workFlowTitle VARCHAR(50)
	,@statusTitle VARCHAR(50)
	,@userGroup VARCHAR(50)
    ,@pageTitle VARCHAR(50)
    ,@access INT
AS

-- Test values ----------------------
--DECLARE @workFlowTitle VARCHAR(50)
--DECLARE @statusTitle VARCHAR(50)
--DECLARE @userGroup VARCHAR(50)
--DECLARE @pageTitle VARCHAR(50)
--DECLARE @access INT

--SET @workFlowTitle = 'Worldwide Duty (WD)'
--SET @statusTitle = 'WD Approved'
--SET @userGroup = 'System Administrator'
--SET @pageTitle = 'WD Med Tech'
--SET @access = 1
-- ----------------------------------


-- Get workflow id
DECLARE @workFlowId INT
SELECT @workFlowId = workflowId
FROM core_Workflow
WHERE title = @workFlowTitle
PRINT '@workFlowId = ' + CONVERT(VARCHAR(50), @workFlowId)


-- Get workstatusId
DECLARE @statusId INT
SELECT @statusId = WS.ws_id
	--,WF.title
	--,SC.description

FROM core_WorkStatus WS
INNER JOIN core_Workflow WF
	ON WF.workflowId = WS.workflowId
INNER JOIN core_StatusCodes SC
	ON SC.statusId = WS.statusId

WHERE WF.title = @workFlowTitle
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
SELECT @pageId = pageId
FROM core_Pages
WHERE title = @pageTitle
PRINT '@pageId = ' + CONVERT(VARCHAR(50), @pageId)
	
PRINT '@access = ' + CONVERT(VARCHAR(1), @access)


INSERT INTO core_PageAccess(workflowId, statusId, groupId, pageId, access) 
					 VALUES(@workFlowId, @statusId, @userGroupId, @pageId, @access)
					 
PRINT 'New page access permissions added: ' + @workFlowTitle + ', ' + @statusTitle + ', ' +@userGroup + ', ' + 
														@pageTitle + ', ' + CONVERT(VARCHAR(1), @access) + CHAR(13)
GO

