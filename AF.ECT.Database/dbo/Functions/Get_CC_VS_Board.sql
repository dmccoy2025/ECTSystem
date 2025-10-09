-- =============================================
-- Author:		Andy Cooper
-- Create date: 9/1/2009
-- Description:	Counts the number of cases where the Final finding did not match the Wing CC finding
-- =============================================
CREATE FUNCTION [dbo].[Get_CC_VS_Board] 
(
	-- Add the parameters for the function here
	@startDate Date,
	@endDate date
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @count int

	-- Add the T-SQL statements to compute the return value here
	SELECT @count =
	(
		select COUNT(*)
		from vw_lod 
		where Closed = 'Yes'
		and isnull([finding_formal_wing_cc],[finding_wing_cc]) is not null
		and [finding_final] is not null
		and isnull([finding_formal_wing_cc],[finding_wing_cc]) <> [finding_final]
		and [Date Completed] between @startDate and @endDate
		and [finding_final] in (1, 3)
		and isnull([finding_formal_wing_cc],[finding_wing_cc]) in (1, 3, 2, 4, 5, 6)
	)

	-- Return the result of the function
	RETURN @count

END
GO

