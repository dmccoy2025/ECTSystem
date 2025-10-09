
CREATE FUNCTION [dbo].[GetCompoForUser] 
(
	@UserId int
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Compo int

	-- Add the T-SQL statements to compute the return value here
	SELECT @Compo = core_Users.workCompo FROM core_Users WHERE userID = @UserId

	-- Return the result of the function
	RETURN @Compo

END
GO

