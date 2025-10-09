
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 11/11/2015
-- Description:	Updates the alternate title for a user which matches the user
--				Id and group Id passed into the stored procedure. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_user_sp_UpdateUserAltTitle]
	@userId INT,
	@groupId INT,
	@newTitle NVARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- Validate input...
	IF (ISNULL(@userId, 0) = 0)
	BEGIN
		RETURN	
	END
	
	IF (ISNULL(@groupId, 0) = 0)
	BEGIN
		RETURN
	END
	
	IF (@newTitle IS NULL)
	BEGIN
		SET @newTitle = ''
	END
	
	DECLARE @count INT = 0
	
	-- Check if record exists...
	SELECT	@count = COUNT(*)
	FROM	core_UsersAltTitles
	WHERE	UserId = @userId
			AND GroupId = @groupId

	-- Update existing record...
	IF (@count = 1)
	BEGIN
		UPDATE	core_UsersAltTitles
		SET		Title = @newTitle
		WHERE	UserId = @userId
				AND GroupId = @groupId
	END
	
	-- Insert new record if one doesn't already exist...
	IF (@count = 0)
	BEGIN
		INSERT	INTO core_UsersAltTitles ([UserId], [GroupId], [Title])
		VALUES	(@userId, @groupId, @newTitle)
	END
END
GO

