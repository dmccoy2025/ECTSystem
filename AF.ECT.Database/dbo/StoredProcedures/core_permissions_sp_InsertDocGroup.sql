
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 6/23/2015
-- Description:	Inserts a new permission document group mapping into the 
--				core_PermissionDocGroups table.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_permissions_sp_InsertDocGroup] 
	 @permId		INT
	,@docGroupId	INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @valid BIT = 1
	DECLARE @cnt INT = 0
	
	-- Make sure the input parameters are valid
	IF @permId <= 0 OR @docGroupId < 0
		BEGIN
		SET @valid = 0
		END
		
	SELECT	@cnt = COUNT(permId)
	FROM	core_Permissions
	WHERE	permId = @permId
	
	IF @cnt != 1
		BEGIN
		SET @valid = 0
		END

	-- Check to see if a record with this permission ID already exists
	IF @valid = 1
		BEGIN
		
		SET @cnt = 0
		
		SELECT	@cnt = COUNT(*)
		FROM	core_PermissionDocGroups
		WHERE	permId = @permId
				
		IF @cnt > 0
			BEGIN
			SET @valid = 0
			END
		
		END


	-- If everything is valid then insert the new record
	IF @valid = 1
		BEGIN
		
		INSERT INTO core_PermissionDocGroups (permId, docGroupId)
		VALUES (@permId, @docGroupId)
		
		END
END
GO

