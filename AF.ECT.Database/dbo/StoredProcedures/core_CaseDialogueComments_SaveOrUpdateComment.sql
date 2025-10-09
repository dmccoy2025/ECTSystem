

-- ============================================================================
-- Author:		Darel Johnson
-- Create date: 10/22/2019
-- Description:	Save or update a case dialogue comment
-- ============================================================================
CREATE PROCEDURE [dbo].[core_CaseDialogueComments_SaveOrUpdateComment]
	@id INT,
	@refId INT, 
	@module INT, 
	@comment varchar(max),
	@userId INT,
	@createdDate DATETIME,
	@deleted INT,
	@commentType INT,
	@role varchar(200)
AS
BEGIN
	
	IF(@id > 0)
	BEGIN
		UPDATE CaseDialogue_Comments
		SET comments = @comment, created_date = @createdDate, deleted = @deleted, role = @role
		WHERE id = @id AND lodid = @refId AND ModuleID = @module
	END
	ELSE
	BEGIN
		INSERT INTO CaseDialogue_Comments (lodid, ModuleID, comments, created_by, created_date, deleted, CommentType, role)
		VALUES(@refId, @module, @comment, @userId, @createdDate, 0, @commentType, @role)
	END

	
END
GO

