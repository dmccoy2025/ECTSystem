CREATE PROCEDURE [dbo].[tmp1_report_sp_GetDisapprovedLod]
(
	  
     @beginDate datetime,
	 @endDate datetime
)

AS

	SET NOCOUNT ON;


 
if (@beginDate is null)
   select @beginDate = min(created_date) from form348
if(@endDate is null)
	SET @endDate = getDate()
	
	
 
 
Declare @temp table
(
	pTypeId INT,
	pName varchar(100),
	eptId int,
	eptCount int,
	nIlodId int,
	nILodCount int,
	nIlodNotId int,
	nIlodNotCount int
)

Declare @count table
(
	ptypeId int,
	finding int
)

INSERT INTO @count (ptypeId, finding)

	SELECT   b.ptype, b.finding FROM  form348 a 
		INNER JOIN   form348_findings b on a.lodid = b.lodid 
		INNER JOIN vw_WorkStatus v on v.ws_id=a.status  
	WHERE v.isFinal =1 
	AND (b.ptype IN (5,10)) 
	AND (b.finding IN (2,4,5))
	 
	 AND a.FinalFindings in (2,4,5)
	AND
	(
					/* This code is to get the records falling on the begin  and end dates also to be included.DATEDIFF(dd, 0, a.created_date ) will get the date only protion */
					 DATEDIFF(dd, 0, a.created_date ) BETWEEN   DATEDIFF(dd, 0,@beginDate ) AND  DATEDIFF(dd, 0,@endDate ) 
					 
	 )
  
INSERT INTO @temp(pTypeId, pName)
	select id, roleName from core_lkupPersonnelTypes where id IN(5,10)

UPDATE @temp 
 SET eptid = 2,
	 eptCount = (select count(*) from @count b where b.ptypeId = t.pTypeid and b.finding  = 2),
	 nIlodId = 4,
	 nILodCount = (select count(*) from @count b where b.ptypeId = t.pTypeid and b.finding  = 4),
	 nIlodNotId = 5,
	 nIlodNotCount = (select count(*) from @count b where b.ptypeId = t.pTypeid and b.finding  = 5)  
	 from @temp t
WHERE t.pTypeid IN (5,10)

select * from @temp
GO

