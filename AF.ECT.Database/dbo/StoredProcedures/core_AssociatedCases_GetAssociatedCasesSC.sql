
-- ============================================================================
-- Author:			Evan Morrison
-- Create date:		6/12/2017
-- Description:		Get SC Associated Cases by the cases workflow and refId
-- ============================================================================
-- Author:			Evan Morrison
-- Create date:		8/1/2017
-- Description:		Fix error of not getting associated special case
-- ============================================================================

CREATE PROCEDURE [dbo].[core_AssociatedCases_GetAssociatedCasesSC]
	@refId	INT,
	@workflowId	INT
AS
BEGIN

	SELECT	*
	FROM	core_AssociatedCases a
	JOIN	core_Workflow w ON w.workflowId = a.associated_workflow
	JOIN	core_lkupModule m ON m.moduleId = w.moduleId
	WHERE	a.refId = @refId
			AND a.workflowId = @workflowId
			AND m.isSpecialCase = 1
			
END
GO

