
CREATE PROCEDURE [dbo].[report_sp_GetConflictFindings]
(
	 @cs_id int,
	 @chain_type int,
     @beginDate datetime,
	 @endDate datetime,
	 @firstPtype int,
	 @secondPtype int,
	 @returnVal int out
)

AS

SET NOCOUNT ON;

Declare @temp1 table
(
	lodid int,
	finding int
)

Declare @temp2 table
(
	lodid int,
	finding int
)

insert into @temp1(lodid,finding)
SELECT  DISTINCT a.lodid, b.finding

FROM 
		form348 a
		INNER JOIN form348_findings b on a.lodid = b.lodid 
		INNER JOIN vw_WorkStatus v on v.ws_id =a.status 
WHERE v.isFinal =1 	
AND a.status<>26 
AND a.deleted=0
AND (b.ptype = @firstPtype)
AND 
     (
        @firstPtype=5 and b.finding in ( 1,2,3,4,5,6)
        or 
        @firstPtype=10 and b.finding in ( 1,2,3,4,5,6) 
        or 
        @firstPtype=6 and b.finding in ( 1,3) 
     
     ) 		
AND( DATEDIFF(dd, 0, a.created_date ) BETWEEN   DATEDIFF(dd, 0,@beginDate ) AND  DATEDIFF(dd, 0,@endDate ) )
AND a.completed_by_unit IN(
SELECT child_id FROM Command_Struct_Tree WHERE parent_id = @cs_id AND view_type = @chain_type)

order by a.lodid

insert into @temp2(lodid,finding)
SELECT  DISTINCT a.lodid, b.finding
FROM 
		form348 a
		INNER JOIN form348_findings b on a.lodid = b.lodid  
		INNER JOIN vw_WorkStatus v on v.ws_id =a.status 
WHERE v.isFinal =1 	
AND a.status<>26 
AND a.deleted=0
AND (b.ptype = @secondPtype)
AND 
     (
        @secondPtype=5 and b.finding in ( 1,2,3,4,5,6)
        or 
        @secondPtype=10 and b.finding in ( 1,2,3,4,5,6) 
        or 
        @secondPtype=6 and b.finding in ( 1,3) 
     
     ) 
AND (DATEDIFF(dd, 0, a.created_date ) BETWEEN   DATEDIFF(dd, 0,@beginDate ) AND  DATEDIFF(dd, 0,@endDate ) )
AND a.completed_by_unit IN(
SELECT child_id FROM Command_Struct_Tree WHERE parent_id = @cs_id AND view_type = @chain_type)
order by a.lodid




declare @i int

set @i = (SELECT
				count(*)
				FROM form348 c
				WHERE c.lodid IN (
				select DISTINCT a.lodid from @temp1 a INNER JOIN @temp2 b ON a.lodid = b.lodid where isnull(a.finding,'') <> isnull(b.finding,'')))
 
select @returnVal =@i
GO

