-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[utility_insert_document_category] 
	@Id INT,
	@catDescr VARCHAR(50)
	
AS
BEGIN
	
	SET NOCOUNT ON;
	DECLARE @results INT

	SET @results = 0
	SELECT @results = COUNT(*) FROM DocumentCategory2 
		WHERE DocCatId = @Id

	IF @results = 0
		BEGIN
			INSERT INTO DocumentCategory2(DocCatId, CategoryDescription)VALUES(@Id, @catDescr)
			PRINT 'Document Category added: ' + CONVERT(VARCHAR(2), @Id) + ', ' + CONVERT(VARCHAR(50), @catDescr)
		END
	ELSE
		BEGIN
			PRINT 'Document Category already exits: ' + CONVERT(VARCHAR(2), @Id) + ', ' + CONVERT(VARCHAR(50), @catDescr)	
		
		END

END
GO

