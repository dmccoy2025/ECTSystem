
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 1/20/2016
-- Description:	Returns all of the Certification Stamp records mapped to the 
--				specified workflow Id. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_CertificationStamp_sp_GetWorkflowCertificationStamps]
	@workflowId INT,
	@isQualified BIT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@workflowId, 0) <= 0)
	BEGIN
		RETURN
	END
	
	SELECT	cs.Id, cs.Name, cs.Body, cs.IsQualified
	FROM	core_Workflow_CertificationStamp_Map wcsm
			JOIN core_CertificationStamp cs ON wcsm.CertStampId = cs.Id
	WHERE	wcsm.WorkflowId = @workflowId
			AND (
				cs.IsQualified = @isQualified 
				OR @isQualified IS NULL
			)
END
GO

