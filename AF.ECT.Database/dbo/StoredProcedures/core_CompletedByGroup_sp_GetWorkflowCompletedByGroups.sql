
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 2/2/2016
-- Description:	Returns all of the Completed by Group records mapped to the 
--				specified workflow Id. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_CompletedByGroup_sp_GetWorkflowCompletedByGroups]
	@workflowId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@workflowId, 0) <= 0)
	BEGIN
		RETURN
	END
	
	SELECT	cbg.Id, cbg.Name
	FROM	core_Workflow_CompletedByGroup_Map wcbgm
			JOIN core_CompletedByGroup cbg ON wcbgm.CompletedById = cbg.Id
	WHERE	wcbgm.WorkflowId = @workflowId
END
GO

