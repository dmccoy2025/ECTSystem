






-- ============================================================================
-- Author:		Darel Johnson
-- Create date: 10/4/2019
-- Description:	Get Child Case Comment by Case
-- ============================================================================
CREATE PROCEDURE [dbo].[core_ChildCaseComment_GetByCase]
	@refid INT,
	@module INT,
	@commentType INT,
	@commentId INT
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
		WHERE ch.[lodid] = @refid
		    AND c.ModuleID = @module
			AND ch.CommentType = @commentType
			AND ch.ParentCommentID = @commentId
		ORDER BY ch.created_date ASC
		
END
GO

