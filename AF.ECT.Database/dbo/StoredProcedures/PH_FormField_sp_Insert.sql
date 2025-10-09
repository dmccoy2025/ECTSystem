
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 3/10/2016
-- Description:	Inserts a new PH Form Field record into the PH_Form_Field 
--				table.
-- ============================================================================
CREATE PROCEDURE [dbo].[PH_FormField_sp_Insert]
	@sectionId INT,
	@fieldId INT,
	@fieldTypeId INT,
	@fieldDisplayOrder INT,
	@fieldTypeDisplayOrder INT,
	@toolTip NVARCHAR(100)
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
	
	IF (@fieldDisplayOrder IS NULL)
	BEGIN
		SET @fieldDisplayOrder = 1
	END
	
	IF (@fieldTypeDisplayOrder IS NULL)
	BEGIN
		SET @fieldTypeDisplayOrder = 1
	END
	
	IF (@toolTip = '')
	BEGIN
		SET @toolTip = NULL
	END
	
	DECLARE @count INT = 0
	
	-- Make sure a PH Form Field with these values doesn't already exist...
	SELECT	@count = COUNT(*)
	FROM	PH_Form_Field ff
	WHERE	ff.SectionId = @sectionId
			AND ff.FieldId = @fieldId
			AND ff.FieldTypeId = @fieldTypeId
	
	IF (@count <> 0)
	BEGIN
		RETURN
	END
	
	BEGIN TRANSACTION

		INSERT	INTO	PH_Form_Field ([SectionId], [FieldId], [FieldTypeId], [FieldDisplayOrder], [FieldTypeDisplayOrder], [ToolTip])
				VALUES (@sectionId, @fieldId, @fieldTypeId, @fieldDisplayOrder, @fieldTypeDisplayOrder, @toolTip)
			
		IF (@@ERROR <> 0)
		BEGIN
			ROLLBACK TRANSACTION
			RETURN
		END
		
		DECLARE @error INT = 0
		
		-- Update the display order for the other Form Fields...
		EXEC @error = PH_FormField_sp_UpdateFieldDisplayOrders @sectionId, @fieldId, @fieldDisplayOrder, @fieldDisplayOrder, 1
		
		IF (@error > 0)
		BEGIN
			ROLLBACK TRANSACTION
			RETURN
		END
		
		EXEC @error = PH_FormField_sp_UpdateFieldTypeDisplayOrders @sectionId, @fieldId, @fieldTypeId, @fieldTypeDisplayOrder, @fieldTypeDisplayOrder, 1
		
		IF (@error > 0)
		BEGIN
			ROLLBACK TRANSACTION
			RETURN
		END
		
	COMMIT TRANSACTION
END
GO

