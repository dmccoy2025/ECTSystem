
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 3/9/2016
-- Description:	Gets a specific record from the core_lkupDataType table by its
--				Id value.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookups_sp_GetDataTypeById]
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
	
	SELECT	dt.Name
	FROM	core_lkupDataType dt
	WHERE	dt.Id = @id
END
GO

