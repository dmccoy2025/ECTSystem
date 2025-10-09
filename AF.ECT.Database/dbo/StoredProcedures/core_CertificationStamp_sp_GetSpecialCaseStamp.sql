
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 1/22/2016
-- Description:	Returns a record for the stamp associated with the specified
--				special case case Id. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	5/19/2016	
-- Description:		Modified to be able to select the secondary cert stamp
--					for a case.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_CertificationStamp_sp_GetSpecialCaseStamp]
	@refId INT,
	@selectSecondary BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@refId, 0) <= 0)
	BEGIN
		RETURN
	END
	
	IF (ISNULL(@selectSecondary, 0) = 1)
	BEGIN
		SELECT	cs.Id, cs.Name, cs.Body, cs.IsQualified
		FROM	Form348_SC s
				JOIN core_CertificationStamp cs ON s.secondary_certification_stamp = cs.Id
		WHERE	s.SC_Id = @refId
	END
	ELSE
	BEGIN
		SELECT	cs.Id, cs.Name, cs.Body, cs.IsQualified
		FROM	Form348_SC s
				JOIN core_CertificationStamp cs ON s.certification_stamp = cs.Id
		WHERE	s.SC_Id = @refId
	END
END
GO

