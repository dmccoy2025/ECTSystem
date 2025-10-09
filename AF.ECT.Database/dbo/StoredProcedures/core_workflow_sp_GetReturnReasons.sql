-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 3/6/2017
-- Description:	Selects the Return reasons associated with the passed in 
--				workflow ID. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_workflow_sp_GetReturnReasons]
	@workflowId TINYINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	BEGIN
		SELECT		r.Id, r.Description
		FROM		core_Workflow_ReturnReason wrr
					INNER JOIN core_lkupRWOAReasons r ON wrr.return_id = r.Id
		WHERE		wrr.WorkflowId = @workflowId
		ORDER BY	r.ID ASC
	END
END
GO

