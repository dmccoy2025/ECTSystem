
--DECLARE @fromId int, @toId int
--SET @fromId = 1
--SET @toId = 9

CREATE PROCEDURE [dbo].[core_workflow_sp_CopyWorkflow]
	@fromId int,
	@toId int

AS

SET XACT_ABORT ON
BEGIN TRANSACTION 

--delete existing data from the target workflow
DELETE FROM core_WorkflowPerms WHERE workflowId = @toId

DELETE FROM core_WorkflowActions WHERE stepId IN (
	SELECT stepId FROM core_WorkflowSteps WHERE workflowId = @toId)

DELETE FROM core_WorkflowSteps WHERE workflowId = @toId

DELETE FROM core_WorkflowViews WHERE workflowId = @toId

DELETE FROM core_PageAccess WHERE workflowId = @toId

--copy workflow views
INSERT INTO core_WorkflowViews (workflowId, pageId)
SELECT @toId, pageId FROM core_WorkflowViews WHERE workflowId = @fromId

--copy page accress
INSERT INTO core_PageAccess (workflowId, statusId, groupId, pageId)
SELECT @toId, statusId, groupId, pageId FROM core_PageAccess WHERE workflowId = @fromId

--copy perms
INSERT INTO core_WorkflowPerms
(workflowId, groupId, canView, canCreate)
SELECT 
@toId, groupId, canView, canCreate
FROM core_WorkflowPerms WHERE workflowId = @fromId

--copy steps/actions
DECLARE @stepId int, @newStep int
DECLARE iter CURSOR FOR 
SELECT stepId FROM core_WorkflowSteps WHERE workflowId = @fromId


OPEN iter
FETCH next FROM iter INTO @stepId
WHILE @@fetch_status <> -1
BEGIN
	--copy step
	INSERT INTO core_workflowsteps
	(workflowId, statusIn, statusOut, displayText, stepType, active, displayOrder, dbSignTemplate, deathStatus)
	SELECT 
	@toId, statusIn, statusOut, displayText, stepType, active, displayOrder, dbSignTemplate, deathStatus
	FROM core_WorkflowSteps WHERE stepId = @stepId

	SET @newStep = SCOPE_IDENTITY()
	--copy actions
	INSERT INTO core_WorkflowActions
	(type, stepId, target, data)
	SELECT type, @newStep, target, data FROM core_WorkflowActions WHERE stepId = @stepId
		
FETCH next FROM iter INTO @stepId
END

CLOSE iter
DEALLOCATE iter

COMMIT TRANSACTION
GO

