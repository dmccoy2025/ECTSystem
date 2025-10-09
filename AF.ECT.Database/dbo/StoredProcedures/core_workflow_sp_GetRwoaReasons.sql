-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 12/19/2016
-- Description:	Selects the RWOA reasons associated with the passed in 
--				workflow ID. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_workflow_sp_GetRwoaReasons]
	@workflowId TINYINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	BEGIN
		SELECT		r.Id, r.Description
		FROM		core_Workflow_RwoaReason wrr
					INNER JOIN core_lkupRWOAReasons r ON wrr.RwoaId = r.Id
		WHERE		wrr.WorkflowId = @workflowId
		ORDER BY	r.ID ASC
	END
END
GO

