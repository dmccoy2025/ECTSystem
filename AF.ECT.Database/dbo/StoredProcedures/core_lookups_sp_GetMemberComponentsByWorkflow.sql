
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 11/23/2016
-- Description:	Gets all the member component records associated with the
--				specified workflow Id. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookups_sp_GetMemberComponentsByWorkflow]
	@workflowId TINYINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF (ISNULL(@workflowId, 0) = 0)
	BEGIN
		RETURN
	END
	
	SELECT	c.component_id AS Id, c.component_description AS Name
	FROM	core_Workflow_MemberComponent wc
			JOIN core_lkupComponent c ON  wc.ComponentId = c.component_id
	WHERE	wc.WorkflowId = @workflowId
END
GO

