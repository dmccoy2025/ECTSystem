
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 1/18/2016
-- Description:	Returns all Workflows assocated with the specified Disposition
--				Id. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookups_sp_GetDispositionWorkflows]
	@dispositionId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@dispositionId, 0) <= 0)
	BEGIN
		RETURN
	END
	
	SELECT	w.workflowId
	FROM	core_Workflow_Disposition_Map wdm
			JOIN core_Workflow w ON wdm.WorkflowId = w.workflowId
	WHERE	wdm.DispositionId = @dispositionId
END
GO

