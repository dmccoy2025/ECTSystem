-- =============================================
-- Author:		Andy Cooper
-- Create date: 
-- Description:	
-- =============================================
CREATE FUNCTION [dbo].[FormatMemberName] 
(
	-- Add the parameters for the function here
	@ssn char(9)
)
RETURNS varchar(256)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result varchar(256)

	-- Add the T-SQL statements to compute the return value here
	DECLARE @first varchar(50), @last varchar(50), @mid varchar(50)
	SELECT @first = First_Name, @last = Last_Name, @mid = Middle_Names FROM MemberData WHERE SSAN = @ssn

	IF (len(@mid) >= 1)
		SET @Result = @last + ' ' + @first + ' ' + LEFT(@mid, 1)
	ELSE
		SET @Result = @last + ' ' + @first

	-- Return the result of the function
	RETURN upper(@Result)

END
GO

