
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 1/25/2016
-- Description:	Returns all of the Case Type records mapped to the 
--				specified workflow Id. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_CaseType_sp_GetWorkflowCaseTypes]
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
	
	SELECT	ct.Id, ct.Name
	FROM	core_Workflow_CaseType_Map wctm
			JOIN core_CaseType ct ON wctm.CaseTypeId = ct.Id
	WHERE	wctm.WorkflowId = @workflowId
END
GO

