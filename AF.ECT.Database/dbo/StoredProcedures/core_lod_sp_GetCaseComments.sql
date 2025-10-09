-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[core_lod_sp_GetCaseComments]
(
	@caseID int,
	@ModuleID int,
	@commentType int,
	@SortOrder bit
)

As
	If @SortOrder = 0
		Begin
			SELECT LodId, created_date, created_by, comments FROM  Case_Comments
			Where lodid = @caseID and ModuleID = @ModuleID and CommentType = @commentType
			Order by created_date desc
		End
	Else
		Begin
			SELECT LodId, created_date, created_by, comments FROM  Case_Comments
			Where lodid = @caseID and ModuleID = @ModuleID and CommentType = @commentType
			Order by created_date asc
		End
GO

