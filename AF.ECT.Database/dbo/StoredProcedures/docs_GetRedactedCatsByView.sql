-- ============================================================================
-- Author:		Kenneth Barnett
-- Create date: 12/3/2016
-- Description:	Returns at DocumentCategory2 records mapped to the specified
--				DocumentView id and are marked as Redacted
-- ============================================================================
CREATE PROCEDURE [dbo].[docs_GetRedactedCatsByView]
	@viewId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT	dc.DocCatId, dc.CategoryDescription
	FROM	DocCategoryView dcv
			JOIN DocumentCategory2 dc ON dcv.DocCatId = dc.DocCatId
	WHERE	dcv.DocViewId = @viewId
			AND dcv.IsRedacted = 1
	ORDER	BY dcv.ViewOrder
END
GO

