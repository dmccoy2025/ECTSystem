
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 3/9/2016
-- Description:	Gets a record from the PH_FieldType table by the record's Id. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	9/11/2017
-- Description:		Modified to select new Length field.
-- ============================================================================
CREATE PROCEDURE [dbo].[PH_FieldType_sp_GetById]
	@id INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@id, 0) = 0)
	BEGIN
		RETURN
	END
	
	SELECT	ft.Id, ft.Name, ft.DataTypeId, ft.Datasource, ft.Placeholder, ft.Color, ft.Length
	FROM	PH_FieldType ft
	WHERE	ft.Id = @id
END
GO

