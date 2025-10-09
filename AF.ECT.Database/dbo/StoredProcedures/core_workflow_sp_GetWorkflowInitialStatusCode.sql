-- ============================================================================
-- Author:		Kenneth Barnett
-- Create date: 7/16/2014
-- Description:	Returns the status code ID of the initial status for a specified workflow and compo.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	8/2/2016
-- Description:		Updated to use the workflowId to select the workstatus Id.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_workflow_sp_GetWorkflowInitialStatusCode]
(
	@compo INT,
	@module INT,
	@workflowId INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	c.statusId
	FROM	core_Workflow a
			LEFT JOIN core_WorkStatus b ON b.workflowId = a.workflowId
			LEFT JOIN core_StatusCodes c ON c.statusId = b.statusId
	WHERE	a.moduleId = @module 
			AND a.compo = @compo
			AND a.workflowId = @workflowId
			AND a.initialStatus = b.ws_id
END
GO

