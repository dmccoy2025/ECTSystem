
CREATE PROCEDURE [dbo].[utility_delete_documentCategoryView]  
	@categoryDesc VARCHAR(100),
	@docViewId INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @docCategoryId INT
	DECLARE @results INT

	SELECT @docCategoryId = DocCatId FROM DocumentCategory2 WHERE CategoryDescription = @categoryDesc 

	SET @results = 0
	
	SELECT	@results = COUNT(*) 
	FROM	DocCategoryView 
	WHERE	DocViewId = @docViewId
			AND DocCatId = @docCategoryId

	IF @results > 0
		BEGIN
			DELETE FROM DocCategoryView
			WHERE DocViewId = @docViewId
				  AND DocCatId = @docCategoryId
			
			PRINT 'Document Category View deleted: ' + CONVERT(VARCHAR(2), @docCategoryId) + ', ' + CONVERT(VARCHAR(2), @docViewId) 
		END
	ELSE
		BEGIN
			PRINT 'Document Category View does not exists: ' + CONVERT(VARCHAR(2), @docCategoryId) + ', ' + CONVERT(VARCHAR(2), @docViewId)	
		END

END
GO

