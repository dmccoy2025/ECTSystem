
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 1/25/2016
-- Description:	Returns all Workflows assocated with the specified Case Type
--				Id. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_CaseType_sp_GetCaseTypeWorkflows]
	@caseTypeId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@caseTypeId, 0) <= 0)
	BEGIN
		RETURN
	END
	
	SELECT	w.workflowId
	FROM	core_Workflow_CaseType_Map wctm
			JOIN core_Workflow w ON wctm.WorkflowId = w.workflowId
	WHERE	wctm.CaseTypeId = @caseTypeId
END
GO

