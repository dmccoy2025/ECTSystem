
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 2/10/2016
-- Description:	Returns 1 if the specified Id has an a reassessment case
--				associated with it; otherweise 1 is returned.
-- ============================================================================
CREATE PROCEDURE [dbo].[Form348_SC_sp_GetHasReassessmentSC]
	@refId INT
AS
BEGIN
	SET NOCOUNT ON;
	
	IF (ISNULL(@refId, 0) = 0)
	BEGIN
		RETURN 0
	END
	
	DECLARE @count INT = 0

	SELECT	@count = COUNT(*)
	FROM	Form348_SC_Reassessment scr
	WHERE	scr.OriginalRefId = @refId
	
	IF (ISNULL(@count,0) > 0)
	BEGIN
		SELECT 1	-- TRUE
	END
	ELSE
	BEGIN
		SELECT 0	-- FALSE
	END
END
GO

