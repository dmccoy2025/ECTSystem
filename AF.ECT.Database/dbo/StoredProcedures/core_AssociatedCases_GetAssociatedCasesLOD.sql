
-- ============================================================================
-- Author:			Evan Morrison
-- Create date:		6/12/2017
-- Description:		Get LOD Associated Cases by the cases workflow and refId
-- ============================================================================
CREATE PROCEDURE [dbo].[core_AssociatedCases_GetAssociatedCasesLOD]
	@refId	INT,
	@workflowId	INT
AS
BEGIN

	SELECT	*
	FROM	core_AssociatedCases a
	WHERE	a.refId = @refId
			AND a.workflowId = @workflowId
			AND a.associated_workflow IN (1, 27)
			
END
GO

