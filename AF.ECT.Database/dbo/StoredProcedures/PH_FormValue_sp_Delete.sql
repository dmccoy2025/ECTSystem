
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 4/12/2016
-- Description:	Deletes a PH Form Value record.
-- ============================================================================
CREATE PROCEDURE [dbo].[PH_FormValue_sp_Delete]
	@refId INT,
	@sectionId INT,
	@fieldId INT,
	@fieldTypeId INT
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
	
	BEGIN TRANSACTION
		
		DELETE	FROM	PH_Form_Value
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

