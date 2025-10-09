-- ============================================================
-- Author:		Tim Jacobs
-- Create date: 12 Feb 2014
-- Description:	Converts comma separated status ids to comma 
--				separated string values based on enums 
--				returned by fn_GetStatusEnums
-- ============================================================
CREATE FUNCTION [dbo].[fn_ConvertStatusIdsToValues] 
(
	@statusIds varchar(100)
)

RETURNS varchar(100) 

AS
BEGIN

DECLARE @statusValues varchar(100)

SELECT	@statusValues = COALESCE(@statusValues + ',', '') + CONVERT(VARCHAR(100), se.value) 
FROM	fn_Split(@statusIds, ',') ids INNER JOIN 
        fn_GetStatusEnums() se ON ids.value = CONVERT(VARCHAR, se.statusId)  
        
if @statusValues = NULL

	set @statusValues = @statusIds

RETURN @statusValues

END
GO

