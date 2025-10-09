
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 3/10/2016
-- Description:	Gets a record from the PH_Form_Field table by the record's 
--				section Id, field Id, and field Id values. 
-- ============================================================================
CREATE PROCEDURE [dbo].[PH_FormField_sp_GetByIds]
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
	
	SELECT	ff.SectionId, ff.FieldId, ff.FieldTypeId, ff.FieldDisplayOrder, ff.FieldTypeDisplayOrder, ff.ToolTip
	FROM	PH_Form_Field ff
	WHERE	ff.SectionId = @sectionId
			AND ff.FieldId = @fieldId
			AND ff.FieldTypeId = @fieldTypeId
END
GO

