-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 8/15/2015
-- Description:	Updates a ICD code with new values.  
-- ============================================================================
CREATE PROCEDURE [dbo].[core_ICD_sp_UpdateCode]
	 @codeId INT
	,@newValue NVARCHAR(50)
	,@newText NVARCHAR(200)
	,@newDisease BIT
	,@newActive BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @valid INT = 1
	DECLARE @count INT = 0

	-- Check if a record with the specified Id actually exists...
	SELECT	@count = COUNT(ICD9_ID)
	FROM	core_lkupICD9
	WHERE	ICD9_ID = @codeId
	
	IF @count <> 1
		BEGIN
		SET @valid = 0
		END
		
	SET @count = 0
	
	-- Check if there already exists a record with the new code...
	SELECT	@count = COUNT(ICD9_ID)
	FROM	core_lkupICD9
	WHERE	value = @newValue
			AND ICD9_ID <> @codeId
	
	IF @count > 0 AND @newValue <> ''
		BEGIN
		SET @valid = 0
		END
		
	-- Check if the new text is valid...
	IF @newText IS NULL OR @newText = ''
		BEGIN
		SET @valid = 0
		END
	
	-- If everything valid then update the record...
	IF @valid = 1
		BEGIN
		
		IF @newValue = ''
			BEGIN
			SET @newValue = NULL
			END
		
		UPDATE	core_lkupICD9
		SET		value = @newValue, text = @newText, isDisease = @newDisease, Active = @newActive
		WHERE	ICD9_ID = @codeId
				
		END
END
GO

