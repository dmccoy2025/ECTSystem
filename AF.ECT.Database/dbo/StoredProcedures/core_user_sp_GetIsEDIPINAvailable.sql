
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 12/14/2015
-- Description:	Returns whether or not a EDIPIN can be used for a new user
--				account.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_user_sp_GetIsEDIPINAvailable]
	@edipin VARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @count INT = 0

	SELECT	@count = COUNT(*)
	FROM	core_Users u
	WHERE	u.EDIPIN = @edipin
	
	IF (@count > 0)
	BEGIN
		SELECT 0	-- FALSE
	END
	ELSE
	BEGIN
		SELECT 1	-- TRUE
	END
END
GO

