
-- ============================================================================
-- Author:		Kenneth Barnett
-- Create date: 3/20/2017
-- Description:	Determines if the specified Restricted SARC case has been fully  
--				completed (i.e. the final determination memo has been created 
--				and the member has bee notified).
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	7/27/2017
-- Description:		- Removed the checks for member informed and determination
--					memo in favor of checking the is_post_processing_complete
--					field. The is_post_processing_complete field will only be
--					1 if the member has been informed and the determination
--					memo has been generated.
-- ============================================================================
CREATE FUNCTION [dbo].[fn_IsRestrictedSARCFullyCompleted]
(
	@refId INT
)
RETURNS BIT
AS
BEGIN
	DECLARE @result BIT = 0,
			@count INT = 0

	IF (ISNULL(@refId, 0) <= 0)
		RETURN @result

	SELECT	
			@count = COUNT(*)
	FROM	
			Form348_SARC sr
			JOIN vw_WorkStatus ws ON sr.status = ws.ws_id
	WHERE	
			sr.sarc_id = @refId
			AND ws.isFinal = 1
			AND
			(
				ws.isCancel = 1
				OR
				sr.duty_status = 5
				OR
				sr.is_post_processing_complete = 1
			)

	IF (ISNULL(@count, 0) = 1)
		SET @result = 1

	RETURN @result
END
GO

