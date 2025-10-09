
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 2/2/2016
-- Description:	Returns all Workflows assocated with the specified Completed By
--				Group Id. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_CompletedByGroup_sp_GetCompletedByGroupWorkflows]
	@completedByGroupId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@completedByGroupId, 0) <= 0)
	BEGIN
		RETURN
	END
	
	SELECT	w.workflowId
	FROM	core_Workflow_CompletedByGroup_Map wcbgm
			JOIN core_Workflow w ON wcbgm.WorkflowId = w.workflowId
	WHERE	wcbgm.CompletedById = @completedByGroupId
END
GO

