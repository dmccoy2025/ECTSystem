
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 3/11/2016
-- Description:	Updates the PH Form Field display order values for the PH Field 
--				associated with the specified Section and Field Id 
--				values. 
--				@filter: 0 = edit; 1 = insert; 2 = delete
--				DO = Display Order
-- ============================================================================
CREATE PROCEDURE [dbo].[PH_FormField_sp_UpdateFieldDisplayOrders]
	@sectionId INT,
	@fieldId INT,
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
			-- If the Field DO was changed for one Field of a specific section, then update all of the fields for that
			-- section with the same new DO...
			IF (@oldOrder <> @newOrder)
			BEGIN
				UPDATE	PH_Form_Field
				SET		FieldDisplayOrder = @newOrder
				WHERE	SectionId = @sectionId
						AND FieldId = @fieldId
			
				IF (@@ERROR <> 0)
				BEGIN
					ROLLBACK TRANSACTION
					RETURN 1
				END
			END
			
			-- If the Field DO increased, then decrement the DO of records who have an DO between the old and new DOs (inclusive)...
			IF (@newOrder > @oldOrder)
			BEGIN
				UPDATE	PH_Form_Field
				SET		FieldDisplayOrder = FieldDisplayOrder - 1
				WHERE	SectionId = @sectionId
						AND FieldId <> @fieldId
						AND FieldDisplayOrder <= @newOrder
						AND FieldDisplayOrder >= @oldOrder
			END
			
			-- If the Field DO decreased, then increment the DO of records who have an DO between the old and new DOs (inclusive)...
			IF (@newOrder < @oldOrder)
			BEGIN
				UPDATE	PH_Form_Field
				SET		FieldDisplayOrder = FieldDisplayOrder + 1
				WHERE	SectionId = @sectionId
						AND FieldId <> @fieldId
						AND FieldDisplayOrder >= @newOrder
						AND FieldDisplayOrder <= @oldOrder
			END
		END
		
		-- A new Form Field record was inserted...
		ELSE IF (@filter = 1)
		BEGIN
			-- Find Form Fields with the same sectionId and fieldId and update their DO to match the new records DO...
			UPDATE	PH_Form_Field
			SET		FieldDisplayOrder = @newOrder
			WHERE	SectionId = @sectionId
					AND FieldId = @fieldId
			
			IF (@@ERROR <> 0)
			BEGIN
				ROLLBACK TRANSACTION
				RETURN 1
			END
			
			DECLARE @duplicates INT = 0

			-- Check if there are existing Form Field records with the same DO...
			SELECT	@duplicates = COUNT(*)
			FROM	PH_Form_Field ff
			WHERE	ff.SectionId = @sectionId
					AND ff.FieldId <> @fieldId
					AND ff.FieldDisplayOrder = @newOrder
			
			IF (@duplicates > 1)
			BEGIN
				-- Increase the Field DO for all records which have a DO >= the new Form Field's Field DO...
				UPDATE	PH_Form_Field
				SET		FieldDisplayOrder = FieldDisplayOrder + 1
				WHERE	SectionId = @sectionId
						AND FieldId <> @fieldId
						AND FieldDisplayOrder >= @newOrder
			END
		END
		
		-- An existing Form Field record was deleted...
		ELSE IF (@filter = 2)
		BEGIN
			-- If there are no more of the same filed Id with the section Id then update the DO for other fields associated with section
			DECLARE @remainder INT = 0
			
			SELECT	@remainder = COUNT(*)
			FROM	PH_Form_Field ff
			WHERE	ff.SectionId = @sectionId
					AND ff.FieldId = @fieldId
			
			IF (@remainder = 0)
			BEGIN
				-- Decrease the Field DO by one for records which had a DO greater than the DO of the record which was deleted...
				UPDATE	PH_Form_Field
				SET		FieldDisplayOrder = FieldDisplayOrder - 1
				WHERE	SectionId = @sectionId
						AND FieldDisplayOrder > @oldOrder
			END
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

