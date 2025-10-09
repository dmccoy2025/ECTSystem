
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 3/9/2016
-- Description:	Gets all records from the PH_FieldType table. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	9/11/2017
-- Description:		Modified to select new Length field.
-- ============================================================================
CREATE PROCEDURE [dbo].[PH_FieldType_sp_GetAll]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT	ft.Id, ft.Name, ft.DataTypeId, ft.Datasource, ft.Placeholder, ft.Color, ft.Length
	FROM	PH_FieldType ft
END
GO

