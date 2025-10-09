
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 2/26/2016
-- Description:	Gets all of the PH Section records which are direct children to
--				the specified PH Section Id value.  
-- ============================================================================
CREATE PROCEDURE [dbo].[PH_Section_sp_GetChildren]
	@parentId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@parentId, 0) = 0)
	BEGIN
		RETURN
	END
	
	SELECT	s.Id, s.Name, s.ParentId, s.FieldColumns, s.IsTopLevel, s.DisplayOrder, s.PageBreak
	FROM	PH_Section s
	WHERE	s.ParentId = @parentId
	ORDER	BY s.DisplayOrder ASC
END
GO

