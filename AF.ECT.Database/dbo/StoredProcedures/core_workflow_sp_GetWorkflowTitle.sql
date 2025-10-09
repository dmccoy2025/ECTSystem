
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 4/3/2017
-- Description:	Get workflow title or sub workflow title
-- ============================================================================
CREATE PROCEDURE [dbo].[core_workflow_sp_GetWorkflowTitle]
	@moduleId INT,
	@subCase INT
AS
BEGIN

	SELECT TOP 1(CASE @subCase WHEN 0 THEN w.title ELSE sub.subTypeTitle END)
	FROM core_Workflow w
	LEFT JOIN core_lkupSCSubType sub ON sub.associatedWorkflowId = w.workflowId AND sub.subTypeId = @subCase
	where w.moduleId = @moduleId
END
GO

