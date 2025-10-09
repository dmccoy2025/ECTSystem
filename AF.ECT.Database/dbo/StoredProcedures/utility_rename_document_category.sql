
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 1/12/2016
-- Description:	Renames a document category in the DocumentCategory2 table.
-- ============================================================================
CREATE PROCEDURE [dbo].[utility_rename_document_category]
	 @oldDescription varchar(150)
	,@newDescription varchar(150)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@oldDescription, '') = '')
	BEGIN
		PRINT '@oldDescription cannot be NULL or empty!'
		RETURN
	END
	
	IF (ISNULL(@newDescription, '') = '')
	BEGIN
		PRINT '@newDescription cannot be NULL or empty!'
		RETURN
	END
	
	DECLARE @count INT = 0
	
	-- Check if a record with the old description exists...
	SELECT	@count = COUNT(*)
	FROM	DocumentCategory2 dc
	WHERE	dc.CategoryDescription = @oldDescription
	
	IF (ISNULL(@count, 0) = 0)
	BEGIN
		PRINT 'Could not find a Document Category record with a description of ' + @oldDescription + '.'
		RETURN
	END
	
	-- Check if there exists multiple records with the old description...
	IF (@count > 1)
	BEGIN
		PRINT 'Found multiple Document Category records with a description of ' + @oldDescription + '.'
		RETURN
	END
	
	-- Check if there already exists a record with the new decription...
	SET @count = 0
	
	SELECT	@count = COUNT(*)
	FROM	DocumentCategory2 dc
	WHERE	dc.CategoryDescription = @newDescription
	
	IF (@count > 0)
	BEGIN
		PRINT 'A Document Category with a description of ' + @newDescription + ' already exists.'
		RETURN
	END
	
	-- Update the Document Category record...
	UPDATE	DocumentCategory2
	SET		CategoryDescription = @newDescription
	WHERE	CategoryDescription = @oldDescription
END
GO

