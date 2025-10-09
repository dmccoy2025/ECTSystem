
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 1/25/2016
-- Description:	Updates the Workflow to Case Type Maps for the specified Case
--				Type ID. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_CaseType_sp_UpdateCaseTypeWorkflowMaps]
	 @caseTypeId INT,
	 @workflowIds tblIntegerList READONLY
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@caseTypeId, 0) <= 0)
	BEGIN
		RETURN
	END
	
	
	-- Delete all existing records associated with the Case Type Id...
	DELETE	FROM	core_Workflow_CaseType_Map
			WHERE	CaseTypeId = @caseTypeId
	
	-- Insert all workflows associated with the Case Type Id...
	INSERT	INTO	core_Workflow_CaseType_Map ([WorkflowId], [CaseTypeId])
			SELECT	w.n, @caseTypeId
			FROM	@workflowIds w
END
GO

