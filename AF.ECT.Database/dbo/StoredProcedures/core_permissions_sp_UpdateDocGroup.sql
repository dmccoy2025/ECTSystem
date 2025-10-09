
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 6/23/2015
-- Description:	Updates an existing permission document group mapping in the 
--				core_PermissionDocGroups table.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_permissions_sp_UpdateDocGroup]
	 @oldPermId		INT
	,@oldDocGroupId	INT
	,@newPermId		INT
	,@newDocGroupId	INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @valid BIT = 1
	DECLARE @cnt INT = 0
	
	-- Make sure we can find a record with the old values
	SELECT	@cnt = COUNT(permId)
	FROM	core_PermissionDocGroups
	WHERE	permId = @oldPermId
			AND docGroupId = @oldDocGroupId
			
	IF @cnt != 1
		BEGIN
		SET @valid = 0
		END

	
	-- Make sure the new values are valid
	IF @valid = 1
		BEGIN
		
		SET @cnt = 0
		
		IF @newPermId <= 0 OR @newDocGroupId < 0
			BEGIN
			SET @valid = 0
			END
			
		SELECT	@cnt = COUNT(permId)
		FROM	core_Permissions
		WHERE	permId = @newPermId
		
		IF @cnt != 1
			BEGIN
			SET @valid = 0
			END

		END
		
	-- Check to see if a record with these input parameters already exists
	IF @valid = 1
		BEGIN
		
		SET @cnt = 0
	
		SELECT	@cnt = COUNT(*)
		FROM	core_PermissionDocGroups
		WHERE	permId = @newPermId
				AND docGroupId = @newDocGroupId
				
		IF @cnt > 0
			BEGIN
			SET @valid = 0
			END
			
		SET @cnt = 0
		
		-- Check to see if a record with the new permId already exists
		IF @oldPermId <> @newPermId
			BEGIN
			
			SELECT	@cnt = COUNT(*)
			FROM	core_PermissionDocGroups
			WHERE	permId = @newPermId
			
			IF @cnt > 0
				BEGIN
				SET @valid = 0
				END
			
			END
		
		END
	
	
	-- If everything is valid then update the record
	IF @valid = 1
		BEGIN
		
		UPDATE	core_PermissionDocGroups
		SET		permId = @newPermId,
				docGroupId = @newDocGroupId
		WHERE	permId = @oldPermId
				AND docGroupId = @oldDocGroupId
		
		END
END
GO

