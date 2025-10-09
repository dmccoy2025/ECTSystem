
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 12/21/2016
-- Description:	Gets a record from the core_lkupDbSignTemplates table by the 
--				record's Id. 
-- ============================================================================
CREATE PROCEDURE [dbo].[DBSign_sp_GetTemplateById]
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
	
	SELECT	*
	FROM	core_lkupDbSignTemplates t
	WHERE	t.t_id = @id
END
GO

