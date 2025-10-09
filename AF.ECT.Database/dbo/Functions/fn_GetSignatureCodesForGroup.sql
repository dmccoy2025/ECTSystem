-- =============================================
-- Author:		Andy Cooper
-- Create date: 24 Oct 2008
-- Description:	Returns a list of status codes a user group has signature authority for
-- =============================================
CREATE FUNCTION [dbo].[fn_GetSignatureCodesForGroup] 
(
	-- Add the parameters for the function here
	@groupId tinyint
)
RETURNS varchar(500)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result varchar(500)

	-- Add the T-SQL statements to compute the return value here
	select @Result = coalesce(@Result + ', ', '') + cast(ws_id AS varchar) from vw_workstatus
	WHERE groupId = @groupId

	select @Result = coalesce(@Result + ', ', '') + cast(status AS varchar) from core_statusCodeSigners
	WHERE groupId = @groupId

	-- Return the result of the function
	RETURN @Result

END
GO

