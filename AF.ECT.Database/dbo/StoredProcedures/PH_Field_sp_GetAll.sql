
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 3/3/2016
-- Description:	Gets all records from the PH_Field table. 
-- ============================================================================
CREATE PROCEDURE [dbo].[PH_Field_sp_GetAll]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT	f.Id, f.Name
	FROM	PH_Field f
	ORDER	BY f.Name ASC
END
GO

