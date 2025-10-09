
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 1/22/2016
-- Description:	Updates the Workflow to Certification Stamp Maps for the
--				specified Stamp ID. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_CertificationStamp_sp_UpdateCertificationStampWorkflowMaps]
	 @stampId INT,
	 @workflowIds tblIntegerList READONLY
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@stampId, 0) <= 0)
	BEGIN
		RETURN
	END
	
	
	-- Delete all existing records associated with the Stamp Id...
	DELETE	FROM	core_Workflow_CertificationStamp_Map
			WHERE	CertStampId = @stampId
	
	-- Insert all workflows associated with the Stamp Id...
	INSERT	INTO	core_Workflow_CertificationStamp_Map ([WorkflowId], [CertStampId])
			SELECT	w.n, @stampId
			FROM	@workflowIds w
END
GO

