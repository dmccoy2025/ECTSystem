CREATE PROCEDURE [dbo].[report_sp_GetDetailLodByUnitId]
	
	@csId INT,
	@beginDate datetime,
	@endDate datetime,
	@isComplete tinyint

AS
	SET NOCOUNT ON; 
	
if (@beginDate is null)
   select @beginDate = min(created_date) from form348
if(@endDate is null)
	SET @endDate = getDate()

SELECT
	 a.lodid,
	 a.member_unit_id, 
	 a.case_id, 
     a. member_unit, 
     b.event_nature_type ,
    vw.Description AS description ,
    b.icd9Id ,
    ic.text as icdName 
FROM 
     form348 a 
LEFT JOIN vw_workstatus vw ON  vw.ws_id=a.status
INNER JOIN form348_medical b on a.lodid = b.lodid
LEFT JOIN core_lkupICD9  ic on  ic.ICD9_ID = b.icd9Id 
 WHERE 
  coalesce(a.completed_by_unit,a.member_unit_id)	=@csId		 	 
  AND b.event_nature_type in ('Disease', 'Illness' , 'injury', 'Death')
  AND (DATEDIFF(dd, 0, a.created_date ) BETWEEN   DATEDIFF(dd, 0,@beginDate ) AND  DATEDIFF(dd, 0,@endDate ) )
  AND a.deleted =0 
  AND (
   CASE 
   WHEN @isComplete=1 THEN    1 
   WHEN @isComplete=0 THEN    0 
   ELSE  vw.IsFinal
   END =vw.IsFinal ) 
 Order by
	a.lodid desc, b.event_nature_type
GO

