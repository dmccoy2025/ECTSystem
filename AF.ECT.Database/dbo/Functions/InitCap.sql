-- =============================================
-- Author:		Andy Cooper
-- Create date: 
-- Description:	Returns a string with the first letter capitalized, the rest lowercase
-- =============================================
CREATE FUNCTION [dbo].[InitCap] 
(
	-- Add the parameters for the function here
	@value varchar(50)
)
RETURNS varchar(50)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result varchar(50)

	-- Add the T-SQL statements to compute the return value here
	SELECT @Result = (SELECT Left(upper(@value) , 1) + substring(lower(@value), 2, len(@value)))

	-- Return the result of the function
	RETURN @Result

END
GO

