-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 7/2/2015
-- Description:	Updates the fields in the core_KeyVal_Value table for a value
--				specified by an Id. The Id is differnet then the ValueId of a
--				key value. Id is an auto increment identity value whereas
--				ValueId is a programatically controled value. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_KeyVal_sp_UpdateKeyValueById]
	 @id INT
	,@newKeyId INT
	,@newValueDescription VARCHAR(50)
	,@newValue VARCHAR(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @valid INT = 1
	DECLARE @count INT = 0

	-- Check if a record with the specified Id actually exists...
	SELECT	@count = COUNT(Id)
	FROM	core_KeyVal_Value
	WHERE	Id = @id
	
	IF @count <> 1
		BEGIN
		SET @valid = 0
		END
	
	-- Check if a record with the specified new keyId actually exists...
	SET @count = 0
	
	SELECT	@count = COUNT(Id)
	FROM	core_KeyVal_Key
	WHERE	ID = @newKeyId
	
	IF @count <> 1
		BEGIN
		SET @valid = 0
		END
		
	-- Check if a record with these new values already exists...
	SET @count = 0
	
	SELECT	@count = COUNT(v.Id)
	FROM	core_KeyVal_Value v
	WHERE	v.Key_Id = @newKeyId
			AND v.Value = @newValue
			AND v.Value_Description = @newValueDescription
	
	IF @count > 0
		BEGIN
		SET @valid = 0
		END
	
	IF @valid = 1
		BEGIN
		
		DECLARE @originalKeyId INT = 0
		DECLARE @newValueId INT = 0
		
		SELECT	@originalKeyId = Key_Id, @newValueId = Value_Id
		FROM	core_KeyVal_Value
		WHERE	Id = @id
		
		-- If the key Id is being changed then determine the new value for the Value_Id field...
		IF @newKeyId <> @originalKeyId
			BEGIN
		
			SELECT	TOP 1 @newValueId = Value_Id
			FROM	core_KeyVal_Value v
			WHERE	v.Key_Id = @newKeyId
			ORDER BY v.Value_Id DESC
			
			SET @newValueId = @newValueId + 1
		
			END
		
		-- Update the record with the new data...
		UPDATE	core_KeyVal_Value
		SET		Key_Id = @newKeyId,
				Value_Id = @newValueId,
				Value = @newValue,
				Value_Description = @newValueDescription
		WHERE	Id = @id
				
		END
END
GO

