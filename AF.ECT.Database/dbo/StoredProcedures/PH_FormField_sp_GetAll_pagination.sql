-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 3/10/2016
-- Description:	Gets all records from the PH_Form_Field table with pagination.
-- ============================================================================
CREATE PROCEDURE [dbo].[PH_FormField_sp_GetAll_pagination]
	@PageNumber INT = 1,
	@PageSize INT = 10
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT	ff.SectionId, ff.FieldId, ff.FieldTypeId, ff.FieldDisplayOrder, ff.FieldTypeDisplayOrder, ff.ToolTip
	FROM	PH_Form_Field ff
	ORDER	BY ff.FieldDisplayOrder ASC, ff.FieldTypeDisplayOrder ASC
	OFFSET (@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
END
GO