
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 11/11/2015
-- Description:	Selects the title from the core_UsersAltTitles table whose
--				UserId and GroupId matches the values passed into the stored
--				procedure.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_user_sp_GetUserAltTitle]
	@userId INT,
	@groupId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Validate input...
	IF (ISNULL(@userId, 0) = 0)
	BEGIN
		--RAISERROR('Invalid parameter: @userId cannot be NULL or zero!', 18, 0)
		RETURN	
	END
	
	IF (ISNULL(@groupId, 0) = 0)
	BEGIN
		--RAISERROR('Invalid parameter: @groupId cannot be NULL or zero!', 18, 0)
		RETURN
	END

	SELECT	t.Title
	FROM	core_UsersAltTitles t
	WHERE	t.UserId = @userId
			AND t.GroupId = @groupId
END
GO

