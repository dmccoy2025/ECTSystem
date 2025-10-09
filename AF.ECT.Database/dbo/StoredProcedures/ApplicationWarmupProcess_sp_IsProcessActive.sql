
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 3/30/2016
-- Description:	Returns the Active value of the specified application warmup 
--				process.
-- ============================================================================
CREATE PROCEDURE [dbo].[ApplicationWarmupProcess_sp_IsProcessActive]
	@processName NVARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@processName, '') = '')
	BEGIN
		SELECT 0
		RETURN
	END
	
	DECLARE @result BIT = 0
	
	SELECT	@result = awp.Active
	FROM	ApplicationWarmupProcess awp
	WHERE	awp.Name = @processName
	
	IF (@result IS NULL)
	BEGIN
		SELECT 0
		RETURN
	END
	
	SELECT CONVERT(INT,@result)
END
GO

