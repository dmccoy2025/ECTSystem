
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 2/9/2016
-- Description:	Returns the Id of the most recent reassessment special case for
--				the specified original special case Id. 
-- ============================================================================
CREATE PROCEDURE [dbo].[Form348_SC_sp_GetReassessmentIdByOriginalId]
	@originalRefId INT
AS
BEGIN
	SET NOCOUNT ON;
	
	IF (ISNULL(@originalRefId, 0) = 0)
	BEGIN
		RETURN 0
	END
	
	DECLARE @result INT = 0
	
	SET @result = (
		SELECT	TOP 1 scr.ReassessmentRefId 
		FROM	Form348_SC_Reassessment scr
		WHERE	scr.OriginalRefId = @originalRefId
		ORDER	BY scr.ReassessmentRefId DESC
	)
	
	SELECT ISNULL(@result, 0)
END
GO

