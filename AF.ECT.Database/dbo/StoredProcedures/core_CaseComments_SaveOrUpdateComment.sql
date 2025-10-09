
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 4/7/2017
-- Description:	Save or update a comment
-- ============================================================================
CREATE PROCEDURE [dbo].[core_CaseComments_SaveOrUpdateComment]
	@id INT,
	@refId INT, 
	@module INT, 
	@comment varchar(max),
	@userId INT,
	@createdDate DATETIME,
	@deleted INT,
	@commentType INT
AS
BEGIN
	
	IF(@id > 0)
	BEGIN
		UPDATE Case_Comments
		SET comments = @comment, created_date = @createdDate, deleted = @deleted
		WHERE id = @id AND lodid = @refId AND ModuleID = @module
	END
	ELSE
	BEGIN
		INSERT INTO Case_Comments (lodid, ModuleID, comments, created_by, created_date, deleted, CommentType)
		VALUES(@refId, @module, @comment, @userId, @createdDate, 0, @commentType)
	END

	
END
GO

