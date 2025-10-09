



-- ============================================================================
-- Author:		Darel Johnson
-- Create date: 10/4/2019
-- Description:	Get Child Case Comment by Id
-- ============================================================================
CREATE PROCEDURE [dbo].[core_ChildCaseComment_GetById]
	@id INT
AS
BEGIN
	
		SELECT ch.[id]
		  ,ch.[lodid]
		  ,ch.[comments]
		  ,ch.[created_by]
		  ,ch.[created_date]
		  ,c.[deleted]
		  ,ch.[ModuleID]
		  ,ch.[CommentType]
		  ,ch.[ParentCommentID]
		  ,ch.[role]
		FROM Child_Case_Comments ch 
		INNER JOIN CaseDialogue_Comments c 
		ON ch.parentcommentid = c.id
		WHERE ch.id = @id
END
GO

