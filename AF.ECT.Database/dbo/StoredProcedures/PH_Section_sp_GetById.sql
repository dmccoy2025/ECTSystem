
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 2/26/2016
-- Description:	Gets a record from the PH_Section table by the record's Id. 
-- ============================================================================
CREATE PROCEDURE [dbo].[PH_Section_sp_GetById]
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
	
	SELECT	s.Id, s.Name, s.ParentId, s.FieldColumns, s.IsTopLevel, s.DisplayOrder, s.PageBreak
	FROM	PH_Section s
	WHERE	s.Id = @id
END
GO

