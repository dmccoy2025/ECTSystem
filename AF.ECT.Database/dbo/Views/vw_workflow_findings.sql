
--Select * from form348_findings
CREATE VIEW [dbo].[vw_workflow_findings]
AS
SELECT    wf.WorkflowId,
		  f.Id,
		  f.Description 
FROM      core_Workflow_Findings wf
JOIN	  core_lkUpFindings f ON f.Id = wf.FindingId
GO

