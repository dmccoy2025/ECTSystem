-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[utility_insert_documentCategoryView]  
	@categoryDesc VARCHAR(100),
	@docViewId INT,
	@sortOrder INT,
	@isRedacted BIT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @docCategory INT
	DECLARE @results INT

	SELECT @docCategory = DocCatId FROM DocumentCategory2 WHERE CategoryDescription = @categoryDesc 

	SET @results = 0
	SELECT @results = COUNT(*) FROM DocCategoryView 
		WHERE DocViewId = @docViewId
		AND DocCatId = @docCategory

	IF @results = 0
		BEGIN
			INSERT	INTO	[dbo].[DocCategoryView] ([DocViewId],[DocCatId],[ViewOrder], [IsRedacted])
					VALUES	(@docViewId, @docCategory, @sortOrder, @isRedacted)		

			PRINT 'Document Category Views added: ' + CONVERT(VARCHAR(2), @docCategory) + ', ' + CONVERT(VARCHAR(2), @docViewId) +
			', ' + CONVERT(VARCHAR(2),@sortOrder) 
		END
	ELSE
		BEGIN
			PRINT 'Document Category Views already exists: ' + CONVERT(VARCHAR(2), @docCategory) + ', ' + CONVERT(VARCHAR(2), @docViewId) +
			', ' + CONVERT(VARCHAR(2),@sortOrder) 		
		END

END
GO

