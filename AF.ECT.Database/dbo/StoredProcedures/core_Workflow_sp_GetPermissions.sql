
-- =============================================
-- Author:		Andy Cooper
-- Create date: 8 May 2008
-- Description:	Inserts a new status code
-- =============================================
CREATE PROCEDURE [dbo].[core_Workflow_sp_GetPermissions]
	@workflowId tinyint
AS

SET NOCOUNT ON

SELECT
	a.groupId, a.name, b.canView, b.canCreate
FROM 
	core_UserGroups a
LEFT JOIN 
	core_WorkflowPerms b ON b.groupId = a.groupId AND b.workflowId = @workflowId
WHERE 
	a.compo = (SELECT compo FROM core_Workflow WHERE workflowId = @workflowId)
ORDER BY
	a.accessScope, a.name
GO

