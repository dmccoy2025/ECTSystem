--
--DECLARE @refId int
--SET @refId = 99
--
--DECLARE @groupId int
--SET @groupId = 3


--core_workflow_sp_GetActiveCases 99

CREATE PROCEDURE [dbo].[core_workflow_sp_GetActiveCases]
	@refId int
	,@groupId smallint
AS


DECLARE @items TABLE
(
	type tinyint,
	refId int,
	description varchar(40),
	title varchar(40),
	moduleName varchar(40),
	parentId INt,
	workflow tinyint
)

-- LODs
INSERT INTO @items
SELECT m.moduleId, lodid refId, s.description, w.title, m.moduleName, @refId parentId, a.workflow
FROM Form2173 a 
JOIN core_StatusCodes s ON s.statusId = a.status
JOIN core_Workflow w ON w.workflowId = a.workflow
JOIN core_lkupModule m ON m.moduleId = w.moduleId
WHERE a.lodId = @refId

-- INCAP
INSERT INTO @items
SELECT m.moduleId, refid, s.description, w.title, m.moduleName, @refId parentId, a.workflow
FROM Incap a
JOIN core_StatusCodes s ON s.statusId = a.status
JOIN core_Workflow w ON w.workflowId = a.workflow
JOIN core_lkupModule m ON m.moduleId = w.moduleId
WHERE a.refId = @refId

-- Extensions

INSERT INTO @items
SELECT m.moduleId, a.extId, s.description, w.title, m.moduleName, @refId parentId, a.workflow
FROM IncapExtensions a
JOIN core_StatusCodes s ON s.statusId = a.status
JOIN core_Workflow w ON w.workflowId = a.workflow
JOIN core_lkupModule m ON m.moduleId = w.moduleId
WHERE a.refId = @refId

-- MMSO Pre-Auths 

INSERT INTO @items
SELECT  m.moduleId, formId refId, s.description, w.title, m.moduleName, a.lodid parentId, a.workflow
FROM MMSOForms a
JOIN core_StatusCodes s ON s.statusId = a.status
JOIN core_Workflow w ON w.workflowId = a.workflow
JOIN core_lkupModule m ON m.moduleId = w.moduleId
WHERE a.formId = (SELECT TOP 1 formId FROM MMSOForms WHERE lodid = @refId ORDER BY createdDate DESC)


SELECT * FROM @items
WHERE workflow IN (SELECT workflowId FROM core_WorkflowPerms WHERE groupId = @groupId)
GO

