

-- =============================================
-- Author:		Andy Cooper
-- Create date: 14 May 2008
-- Description:	Gets all steps for the given workflow
-- =============================================

CREATE PROCEDURE [dbo].[core_workflow_sp_GetStepsByWorkflowAndStatus]
	@workflow tinyint,
	@status TINYINT,
	@deathStatus AS CHAR(1)

AS

SET NOCOUNT ON

SELECT 
	stepId, workflowId, statusIn, statusOut
	,displayText, stepType, a.active, displayOrder
	,b.description statusInDescr
	,c.description statusOutDescr
	,b.groupId groupInId, e.name groupInName
	,c.groupId groupOutId, f.name groupOutName
	,a.dbSignTemplate
	,(SELECT count(*) FROM core_WorkflowActions WHERE stepId = a.stepId) actions
	,a.deathStatus, a.memoTemplate
FROM
	core_workflowSteps a
JOIN 
	core_StatusCodes b ON b.statusId = a.statusIn
JOIN 
	core_StatusCodes c ON c.statusId = a.statusOut
LEFT JOIN
	core_usergroups e ON e.groupId = b.groupId
LEFT JOIN
	core_userGroups f ON f.groupId = c.groupId
WHERE
	a.workflowId = @workflow
AND
	a.statusIn = @status
AND 
	a.active = 1
AND
	(a.deathStatus = 'A' OR a.deathStatus = @deathStatus) 
ORDER BY
	a.statusIn, a.displayOrder
GO

