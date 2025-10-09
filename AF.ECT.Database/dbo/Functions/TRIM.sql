
-- ============================================================================
-- Author:			Ken Barnett
-- Created Date:	6/28/2016
-- Work Item:		TFS User Story 120
-- Description:		Removes leading and trailing whitespace from a string. 
-- ============================================================================
CREATE FUNCTION [dbo].[TRIM] 
(
	@string NVARCHAR(MAX)
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
	RETURN LTRIM(RTRIM(@string))
END
GO

