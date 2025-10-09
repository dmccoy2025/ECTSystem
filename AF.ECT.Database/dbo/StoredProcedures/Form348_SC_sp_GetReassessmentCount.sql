
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 2/9/2016
-- Description:	Returns the number of reassessment special cases exist for the
--				specified reference Id. 
-- ============================================================================
CREATE PROCEDURE [dbo].[Form348_SC_sp_GetReassessmentCount]
	@originalRefId INT
AS
BEGIN
	SET NOCOUNT ON;
	
	IF (ISNULL(@originalRefId, 0) = 0)
	BEGIN
		RETURN 0
	END
	
	SELECT	COUNT(*)
	FROM	Form348_SC_Reassessment scr
	WHERE	scr.OriginalRefId = @originalRefId
END
GO

