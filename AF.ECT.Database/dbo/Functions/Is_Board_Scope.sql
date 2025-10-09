-- =============================================
-- Author:		Andy Cooper
-- Create date: 9/1/2009
-- Description:	Counts the number of cases where the Final finding did not match the Wing CC finding
-- =============================================
CREATE FUNCTION [dbo].[Is_Board_Scope] 
(
	-- Add the parameters for the function here
	@groupId int
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	Declare @scope int
	Select @scope = 
		Case @groupId
			When 1 Then 2
			When 7 Then 2
			When 8 Then 2
			When 9 Then 2
			When 11 Then 2
			When 88 Then 2
			Else 1
		End
	-- Return the result of the function
	RETURN @scope

END
GO

