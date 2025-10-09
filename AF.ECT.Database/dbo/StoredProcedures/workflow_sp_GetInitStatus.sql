

CREATE PROCEDURE [dbo].[workflow_sp_GetInitStatus]
	@userId int, 
	@workflow tinyint, 
	@groupId int

AS

SET nocount ON 

DECLARE @status int

--SET @userId = 3
--SET @workflow = 1
--SET @groupId = 3


--see if we have a status override
SET @status = (SELECT statusId FROM core_WorkflowInitStatus WHERE workflowId = @workflow AND groupId = @groupId)

--if not use the default
IF (@status IS NULL)
	SET @status = (SELECT initialStatus FROM core_Workflow WHERE workflowId = @workflow)

SELECT @status

--SELECT ws_id Id, workflowId WorkflowId, sortOrder SortOrder, statusId StatusCodeType
--FROM core_WorkStatus 
--WHERE ws_id = @status
GO

