
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 2/29/2016
-- Description:	Gets all records from the PH_Section table. 
-- ============================================================================
CREATE PROCEDURE [dbo].[PH_Section_sp_GetAll]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT	s.Id, s.Name, s.ParentId, s.FieldColumns, s.IsTopLevel, s.DisplayOrder, s.PageBreak
	FROM	PH_Section s
END
GO

