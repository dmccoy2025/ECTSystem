-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
-- Modified By:		Evan Morrison
-- Modified Date:	1/17/2017
-- Description:		Make it so it doesnt add the miscelaneous Document category
--					if one isnt found
-- =============================================

-- Exec docs_GetCatsByView 9

CREATE PROCEDURE [dbo].[docs_GetCatsByView]
	-- Add the parameters for the stored procedure here
	@viewId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

-- Begin, ensure that Miscellaneous is LAST category to display
Declare @maxView int, @maxDocCatId int, @miscView int, @miscId int

Select dc2.DocCatId, dc2.CategoryDescription
From DocCategoryView dcv Inner Join DocumentCategory2 dc2 On dcv.DocCatId = dc2.DocCatId
Where DocViewId = @viewId
	And dc2.DocCatId != 73  -- Exclude 'AFPC Final Disposition' (same as 42 'Disposition')
Order By ViewOrder

END
GO

