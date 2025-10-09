
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 1/12/2016
-- Description:	Updates the Workflow to Disposition Maps for the specified
--				Disposition ID. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookups_sp_UpdateWorkflowDispositionMaps]
	 @dispositionId INT,
	 @workflowIds tblIntegerList READONLY
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@dispositionId, 0) = 0)
	BEGIN
		RETURN
	END
	
	
	-- Delete all existing records associated with the Disposition Id...
	DELETE	FROM	core_Workflow_Disposition_Map
			WHERE	DispositionId = @dispositionId
	
	-- Insert all workflows associated with the Disposition Id...
	INSERT	INTO	core_Workflow_Disposition_Map ([WorkflowId], [DispositionId])
			SELECT	w.n, @dispositionId
			FROM	@workflowIds w
END
GO

