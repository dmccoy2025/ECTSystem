
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 3/10/2016
-- Description:	Selects Form Field records associated with the specified
--				section and field Id values. This procedure is meant to be
--				called when you want to get all of the PH_FieldTypes associated
--				with a specific PH_Field in a specific PH_Section.
-- ============================================================================
CREATE PROCEDURE [dbo].[PH_FormField_sp_GetFieldTypes]
	@sectionId INT,
	@fieldId INT
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
	
	SELECT	ff.SectionId, ff.FieldId, ff.FieldTypeId, ff.FieldDisplayOrder, ff.FieldTypeDisplayOrder, ff.ToolTip
	FROM	PH_Form_Field ff
	WHERE	ff.SectionId = @sectionId
			AND ff.FieldId = @fieldId
END
GO

