-- =============================================
-- Author:		Andy Cooper
-- Create date: 
-- Description:	
-- =============================================
CREATE FUNCTION [dbo].[FormatName] 
(
	-- Add the parameters for the function here
	@userId int
)
RETURNS varchar(256)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result varchar(256)

	-- Add the T-SQL statements to compute the return value here
	DECLARE @first varchar(50), @last varchar(50), @mid varchar(50)
	SELECT @first = FirstName, @last = LastName, @mid = MiddleName FROM vw_users WHERE userID = @userId

	IF (len(@mid) >= 1)
		SET @Result = @last + ' ' + @first + ' ' + LEFT(@mid, 1)
	ELSE
		SET @Result = @last + ' ' + @first

	-- Return the result of the function
	RETURN @Result

END
GO

