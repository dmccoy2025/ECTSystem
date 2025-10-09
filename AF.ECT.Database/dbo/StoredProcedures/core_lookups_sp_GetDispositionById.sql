
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 1/20/2016
-- Description:	Returns a record from the core_lkupDisposition table which has
--				a matching ID value. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookups_sp_GetDispositionById]
	@id INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@id, 0) <= 0)
	BEGIN
		RETURN
	END
	
	SELECT	d.Id, d.Name
	FROM	core_lkupDisposition d
	WHERE	d.Id = @id
END
GO

