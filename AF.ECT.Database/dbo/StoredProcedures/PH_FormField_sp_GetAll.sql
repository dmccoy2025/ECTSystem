
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 3/10/2016
-- Description:	Gets all records from the PH_Form_Field table. 
-- ============================================================================
CREATE PROCEDURE [dbo].[PH_FormField_sp_GetAll]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT	ff.SectionId, ff.FieldId, ff.FieldTypeId, ff.FieldDisplayOrder, ff.FieldTypeDisplayOrder, ff.ToolTip
	FROM	PH_Form_Field ff
	ORDER	BY ff.FieldDisplayOrder ASC, ff.FieldTypeDisplayOrder ASC
END
GO

