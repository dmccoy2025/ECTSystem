
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 3/17/2016
-- Description:	Gets a record from the core_lkupRMUs table by the record's Id. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookups_sp_GetRMUById]
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
	
	SELECT	r.Id, r.RMU, r.cs_id
	FROM	core_lkupRMUs r
	WHERE	r.Id = @id
END
GO

