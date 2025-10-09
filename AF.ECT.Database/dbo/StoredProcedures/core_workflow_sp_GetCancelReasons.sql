-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 10/21/2015
-- Description:	Selects the cancel reasons associated with the passed in 
--				workflow ID. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	8/2/2016
-- Description:		Updated to be able to handle multiple LOD workflows.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_workflow_sp_GetCancelReasons]
	@workflowId TINYINT,
	@isFormal BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- Check if this is an LOD workflow...
	IF @workflowId IN (SELECT w.workflowId FROM core_Workflow w JOIN core_lkupModule m ON w.moduleId = m.moduleId WHERE m.moduleName = 'LOD')
	BEGIN
		-- Cancel reasons are slightly different for formal and informal LOD cases...
		IF @isFormal = 1
		BEGIN
			SELECT		cr.Id, cr.Description
			FROM		core_Workflow_CancelReason wcr
						INNER JOIN core_lkupCancelReason cr ON wcr.CancelReasonId = cr.Id
			WHERE		wcr.WorkflowId = @workflowId
						AND cr.Description <> 'Case started in error by Tech'
			ORDER BY	cr.DisplayOrder ASC, cr.Description
		END
		ELSE
		BEGIN
			SELECT		cr.Id, cr.Description
			FROM		core_Workflow_CancelReason wcr
						INNER JOIN core_lkupCancelReason cr ON wcr.CancelReasonId = cr.Id
			WHERE		wcr.WorkflowId = @workflowId
						AND cr.Description <> 'Formal LOD no longer required'
			ORDER BY	cr.DisplayOrder ASC, cr.Description
		END
	END
	-- All other workflows...
	ELSE
	BEGIN
		SELECT		cr.Id, cr.Description
		FROM		core_Workflow_CancelReason wcr
					INNER JOIN core_lkupCancelReason cr ON wcr.CancelReasonId = cr.Id
		WHERE		wcr.WorkflowId = @workflowId
		ORDER BY	cr.DisplayOrder ASC, cr.Description
	END
END
GO

