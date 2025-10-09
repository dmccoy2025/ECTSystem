
-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	7/29/2016
-- Description:		Updated to no longer use a hard coded value for the LOD
--					cancel status.
-- ============================================================================
CREATE PROCEDURE [dbo].[report_sp_GetLODStatistics]
(
	 @userId int,
     @beginDate datetime,
	 @endDate datetime
)
AS
BEGIN
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
		mon varchar(10),
		yr varchar(10) 	 
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

		INSERT	INTO	@temp( start, last,mon ,yr)	 
				VALUES	(@startDate, @lastDate ,DATENAME(mm, @startDate), DATENAME(yyyy, @startDate))
	
		SET @startDate = DATEADD(D,1,@lastDate)
	END
 
  
 
	declare @Units TABLE
	(
		child_id int,
		PRIMARY KEY (child_id)
	)
 
	Insert	into	@Units
			SELECT	distinct child_id FROM Command_Struct_Tree WHERE parent_id = @cs_id AND view_type = @chain_type 
 
 
	;WITH 
	Type1 as 
	(
		SELECT  DISTINCT a.lodid, b.finding, a.created_date
		FROM	form348 a
				INNER JOIN form348_findings b on a.lodid = b.lodid  
				INNER JOIN vw_WorkStatus v on v.ws_id =a.status 
		WHERE	v.isFinal =1 	
				AND v.isCancel = 0
				AND a.deleted=0
				AND (b.ptype = 5)
				AND  b.finding in ( 1,2,3,4,5,6)
				AND( DATEDIFF(dd, 0, a.created_date ) BETWEEN   DATEDIFF(dd, 0,@beginDate ) AND  DATEDIFF(dd, 0,@endDate ) )
				AND a.completed_by_unit IN( SELECT child_id FROM @Units)
	),
	Type2 as 
	(
		SELECT	DISTINCT a.lodid, b.finding,a.created_date
		FROM	form348 a
				INNER JOIN form348_findings b on a.lodid = b.lodid  
				INNER JOIN vw_WorkStatus v on v.ws_id =a.status 
		WHERE	v.isFinal =1 	
				AND v.isCancel = 0
				AND a.deleted=0
				AND (b.ptype = 10) 
				AND  b.finding in ( 1,2,3,4,5,6) 
				AND( DATEDIFF(dd, 0, a.created_date ) BETWEEN   DATEDIFF(dd, 0,@beginDate ) AND  DATEDIFF(dd, 0,@endDate ) )
				AND a.completed_by_unit IN( SELECT child_id FROM @Units)
	),
	Type3 as 
	(
		SELECT  DISTINCT a.lodid, b.finding, a.created_date
		FROM	form348 a
				INNER JOIN form348_findings b on a.lodid = b.lodid  
				INNER JOIN vw_WorkStatus v on v.ws_id =a.status 
		WHERE	v.isFinal =1 	
				AND v.isCancel = 0
				AND a.deleted=0
				AND (b.ptype = 6)
				AND  b.finding in ( 1,3)
				AND( DATEDIFF(dd, 0, a.created_date ) BETWEEN   DATEDIFF(dd, 0,@beginDate ) AND  DATEDIFF(dd, 0,@endDate ) )
				AND a.completed_by_unit IN( SELECT child_id FROM @Units)
	) 
	SELECT	start, last, cast(datepart(m,start) as varchar(2))+ '-' + cast(datepart(yyyy,start) as varchar(4))as LodMonth ,
			IsNull(o.LodCreated,0) as lodInitiated,IsNull(q.wingClosed,0)as wingClosed ,Isnull(p.rlpClosed ,0) as rlpClosed,
			IsNull(s.wcrlbCount,0)as wcrlb,IsNull(r.wccaaCount,0)as wcaa,IsNull(t.rlbaaCount,0)as rlbaa,a.mon,a.yr  
	from @temp AS a  
	Left Join  
	(
		SELECT     
		DATENAME(mm, created_date) AS mon,
		DATENAME(yyyy, created_date) AS yr,
		COUNT(*) AS LodCreated
		FROM FORM348   
		WHERE 
		DATEDIFF(dd, 0,  created_date ) BETWEEN   DATEDIFF(dd, 0,@beginDate ) AND  DATEDIFF(dd, 0,@endDate ) 	 
		AND deleted=0					 
		AND  coalesce(completed_by_unit, member_unit_id)  IN (SELECT child_id FROM Command_Struct_Tree WHERE parent_id = @cs_id AND view_type = @chain_type )
		GROUP BY   DATENAME(mm, created_date), DATENAME(yyyy, created_date)  
	)  as o on   o.mon= a.mon and  a.yr= o.yr 
	Left Join  
	(
		SELECT  DATENAME(mm, created_date) AS mon,
				DATENAME(yyyy, created_date) AS yr,
				COUNT(*) as  rlpClosed    
		FROM	FORM348 a INNER JOIN vw_WorkStatus v on v.ws_id =a.status 
		WHERE	v.isFinal =1 AND a.deleted=0 AND v.isCancel = 0
				AND a.completed_by_unit  IN  (SELECT child_id FROM @Units  )
				AND   DATEDIFF(dd, 0,  a.created_date ) BETWEEN   DATEDIFF(dd, 0,@beginDate ) AND  DATEDIFF(dd, 0,@endDate )  
				AND    FinalDecision like '%Closed by LOD Board%'  
		GROUP	BY   DATENAME(mm, created_date),      DATENAME(yyyy, created_date) 
	)  
	p  on a.mon= p.mon and  a.yr= p.yr 
	Left Join  
	(
		SELECT  DATENAME(mm, created_date) AS mon,
				DATENAME(yyyy, created_date) AS yr,
				COUNT(*) as  wingClosed    
		FROM	FORM348 a INNER JOIN vw_WorkStatus v on v.ws_id =a.status 
		WHERE	v.isFinal =1 AND a.deleted=0 AND v.isCancel = 0
				AND a.completed_by_unit  IN  (SELECT child_id FROM @Units  )
				AND   DATEDIFF(dd, 0,  a.created_date ) BETWEEN   DATEDIFF(dd, 0,@beginDate ) AND  DATEDIFF(dd, 0,@endDate )  
				AND (FinalDecision like '%Closed by Appointing Auth%' or FinalDecision Like '%Closed by Wing Commander%' )
		GROUP	BY   DATENAME(mm, created_date),      DATENAME(yyyy, created_date)
	) 
	q on a.mon= q.mon and  a.yr= q.yr 
	Left Join  
	(   
		select	count (DISTINCT a.lodid ) as wccaaCount ,DATENAME(mm, a.created_date) AS mon,
				DATENAME(yyyy, a.created_date) AS yr 
		from	Type1 a INNER JOIN Type2 b ON a.lodid = b.lodid 
		where	isnull(a.finding,'') <> isnull(b.finding,'') 
		GROUP	BY DATENAME(mm, a.created_date),      DATENAME(yyyy, a.created_date)
	) 
	r on a.mon= r.mon and  a.yr= r.yr
	Left Join  
	(  
		SELECT	count (DISTINCT a.lodid ) AS wcrlbCount  ,DATENAME(mm, a.created_date) AS mon,
				DATENAME(yyyy, a.created_date) AS yr 
		from	Type1 a INNER JOIN Type3 b ON a.lodid = b.lodid 
		where	isnull(a.finding,'') <> isnull(b.finding,'') 
		GROUP	BY DATENAME(mm, a.created_date), DATENAME(yyyy, a.created_date)
	) 
	s on a.mon= s.mon and  a.yr= s.yr 
	Left Join  
	(  
		select	count (DISTINCT a.lodid ) AS rlbaaCount ,DATENAME(mm, a.created_date) AS mon,
				DATENAME(yyyy, a.created_date) AS yr 
		from	Type2 a INNER JOIN Type3 b ON a.lodid = b.lodid 
		where	isnull(a.finding,'') <> isnull(b.finding,'') 
		GROUP	BY DATENAME(mm, a.created_date), DATENAME(yyyy, a.created_date)
	) 
	t on a.mon= t.mon and  a.yr= t.yr
END
GO

