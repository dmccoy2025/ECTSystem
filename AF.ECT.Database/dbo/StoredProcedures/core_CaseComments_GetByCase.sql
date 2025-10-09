
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 4/6/2017
-- Description:	Get Case Comments by case and module
-- ============================================================================
CREATE PROCEDURE [dbo].[core_CaseComments_GetByCase]
	@refid INT,
	@module INT,
	@commentType INT,
	@sorted BIT
AS
BEGIN
	
	IF(@sorted = 0)
	BEGIN
		SELECT * 
		FROM Case_Comments 
		Where lodid = @refid 
			AND ModuleID = @module
			AND CommentType = @commentType
	END
	ELSE
	BEGIN
		SELECT * 
		FROM Case_Comments 
		Where lodid = @refid 
			AND ModuleID = @module
			AND CommentType = @commentType
		ORDER BY created_date DESC
	END
END
GO

