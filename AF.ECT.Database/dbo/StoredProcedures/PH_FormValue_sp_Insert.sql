
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 3/15/2016
-- Description:	Inserts a new PH Form Value record.
-- ============================================================================
CREATE PROCEDURE [dbo].[PH_FormValue_sp_Insert]
	@refId INT,
	@sectionId INT,
	@fieldId INT,
	@fieldTypeId INT,
	@value NVARCHAR(500)
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
	
	IF (@value IS NULL)
	BEGIN
		SET @value = ''
	END
	
	DECLARE @count INT = 0
	
	-- Makes sure this record doesn't already exist...
	SELECT	@count = COUNT(*)
	FROM	PH_Form_Value fv
	WHERE	fv.RefId = @refId
			AND fv.SectionId = @sectionId
			AND fv.FieldId = @fieldId
			AND fv.FieldTypeId = @fieldTypeId
	
	IF (@count <> 0)
	BEGIN
		SELECT 0
		RETURN
	END
	
	BEGIN TRANSACTION
		
		INSERT	INTO	PH_Form_Value ([RefId], [SectionId], [FieldId], [FieldTypeId], [Value])
				VALUES	(@refId, @sectionId, @fieldId, @fieldTypeId, @value)
	
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

