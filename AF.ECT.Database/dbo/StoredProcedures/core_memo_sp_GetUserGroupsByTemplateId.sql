-- =============================================
-- Author:		Nicholas McQuillen
-- Create date: May 27, 2008
-- Description:	Retrieves workflowId, title and 
-- whether or not the workflow has been selected
-- =============================================
CREATE PROCEDURE [dbo].[core_memo_sp_GetUserGroupsByTemplateId]
	@templateId as tinyint,
	@compo as char(1)
AS
BEGIN
	SET NOCOUNT ON;
    
	SELECT 
		g.groupId,g.name, 
		isnull(canEdit, 0) canEdit, 
		isnull(canCreate, 0) canCreate, 
		isnull(canView, 0) canView, 
		isnull(canDelete, 0) canDelete
	FROM core_UserGroups g 
	LEFT JOIN 
		core_MemoGroups m on m.groupId=g.groupId and m.templateid=@templateId
	WHERE( g.compo=@compo or  g.compo='0')
	ORDER BY g.name
END


--core_memo_sp_GetUserGroupsByTemplateId 2, '6'
GO

