
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 3/11/2016
-- Description:	Updates an existing PH Form Field record in the PH_Form_Field 
--				table.
-- ============================================================================
CREATE PROCEDURE [dbo].[PH_FormField_sp_Update]
	@oldSectionId INT,
	@oldFieldId INT,
	@oldFieldTypeId INT,
	@newSectionId INT,
	@newFieldId INT,
	@newFieldTypeId INT,
	@oldFieldDisplayOrder INT,
	@oldFieldTypeDisplayOrder INT,
	@newFieldDisplayOrder INT,
	@newFieldTypeDisplayOrder INT,
	@oldToolTip NVARCHAR(100),
	@newToolTip NVARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@oldSectionId, 0) = 0)
	BEGIN
		RETURN
	END
	
	IF (ISNULL(@oldFieldId, 0) = 0)
	BEGIN
		RETURN
	END
	
	IF (ISNULL(@oldFieldTypeId, 0) = 0)
	BEGIN
		RETURN
	END
	
	IF (ISNULL(@newSectionId, 0) = 0)
	BEGIN
		RETURN
	END
	
	IF (ISNULL(@newFieldId, 0) = 0)
	BEGIN
		RETURN
	END
	
	IF (ISNULL(@newFieldTypeId, 0) = 0)
	BEGIN
		RETURN
	END
	
	IF (@newToolTip = '')
	BEGIN
		SET @newToolTip = NULL
	END
	
	DECLARE @count INT = 0
	
	-- Make sure the PH Field being updated actually exists...
	SELECT	@count = COUNT(*)
	FROM	PH_Form_Field ff
	WHERE	ff.SectionId = @oldSectionId
			AND ff.FieldId = @oldFieldId
			AND ff.FieldTypeId = @oldFieldTypeId
	
	IF (@count <> 1)
	BEGIN
		RETURN
	END
	
	DECLARE @error INT = 0
	
	-- Check if at least one of the ID values was modified...
	IF (@oldSectionId <> @newSectionId OR
		@oldFieldId <> @newFieldId OR
		@oldFieldTypeId <> @newFieldTypeId)
	BEGIN
		-- Make sure a PH Form Field with these values doesn't already exist...
		SET @count = 0
		
		SELECT	@count = COUNT(*)
		FROM	PH_Form_Field ff
		WHERE	ff.SectionId = @newSectionId
				AND ff.FieldId = @newFieldId
				AND ff.FieldTypeId = @newFieldTypeId
				
		IF (@count <> 0)
		BEGIN
			RETURN
		END
	
		-- First, delete the existing Form Field record...PH_FormField_sp_Delete takes care of handling the DisplayOrder updates...
		EXEC @error = PH_FormField_sp_Delete @oldSectionId, @oldFieldId, @oldFieldTypeId
		
		IF (@error > 0)
		BEGIN
			RETURN
		END
		
		-- Next, insert new Form Field record with the modified values...PH_FormField_sp_Insert takes care of handling the DisplayOrder updates...
		EXEC @error = PH_FormField_sp_Insert @newSectionId, @newFieldId, @newFieldTypeId, @newFieldDisplayOrder, @newFieldTypeDisplayOrder, @newToolTip
	END
	ELSE
	BEGIN
		-- Update ToolTip if necessary...
		IF (@oldToolTip <> @newToolTip)
		BEGIN
			UPDATE	PH_Form_Field
			SET		ToolTip = @newToolTip
			WHERE	SectionId = @oldSectionId
					AND FieldId = @oldFieldId
					AND FieldTypeId = @oldFieldTypeId
		END
		
		-- Update Field display orders if necessary...
		IF (@oldFieldDisplayOrder <> @newFieldDisplayOrder)
		BEGIN
			UPDATE	PH_Form_Field
			SET		FieldDisplayOrder = @newFieldDisplayOrder
			WHERE	SectionId = @oldSectionId
					AND FieldId = @oldFieldId
					AND FieldTypeId = @oldFieldTypeId
			
			EXEC @error = PH_FormField_sp_UpdateFieldDisplayOrders @oldSectionId, @oldFieldId, @oldFieldDisplayOrder, @newFieldDisplayOrder, 0
			
			IF (@error > 0)
			BEGIN
				RETURN
			END
		END
		
		-- Update Field Type display orders if necessary...
		IF (@oldFieldTypeDisplayOrder <> @newFieldTypeDisplayOrder)
		BEGIN
			UPDATE	PH_Form_Field
			SET		FieldTypeDisplayOrder = @newFieldTypeDisplayOrder
			WHERE	SectionId = @oldSectionId
					AND FieldId = @oldFieldId
					AND FieldTypeId = @oldFieldTypeId
			
			EXEC @error = PH_FormField_sp_UpdateFieldTypeDisplayOrders @oldSectionId, @oldFieldId, @oldFieldTypeId, @oldFieldTypeDisplayOrder, @newFieldTypeDisplayOrder, 0
			
			IF (@error > 0)
			BEGIN
				RETURN
			END
		END
	END
END
GO

