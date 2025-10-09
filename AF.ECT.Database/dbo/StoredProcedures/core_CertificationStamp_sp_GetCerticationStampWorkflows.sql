
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 1/18/2016
-- Description:	Returns all Workflows assocated with the specified Certification
--				Stamp Id. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_CertificationStamp_sp_GetCerticationStampWorkflows]
	@stampId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@stampId, 0) <= 0)
	BEGIN
		RETURN
	END
	
	SELECT	w.workflowId
	FROM	core_Workflow_CertificationStamp_Map wcsm
			JOIN core_Workflow w ON wcsm.WorkflowId = w.workflowId
	WHERE	wcsm.CertStampId = @stampId
END
GO

