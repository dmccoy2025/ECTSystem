
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 2/2/2016
-- Description:	Returns a record from the core_CompletedByGroup table which
--				has a matching ID value. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_CompletedByGroup_sp_GetById]
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
	
	SELECT	cbg.Id, cbg.Name
	FROM	core_CompletedByGroup cbg
	WHERE	cbg.Id = @id
END
GO

