--Select * from form348_findings
CREATE VIEW [dbo].[vw_Findings]
AS
SELECT    a.case_id, a.lodid, p.Description, p.RoleName ,f.ssn,f.name,f.grade,f.compo,f.rank,f.pascode,f.finding,f.decision_yn,f.explanation,f.findingsText
FROM       form348   a 
LEFT OUTER JOIN 
	dbo.form348_findings AS f ON f.lodid = a.lodid 
INNER JOIN
  dbo.core_lkupPersonnelTypes AS p ON p.id = f.ptype
GO

