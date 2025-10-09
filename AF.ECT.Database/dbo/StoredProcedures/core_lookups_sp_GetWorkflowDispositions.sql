
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 1/18/2016
-- Description:	Returns all of the Disposition records mapped to the specified
--				workflow Id. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookups_sp_GetWorkflowDispositions]
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
	
	SELECT	d.Id, d.Name
	FROM	core_Workflow_Disposition_Map wdm
			JOIN core_lkupDisposition d ON wdm.DispositionId = d.Id
	WHERE	wdm.WorkflowId = @workflowId
END
GO

