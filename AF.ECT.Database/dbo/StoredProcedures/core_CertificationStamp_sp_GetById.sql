
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 1/22/2016
-- Description:	Returns a record from the core_CertificationStamp table which
--				has a matching ID value. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_CertificationStamp_sp_GetById]
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
	
	SELECT	cs.Id, cs.Name, cs.Body, cs.IsQualified
	FROM	core_CertificationStamp cs
	WHERE	cs.Id = @id
END
GO

