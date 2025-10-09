
--=========================================================
-- Modified By:		Evan Morrison
-- Modified Date:	7/24/2017
-- Description:		Workstatus now takes in an integer
--=========================================================

CREATE PROCEDURE [dbo].[core_workflow_sp_GetPageAccessByWorkflowView]
	@compo char(1),
	@workflow tinyint,
	@status INT

AS

SET NOCOUNT ON

IF (@workflow = 0 OR @status = 0)
	RETURN 

SELECT DISTINCT
	a.pageId PageId
--	, c.mapId Id
	,  d.groupId GroupId --, d.name groupName
	,CAST (CASE c.access
		WHEN 2 THEN '2'
		WHEN 1 THEN '1'		
		ELSE '0'
	END AS tinyint) AS Access
	,@workflow WorkflowId
	,@status WorkStatusId
	,b.title PageTitle
FROM 
	core_workflowviews a
INNER JOIN 
	core_pages b ON b.pageId = a.pageId
CROSS JOIN  
	core_UserGroups d
LEFT JOIN 
	core_PageAccess c ON c.pageId = b.pageId 
		AND c.groupId = d.groupId 
		AND c.workflowId = @workflow
		AND c.statusId = @status
WHERE 
	a.workflowId = @workflow
AND 
	d.compo = @compo
ORDER BY 
	a.pageId
	-- , d.accessScope
	-- , c.groupId
GO

