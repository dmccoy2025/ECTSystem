CREATE PROCEDURE [dbo].[Print_sp_Form348]
(
	@lodId INT
)

AS
	SET NOCOUNT ON;
-- query may needs to be modified once the pdf fields known
SELECT 
	   a.member_name, 
	   a.member_ssn, 
	   a.member_grade, 
	   a.member_unit,
	   b.event_nature_details, 
	   c.cmdr_circ_details 
FROM
	form348 a 
INNER JOIN 
	form348_Medical b ON a.lodid = b.lodid 
INNER JOIN 
	form348_Unit c ON a.lodid = c.lodid
WHERE 
	a.lodid = @lodId
GO

