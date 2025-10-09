
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 3/15/2016
-- Description:	Gets all of the PH Form Value records associated with a
--				specific reference Id value.
-- ============================================================================
CREATE PROCEDURE [dbo].[PH_FormValue_sp_GetByRefId]
	@refId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@refId, 0) = 0)
	BEGIN
		RETURN
	END
	
	SELECT	fv.RefId, fv.SectionId, fv.FieldId, fv.FieldTypeId, fv.Value
	FROM	PH_Form_Value fv
	WHERE	fv.RefId = @refId
END
GO

