-- =============================================
-- Author:		Andy Cooper
-- Create date: 25 February 2008
-- Description:	Formats an SSN for display
-- =============================================
CREATE FUNCTION [dbo].[FormatPhone] 
(
	-- Add the parameters for the function here
	@phone varchar(20)
)
RETURNS varchar(20)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result varchar(20)

	-- Add the T-SQL statements to compute the return value here\
	SET @Result =  LEFT(@phone,3) + '-' + substring(@phone, 4, 3) + '-' + RIGHT(@phone, 4)

	-- Return the result of the function
	RETURN @Result

END
GO

