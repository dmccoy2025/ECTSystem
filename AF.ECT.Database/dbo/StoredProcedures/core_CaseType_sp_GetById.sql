
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 1/25/2016
-- Description:	Returns a record from the core_CaseType table which has a
--				matching ID value. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_CaseType_sp_GetById]
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
	
	SELECT	ct.Id, ct.Name
	FROM	core_CaseType ct
	WHERE	ct.Id = @id
END
GO

