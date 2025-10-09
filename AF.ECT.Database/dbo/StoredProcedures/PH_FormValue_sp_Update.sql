
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 3/15/2016
-- Description:	Updates the value for the specified PH Form Value.
-- ============================================================================
CREATE PROCEDURE [dbo].[PH_FormValue_sp_Update]
	@refId INT,
	@sectionId INT,
	@fieldId INT,
	@fieldTypeId INT,
	@newValue NVARCHAR(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@refId, 0) = 0)
	BEGIN
		SELECT 0
		RETURN
	END
	
	IF (ISNULL(@sectionId, 0) = 0)
	BEGIN
		SELECT 0
		RETURN
	END
	
	IF (ISNULL(@fieldId, 0) = 0)
	BEGIN
		SELECT 0
		RETURN
	END
	
	IF (ISNULL(@fieldTypeId, 0) = 0)
	BEGIN
		SELECT 0
		RETURN
	END
	
	IF (@newValue IS NULL)
	BEGIN
		SET @newValue = ''
	END
	
	DECLARE @count INT = 0
	
	-- Makes sure this record exist...
	SELECT	@count = COUNT(*)
	FROM	PH_Form_Value fv
	WHERE	fv.RefId = @refId
			AND fv.SectionId = @sectionId
			AND fv.FieldId = @fieldId
			AND fv.FieldTypeId = @fieldTypeId
	
	IF (@count <> 1)
	BEGIN
		SELECT 0
		RETURN
	END
	
	BEGIN TRANSACTION
		
		UPDATE	PH_Form_Value
		SET		Value = @newValue
		WHERE	RefId = @refId
				AND SectionId = @sectionId
				AND FieldId = @fieldId
				AND FieldTypeId = @fieldTypeId
	
		IF (@@ERROR <> 0)
		BEGIN
			ROLLBACK TRANSACTION
			SELECT 0
			RETURN
		END
		
	COMMIT TRANSACTION
	
	SELECT 1
END
GO

