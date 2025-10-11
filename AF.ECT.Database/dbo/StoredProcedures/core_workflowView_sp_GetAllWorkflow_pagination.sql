CREATE PROCEDURE [dbo].[core_workflowView_sp_GetAllWorkflow_pagination]
	@PageNumber INT = 1,
	@PageSize INT = 10
AS

	SET NOCOUNT ON;

SELECT	
	a.workflowId
	,a.title + ' - ' + b.compo_descr AS title
FROM dbo.core_Workflow a
INNER JOIN core_lkupCompo b ON a.compo = b.compo
ORDER BY a.compo, a.workflowId
OFFSET (@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
GO