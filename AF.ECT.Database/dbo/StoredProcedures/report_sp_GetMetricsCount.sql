CREATE proc [dbo].[report_sp_GetMetricsCount]
(
	@cs_id int,
	@chain_type int,
	@ssn varchar(10),
	@beginDate datetime,
	@endDate datetime,
	@isComplete tinyint,
	@includeSubordinate bit
)

AS
	SET NOCOUNT ON;


--DECLARE @cs_id int
--DECLARE @chain_type int
--
--
--SET @cs_id = 3
--SET @chain_type = 3

	if (@ssn = '')
		SET @ssn = null;
	if (@beginDate is null)
	   select @beginDate = min(created_date) from form348
	if(@endDate is null)
		SET @endDate = getDate()

DECLARE @counts TABLE
(
      ws_id int,
      days decimal(9,2),
      num decimal(9,2),
      title varchar(50)
)

 
--get counts for everything except the board
INSERT INTO @counts (ws_id, days, num, title)
SELECT a.ws_id, sum(datediff(d, a.startdate, isnull(a.enddate,getdate()))), count(*), description 
FROM core_WorkStatus_Tracking a
JOIN vw_WorkStatus w ON w.ws_id = a.ws_id
WHERE 
      a.refId IN (
            SELECT b.lodid     
				FROM form348 b
				LEFT JOIN vw_workstatus vw ON  vw.ws_id=b.status				 
				WHERE 
				 coalesce(b.completed_by_unit,b.member_unit_id)			
			 	IN  (SELECT child_id FROM Command_Struct_Tree WHERE parent_id=@cs_id and view_type=@chain_type )
		  		AND
					(
						CASE 
						WHEN @isComplete=1 THEN    1 
						WHEN @isComplete=0 THEN    0 
						ELSE  vw.IsFinal
						END =vw.IsFinal 
					)
			 
				AND   (	DATEDIFF(dd, 0, b.created_date ) BETWEEN   DATEDIFF(dd, 0,@beginDate ) AND  DATEDIFF(dd, 0,@endDate ) )   
 	 	    	AND b.member_ssn = IsNull(@ssn,b.member_ssn) 
            

	)
AND
      a.ws_id IN (1, 2, 3, 4, 5)
GROUP BY a.ws_id, w.description



--get the board count
INSERT INTO @counts (ws_id, days, num, title)
SELECT 0, sum(datediff(d, startdate, isnull(enddate,getDate()))), count(*), 'LOD Board' FROM core_WorkStatus_Tracking
WHERE 
      refId IN (
				SELECT b.lodid     
				FROM form348 b
				LEFT JOIN vw_workstatus vw ON  vw.ws_id=b.status				 
				WHERE 
				 coalesce(b.completed_by_unit,b.member_unit_id)	IN (SELECT child_id FROM Command_Struct_Tree WHERE parent_id=@cs_id and view_type=@chain_type )			 	 
				 AND
					(
						CASE 
						WHEN @isComplete=1 THEN    1 
						WHEN @isComplete=0 THEN    0 
						ELSE  vw.IsFinal
						END =vw.IsFinal 
					)
			 	AND (	DATEDIFF(dd, 0, b.created_date ) BETWEEN   DATEDIFF(dd, 0,@beginDate ) AND  DATEDIFF(dd, 0,@endDate ) )   
 	 			AND b.member_ssn = IsNull(@ssn,b.member_ssn)  
            )
AND
      ws_id IN (8, 9, 10,16,17, 18, 19, 20,21,22)


--get the formal investigation and MPF count
INSERT INTO @counts (ws_id, days, num, title)
SELECT 11, sum(datediff(d, startdate, isnull(enddate,getDate()))), count(*), 'Formal Inv' FROM core_WorkStatus_Tracking
WHERE 
      refId IN (
			   SELECT b.lodid     
				FROM form348 b
				LEFT JOIN vw_workstatus vw ON  vw.ws_id=b.status
				WHERE 
				 coalesce(b.completed_by_unit,b.member_unit_id)	IN (SELECT child_id FROM Command_Struct_Tree WHERE parent_id=@cs_id and view_type=@chain_type )			 
				 AND
					(
						CASE 
						WHEN @isComplete=1 THEN    1 
						WHEN @isComplete=0 THEN    0 
						ELSE  vw.IsFinal
						END =vw.IsFinal 
					)
			 
				AND 	 (	DATEDIFF(dd, 0, b.created_date ) BETWEEN   DATEDIFF(dd, 0,@beginDate ) AND  DATEDIFF(dd, 0,@endDate ) )   
 	 			AND b.member_ssn = IsNull(@ssn,b.member_ssn) 
            )
AND
      ws_id IN (11,7) 



--select * from @counts

SELECT distinct
		cast(isnull((SELECT ROUND(mu.days/mu.num,1,1) from @counts mu where mu.ws_id = 1),0)as decimal(8,1)) as [MedicalUnit],
	    cast(isnull((SELECT ROUND(mo.days/mo.num,1) from @counts mo where mo.ws_id = 2),0)as decimal(8,1)) as [MedicalOfficer],
		cast(isnull((SELECT ROUND(un.days/un.num,1) from @counts un where un.ws_id = 3),0)as decimal(8,1)) as [Unit],
	    cast(isnull((SELECT ROUND(wj.days/wj.num,1) from @counts wj where wj.ws_id = 4),0)as decimal(8,1)) as [WingJA],
		cast(isnull((SELECT ROUND(wc.days/wc.num,1) from @counts wc where wc.ws_id = 5),0)as decimal(8,1)) as [WingCC],
		cast(isnull((SELECT ROUND(lb.days/lb.num,1) from @counts lb where lb.ws_id = 0),0)as decimal(8,1)) as [LODBoard],
		cast(isnull((SELECT ROUND(fi.days/fi.num,1) from @counts fi where fi.ws_id = 11),0)as decimal(8,1)) as [FormalInv]
FROM @counts a
GO

