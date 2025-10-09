
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 3/11/2016
-- Description:	Updates the PH Form Field display order values for the PH Field 
--				Types associated with the specified Section and Field Id 
--				values. 
--				@filter: 0 = edit; 1 = insert; 2 = delete
--				DO = Display Order
-- ============================================================================
CREATE PROCEDURE [dbo].[PH_FormField_sp_UpdateFieldTypeDisplayOrders]
	@sectionId INT,
	@fieldId INT,
	@fieldTypeId INT,
	@oldOrder INT,
	@newOrder INT,
	@filter TINYINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- VALIDATE INPUT PARAMETERS --
	IF (ISNULL(@sectionId, 0) = 0)
	BEGIN
		RETURN 1
	END
	
	IF (ISNULL(@fieldId, 0) = 0)
	BEGIN
		RETURN 1
	END
	
	IF (ISNULL(@fieldTypeId, 0) = 0)
	BEGIN
		RETURN 1
	END
	
	IF (@filter IS NULL)
	BEGIN
		RETURN 1
	END
	
	IF (@oldOrder IS NULL)
	BEGIN
		SET @oldOrder = 0
	END
	
	IF (@newOrder IS NULL)
	BEGIN
		SET @newOrder = 0
	END

	BEGIN TRANSACTION
	
		-- An existing Form Field record was edited....
		IF (@filter = 0)
		BEGIN
			-- If the Field Type DO increased, then decrement the DO of records who have an DO between the old and new DOs (inclusive)...
			IF (@newOrder > @oldOrder)
			BEGIN
				UPDATE	PH_Form_Field
				SET		FieldTypeDisplayOrder = FieldTypeDisplayOrder - 1
				WHERE	SectionId = @sectionId
						AND FieldId = @fieldId
						AND FieldTypeId <> @fieldTypeId
						AND FieldTypeDisplayOrder <= @newOrder
						AND FieldTypeDisplayOrder >= @oldOrder
			END
			
			-- If the Field Type DO decreased, then increment the DO of records who have an DO between the old and new DOs (inclusive)...
			IF (@newOrder < @oldOrder)
			BEGIN
				UPDATE	PH_Form_Field
				SET		FieldTypeDisplayOrder = FieldTypeDisplayOrder + 1
				WHERE	SectionId = @sectionId
						AND FieldId = @fieldId
						AND FieldTypeId <> @fieldTypeId
						AND FieldTypeDisplayOrder >= @newOrder
						AND FieldTypeDisplayOrder <= @oldOrder
			END
		END
		
		-- A new Form Field record was inserted...
		ELSE IF (@filter = 1)
		BEGIN
			DECLARE @duplicates INT = 0
			
			-- Check if there are existing Form Field records with the same DO...
			SELECT	@duplicates = COUNT(*)
			FROM	PH_Form_Field ff
			WHERE	ff.SectionId = @sectionId
					AND ff.FieldId = @fieldId
					AND ff.FieldTypeDisplayOrder = @newOrder
			
			IF (@duplicates > 1)
			BEGIN
				-- Increase the Field Type DO for all records which have a DO >= the new Form Field's Field Type DO...
				UPDATE	PH_Form_Field
				SET		FieldTypeDisplayOrder = FieldTypeDisplayOrder + 1
				WHERE	SectionId = @sectionId
						AND FieldId = @fieldId
						AND FieldTypeId <> @fieldTypeId
						AND FieldTypeDisplayOrder >= @newOrder
			END
		END
		
		-- An existing Form Field record was deleted...
		ELSE IF (@filter = 2)
		BEGIN
			-- Decrease the Field Type DO by one for records which had a DO greater than the DO of the record which was deleted...
			UPDATE	PH_Form_Field
			SET		FieldTypeDisplayOrder = FieldTypeDisplayOrder - 1
			WHERE	SectionId = @sectionId
					AND FieldId = @fieldId
					AND FieldTypeDisplayOrder > @oldOrder
		END
		
		IF (@@ERROR <> 0)
		BEGIN
			ROLLBACK TRANSACTION
			RETURN 1
		END
		
	COMMIT TRANSACTION
	
	RETURN 0
END
GO

