-- ================================================================
-- Author:		Tim Jacobs
-- Create date: 12 Feb 2014
-- Description:	Converts comma separated memo status ids to comma 
--				separated string values based on core_memoTemplates
-- ================================================================
CREATE FUNCTION [dbo].[fn_ConvertMemoTemplateIdsToValues] 
(
	@memoTemplateIds varchar(100)
)

RETURNS varchar(100) 

AS
BEGIN

DECLARE @memoValues varchar(100)

SELECT	@memoValues = COALESCE(@memoValues + ',', '') + CONVERT(VARCHAR(100), mt.title) 
FROM	fn_Split(@memoTemplateIds, ',') ids INNER JOIN 
        core_MemoTemplates mt on ids.value = mt.templateId
			        
if @memoValues = NULL

	set @memoValues = @memoTemplateIds

RETURN @memoValues

END
GO

