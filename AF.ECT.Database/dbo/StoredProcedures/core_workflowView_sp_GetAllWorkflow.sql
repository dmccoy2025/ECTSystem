CREATE PROCEDURE [dbo].[core_workflowView_sp_GetAllWorkflow]

AS

	SET NOCOUNT ON;

SELECT	
	a.workflowId
	,a.title + ' - ' + b.compo_descr AS title
FROM dbo.core_Workflow a
INNER JOIN core_lkupCompo b ON a.compo = b.compo
ORDER BY a.compo, a.workflowId
GO

