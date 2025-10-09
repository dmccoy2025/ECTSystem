
--EXECUTE report_sp_GetLODStatistics 7,'12/01/2009','12/31/2009'
CREATE PROCEDURE [dbo].[report_sp_GetLODStatistics_old]
(
	 @userId int,
     @beginDate datetime,
	 @endDate datetime
)

AS


	SET NOCOUNT ON;

if (@beginDate is null)
   SET @beginDate = DATEADD(m, -30, getDate())
if(@endDate is null)
	SET @endDate = getDate()

DECLARE @chain_type INT, @cs_id INT

SELECT  @cs_id = unit_id, @chain_type = ViewType FROM  vw_users WHERE userid = @userId
 

Declare @temp table
(
	start datetime,
	last datetime,
	lodInitiated int,
	wingClosed int DEFAULT ((0)),
	rlpClosed int DEFAULT ((0)),
	wcrlb int DEFAULT ((0)),
	wcaa int DEFAULT ((0)),
	rlbaa int DEFAULT ((0))
)

Declare @count table
(
	ptypeId int,
	finding int
)

DECLARE @startDate datetime, @lastDate datetime

set @startDate = @beginDate
set @LastDate = @endDate

WHILE  @endDate > @startDate 
BEGIN
	SET @lastDate =  @startDate
	SET @lastDate =  DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,@lastDate)+1,0))

	If (@lastDate > @endDate)
		SET @lastDate = @endDate

	INSERT INTO @temp(start, last)
	VALUES(@startDate, @lastDate)
	
	SET @startDate = DATEADD(D,1,@lastDate)

END

DECLARE @firstDate DATETIME, @secondDate DATETIME, @wccaaCount int, @wcrlbCount int,@rlbaaCount int

DECLARE c2 CURSOR FOR
SELECT start,last FROM @temp

OPEN c2
FETCH next from c2 INTO @firstDate, @secondDate

WHILE @@FETCH_STATUS = 0
BEGIN

EXEC [dbo].[report_sp_GetConflictFindings]@cs_id,@chain_type,@firstDate,@secondDate,5,10,@wccaaCount OUTPUT
EXEC [dbo].[report_sp_GetConflictFindings]@cs_id,@chain_type,@firstDate,@secondDate,5,6,@wcrlbCount OUTPUT
EXEC [dbo].[report_sp_GetConflictFindings]@cs_id,@chain_type,@firstDate,@secondDate,6,10,@rlbaaCount OUTPUT

UPDATE @temp
SET lodInitiated = (select count(*) as TotalInitiated from form348 
						where 
							(DATEDIFF(dd, 0,  created_date ) BETWEEN   DATEDIFF(dd, 0,@firstDate ) AND  DATEDIFF(dd, 0,@secondDate ) 	)
						AND deleted=0					 
						AND ( coalesce(completed_by_unit, member_unit_id)  IN (
                 SELECT child_id FROM Command_Struct_Tree WHERE parent_id = @cs_id AND view_type = @chain_type 
		 )))
		
		
		,wingClosed = (	
						SELECT count(*) FROM  form348 a  
						INNER JOIN vw_WorkStatus v on v.ws_id =a.status 
						WHERE v.isFinal =1 	 
						AND a.deleted=0
						AND a.status<>26
						AND a.completed_by_unit  IN  (SELECT child_id FROM Command_Struct_Tree WHERE parent_id = @cs_id AND view_type = @chain_type)
						AND  (DATEDIFF(dd, 0,  a.created_date ) BETWEEN   DATEDIFF(dd, 0,@firstDate ) AND  DATEDIFF(dd, 0,@secondDate )  )
						  
	 					AND ( a.FinalDecision like '%Closed by Appointing Auth%' or FinalDecision Like '%Closed by Wing Commander%')
				)
			 		  
		,rlpClosed = (select count(*) from form348 a  
						  INNER JOIN vw_WorkStatus v on v.ws_id =a.status 
						  WHERE v.isFinal =1 	 
						    AND	a.completed_by_unit IN (SELECT child_id FROM Command_Struct_Tree WHERE parent_id = @cs_id AND view_type = @chain_type)
							AND a.deleted=0		
							AND a.status<>26							   
							AND (DATEDIFF(dd, 0,  a.created_date ) BETWEEN   DATEDIFF(dd, 0,@firstDate ) AND  DATEDIFF(dd, 0,@secondDate )  )
							AND (a.FinalDecision like '%Closed by LOD Board%'  ) 
							 
				  )
 
,wcaa  = @wccaaCount,
wcrlb = @wcrlbCount,
rlbaa = @rlbaaCount


WHERE start = @firstDate and last = @secondDate

FETCH next from c2 INTO @firstDate, @secondDate
END

CLOSE c2
DEALLOCATE c2

select start, last, cast(datepart(m,start) as varchar(2))+ '-' + cast(datepart(yyyy,start) as varchar(4))as LodMonth, 
lodInitiated, wingClosed, rlpClosed, wcrlb, wcaa, rlbaa 
 from @temp
GO

