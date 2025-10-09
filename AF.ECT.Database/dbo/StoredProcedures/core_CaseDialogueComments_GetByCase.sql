


-- ============================================================================
-- Author:		Darel Johnson
-- Create date: 10/22/2019
-- Description:	Get Case Dialogue Comments by case and module
-- ============================================================================
CREATE PROCEDURE [dbo].[core_CaseDialogueComments_GetByCase]
	@refid INT,
	@module INT,
	@commentType INT,
	@sorted BIT
AS
BEGIN
	
	IF(@sorted = 0)
	BEGIN
		SELECT * 
		FROM CaseDialogue_Comments 
		Where lodid = @refid 
			AND ModuleID = @module
			AND CommentType = @commentType
	END
	ELSE
	BEGIN
		SELECT * 
		FROM CaseDialogue_Comments 
		Where lodid = @refid 
			AND ModuleID = @module
			AND CommentType = @commentType
		ORDER BY created_date DESC
	END
END
GO

