
--=========================================================
-- Modified By:		Evan Morrison
-- Modified Date:	7/24/2017
-- Description:		Workstatus now takes in an integer
--=========================================================

CREATE PROCEDURE [dbo].[core_workflow_sp_GetPageAccessByGroup]
	@workflow tinyint,
	@status INT,
	@group tinyint

AS

SET NOCOUNT ON

--Id          PageId GroupId Access WorkflowId WorkStatusId PageTitle


SELECT DISTINCT 
	a.pageId PageId
--	, c.mapId Id
	,d.groupId GroupId
	,CAST (CASE c.access
		WHEN 2 THEN '2'
		WHEN 1 THEN '1'		
		ELSE '0'
	END AS tinyint) AS Access
	,@workflow WorkflowId, @status WorkStatusId
	,b.title PageTitle
--	,d.accessScope
FROM 
	core_workflowviews a
INNER JOIN 
	core_pages b ON b.pageId = a.pageId
LEFT JOIN  
	core_UserGroups d ON d.groupId = @group
LEFT JOIN 
	core_PageAccess c ON c.pageId = b.pageId 
		AND c.groupId = d.groupId 
		AND c.workflowId = @workflow
		AND c.statusId = @status
WHERE 
	a.workflowId = @workflow
ORDER BY 
	a.pageId
--	, d.accessScope
GO

