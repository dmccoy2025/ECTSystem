-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 7/6/2015
-- Description:	Inserts a new record into the core_KeyVal_Value table.  
-- ============================================================================
CREATE PROCEDURE [dbo].[core_KeyVal_sp_InsertKeyValue]
	 @keyId INT
	,@valueDescription VARCHAR(50)
	,@value VARCHAR(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @valid INT = 1
	DECLARE @count INT = 0

	-- Check if a record with the specified new keyId actually exists...
	SELECT	@count = COUNT(Id)
	FROM	core_KeyVal_Key
	WHERE	ID = @keyId
	
	IF @count <> 1
		BEGIN
		SET @valid = 0
		END
		
	-- Check if a record with these new values already exists...
	SET @count = 0
	
	SELECT	@count = COUNT(v.Id)
	FROM	core_KeyVal_Value v
	WHERE	v.Key_Id = @keyId
			AND v.Value = @value
			AND v.Value_Description = @valueDescription
	
	IF @count > 0
		BEGIN
		SET @valid = 0
		END
	
	IF @valid = 1
		BEGIN
		
		DECLARE @valueId INT = 0
		
		-- Determine the value for the Value_Id field...
		SELECT	TOP 1 @valueId = Value_Id
		FROM	core_KeyVal_Value v
		WHERE	v.Key_Id = @keyId
		ORDER	BY v.Value_Id DESC
		
		SET @valueId = @valueId + 1
		
		-- Insert the new record...
		INSERT INTO core_KeyVal_Value ([Key_Id], [Value_Id], [Value], [Value_Description])
		VALUES (@keyId, @valueId, @value, @valueDescription)
				
		END
END
GO

