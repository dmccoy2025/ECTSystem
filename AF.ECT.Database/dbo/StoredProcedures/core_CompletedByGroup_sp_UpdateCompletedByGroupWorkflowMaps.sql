
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 2/2/2016
-- Description:	Updates the Workflow to Completed By Group Maps for the
--				specified Completed By Group ID. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_CompletedByGroup_sp_UpdateCompletedByGroupWorkflowMaps]
	 @completedByGroupId INT,
	 @workflowIds tblIntegerList READONLY
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@completedByGroupId, 0) <= 0)
	BEGIN
		RETURN
	END
	
	
	-- Delete all existing records associated with the Stamp Id...
	DELETE	FROM	core_Workflow_CompletedByGroup_Map
			WHERE	CompletedById = @completedByGroupId
	
	-- Insert all workflows associated with the Stamp Id...
	INSERT	INTO	core_Workflow_CompletedByGroup_Map ([WorkflowId], [CompletedById])
			SELECT	w.n, @completedByGroupId
			FROM	@workflowIds w
END
GO

