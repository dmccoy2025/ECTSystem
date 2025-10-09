




-- ============================================================================
-- Author:		Darel Johnson
-- Create date: 10/4/2019
-- Description:	Save or update a child comment
-- ============================================================================
CREATE PROCEDURE [dbo].[core_ChildCaseComments_SaveOrUpdateComment]
	@id INT,
	@refId INT, 
	@module INT, 
	@comment varchar(max),
	@userId INT,
	@createdDate DATETIME,
	--@deleted BIT,
	@commentType INT,
	@commentId INT,
	@role varchar(200)
AS
BEGIN
	
	IF(@id > 0)
	BEGIN
		UPDATE Child_Case_Comments
		SET comments = @comment, created_date = @createdDate, parentcommentID = @commentId, role = @role
		FROM Child_Case_Comments ch 
				INNER JOIN CaseDialogue_Comments c 
				ON ch.parentcommentid = c.id
		WHERE 
		c.id = @id AND c.lodid = @refId AND c.ModuleID = @module
	END
	ELSE
	BEGIN
		INSERT INTO Child_Case_Comments (lodid, ModuleID, comments, created_by, created_date, deleted, CommentType, parentcommentID, role)
		VALUES(@refId, @module, @comment, @userId, @createdDate, 0, @commentType, @commentId, @role)
	END

	
END
GO

