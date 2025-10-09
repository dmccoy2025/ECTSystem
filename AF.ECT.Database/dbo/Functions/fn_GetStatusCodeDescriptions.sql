-- ============================================================================
-- Author:		Kenneth Barnett
-- Create date: 9/20/2016
-- Description:	Returns a CSV of StatusCode descriptions from a passed in set
--				of CSV data. The data can either be WorkStatus IDs or 
--				StatusCode IDs.
--				If 0 or NULL is passed in for @areWorkStatusIDs parameter then
--				@csvIDs is assumed to contain StatusCode IDs. 
-- ============================================================================
CREATE FUNCTION [dbo].[fn_GetStatusCodeDescriptions] 
(
	@csvIDs NVARCHAR(MAX),
	@areWorkStatusIDs BIT
)

RETURNS NVARCHAR(MAX)

AS

BEGIN
	-- Declare the return variable here
	DECLARE @Result NVARCHAR(MAX)

	IF (ISNULL(@areWorkStatusIDs, 0) = 0)	-- StatusCode IDs
	BEGIN
		SELECT	@Result = COALESCE(@Result + ',', '')  + ISNULL(StatusCodes.description, '')
		FROM	(
					SELECT	sc.description
					FROM	fn_Split(@csvIDs, ',') rData
							JOIN core_StatusCodes sc ON rdata.value = sc.statusId
				) AS StatusCodes
	END
	ELSE	-- WorkStatus IDs
	BEGIN
		SELECT	@Result = COALESCE(@Result + ',', '')  + ISNULL(StatusCodes.description, '')
		FROM	(
					SELECT	sc.description
					FROM	fn_Split(@csvIDs, ',') rData
							JOIN core_WorkStatus ws ON rdata.value = ws.ws_id
							JOIN core_StatusCodes sc ON ws.statusId = sc.statusId
				) AS StatusCodes
	END

	-- Return the result of the function
	RETURN @Result

END
GO

