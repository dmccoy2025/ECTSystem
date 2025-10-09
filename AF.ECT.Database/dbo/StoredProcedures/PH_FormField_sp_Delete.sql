
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 3/10/2016
-- Description:	Delete a PH Form Field record from the PH_Form_Field table.
-- ============================================================================
CREATE PROCEDURE [dbo].[PH_FormField_sp_Delete]
	@sectionId INT,
	@fieldId INT,
	@fieldTypeId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@sectionId, 0) = 0)
	BEGIN
		RETURN
	END
	
	IF (ISNULL(@fieldId, 0) = 0)
	BEGIN
		RETURN
	END
	
	IF (ISNULL(@fieldTypeId, 0) = 0)
	BEGIN
		RETURN
	END
	
	DECLARE @count INT = 0,
			@fieldTypeDisplayOrder INT = 0,
			@fieldDisplayOrder INT = 0
		
	-- Check if the record to delete actually exists...
	SELECT	@count = COUNT(*)
	FROM	PH_Form_Field ff
	WHERE	ff.SectionId = @sectionId
			AND ff.FieldId = @fieldId
			AND ff.FieldTypeId = @fieldTypeId
			
	IF (@count = 0)
	BEGIN
		RETURN
	END
	
	-- Get display orders for the record...
	SELECT	@fieldDisplayOrder = ff.FieldDisplayOrder,
			@fieldTypeDisplayOrder = ff.FieldTypeDisplayOrder
	FROM	PH_Form_Field ff
	WHERE	ff.SectionId = @sectionId
			AND ff.FieldId = @fieldId
			AND ff.FieldTypeId = @fieldTypeId
	
	BEGIN TRANSACTION
	
		DELETE	FROM	PH_Form_Field
				WHERE	SectionId = @sectionId
						AND FieldId = @fieldId
						AND FieldTypeId = @fieldTypeId

		IF (@@ERROR <> 0)
		BEGIN
			ROLLBACK TRANSACTION
			RETURN
		END
		
		DECLARE @error INT = 0
		
		-- Update the display order for the other Form Fields...
		EXEC @error = PH_FormField_sp_UpdateFieldDisplayOrders @sectionId, @fieldId, @fieldDisplayOrder, @fieldDisplayOrder, 2
		
		IF (@error > 0)
		BEGIN
			ROLLBACK TRANSACTION
			RETURN
		END
		
		EXEC @error = PH_FormField_sp_UpdateFieldTypeDisplayOrders @sectionId, @fieldId, @fieldTypeId, @fieldTypeDisplayOrder, @fieldTypeDisplayOrder, 2
		
		IF (@error > 0)
		BEGIN
			ROLLBACK TRANSACTION
			RETURN
		END
		
	COMMIT TRANSACTION
END
GO

