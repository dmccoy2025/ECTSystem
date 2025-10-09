-- ============================================================================
-- Author:		<George Nebeling>
-- Create date: <2/29/2011>
-- Description:	<Gets the Number Of Lessons Learned>
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified date:	11/8/2016
-- Description:		Update where the lessons learned data is being held
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified date:	4/7/2017
-- Description:		only returns the count of lessons learned
-- ============================================================================
CREATE PROCEDURE [dbo].[lod_sp_GetNumberOfLessonsLearned] 
	@refId int 
AS
BEGIN
	SELECT COUNT(*) as LessonsLearnedCount from Case_Comments where lodid = @refId AND CommentType = 8
END
GO

