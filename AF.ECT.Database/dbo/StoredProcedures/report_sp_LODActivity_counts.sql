
CREATE procedure [dbo].[report_sp_LODActivity_counts]
	@cs_id int,
	@viewType int, 
	@beginDate datetime,
	@endDate datetime,
	@isComplete tinyint,
	@includeSubordinate bit
	
AS

--set @cs_id = 5;
--set @viewType = 7;
--set @ssn = null;
--set @beginDate = '2008-01-01';
--set @endDate = GETDATE();
--set @isComplete = 1;
--set @includeSubordinate = 1;

--begin
declare @command_level as int

set @isComplete = coalesce(@isComplete, 0);
set @command_level = @includeSubordinate + 1;

if (OBJECT_ID('tempdb..#units')) is not null
	drop table #units
	
if (OBJECT_ID('tempdb..#children')) is not null
	drop table #children
		
create table #units
(
	cs_id int,
	long_name nvarchar(100),
	Total int default 0 
 
);

 

--which cases are we including?
-- 0 : open only
-- 1 : closed only
-- 2 : all

 
	
create table #children
(
	cs_id int
);

/* 
we start by getting the top level units
if includeSubordinates is 0 this will be just the unit passed in
if includeSubordinates is 1 this will be the immediate children of the
unit that was passed in, plus the unit passed in
*/
WITH pasCTE AS (
		SELECT csc_id, csc_id_parent, cs_id, 1 AS depth
		FROM Command_Struct_Chain WHERE CS_ID = @cs_id and view_type = @viewType

	UNION ALL

	SELECT p.csc_id, p.csc_id_parent, p.cs_id, pcte.depth + 1
	FROM Command_Struct_Chain p 
	INNER JOIN pasCTE pcte ON pcte.csc_id = p.CSC_ID_PARENT
	WHERE pcte.depth <= @command_level
	AND p.view_type = @viewType
	)
	insert into #units (p.cs_id, long_name)
	select p.cs_id, c.LONG_NAME from pasCTE p
	join Command_Struct c on c.CS_ID = p.CS_ID
	where depth <= @command_level
	

declare unitCur cursor for
	select cs_id from #units
	
declare @unit_id int

open unitCur
fetch next from unitCur into @unit_id

  
while @@FETCH_STATUS = 0
begin
	delete from #children; 
	insert into #children (cs_id)
	select child_id from Command_Struct_Tree 
	where parent_id = @unit_id 
	and view_type = @viewType;
	
	--med tech
	update #units set Total = (
		select  COUNT(*)
		from Form348 f   
		 LEFT JOIN 
	     vw_workstatus vw ON  vw.ws_id=f.status   
		where   
	    (DATEDIFF(dd, 0, f.created_date ) BETWEEN   DATEDIFF(dd, 0,@beginDate ) AND  DATEDIFF(dd, 0,@endDate ) )
 	    AND 
 	    coalesce(f.completed_by_unit, f.member_unit_id)	IN (   SELECT cs_id FROM #children    )
		AND 
        (
			CASE 
			WHEN @isComplete=1 THEN    1 
			WHEN @isComplete=0 THEN    0 
			ELSE  vw.IsFinal
			END =vw.IsFinal 
		)
	)
	where cs_id = @unit_id;
	 
	fetch next from unitCur into @unit_id;
end

close unitCur;
deallocate unitCur;


select
	cs_id, long_name as name, 
	Total as Total  
From #units 
order by  Total asc

drop table #units
drop table #children
GO

