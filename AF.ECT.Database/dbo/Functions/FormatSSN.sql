-- =============================================
-- Author:		Andy Cooper
-- Create date: 25 February 2008
-- Description:	Formats an SSN for display
-- =============================================
CREATE FUNCTION [dbo].[FormatSSN] 
(
	-- Add the parameters for the function here
	@SSN char(9)
)
RETURNS char(11)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result char(11)

	-- Add the T-SQL statements to compute the return value here\
	SET @Result =  LEFT(@SSN,3) + '-' + substring(@SSN, 4, 2) + '-' + RIGHT(@SSN, 4)

	-- Return the result of the function
	RETURN @Result

END
GO

