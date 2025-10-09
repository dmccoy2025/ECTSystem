
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 4/14/2017
-- Description:	Get Associated Cases by the cases workflow and refId
-- ============================================================================

CREATE PROCEDURE [dbo].[core_AssociatedCases_GetAssociatedCases]
	@refId	INT,
	@workflowId	INT
AS
BEGIN
	SELECT	*
	FROM	core_AssociatedCases
	WHERE	refId = @refId
			AND workflowId = @workflowId
END
GO

