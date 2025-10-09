
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 2/26/2016
-- Description:	Gets a record from the PH_Field table by the record's Id. 
-- ============================================================================
CREATE PROCEDURE [dbo].[PH_Field_sp_GetById]
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
	
	SELECT	f.Id, f.Name
	FROM	PH_Field f
	WHERE	f.Id = @id
END
GO

