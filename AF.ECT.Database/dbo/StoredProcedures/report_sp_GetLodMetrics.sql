
-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	8/1/2016
-- Description:		Updated to use Status Codes instead of Work Statuses.
-- ============================================================================
CREATE procedure [dbo].[report_sp_GetLodMetrics]
	@cs_id int,
	@viewType int,
	@ssn varchar(10),
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
	med_tech decimal(6,1) default 0,
	med_off decimal(6,1) default 0,
	unit_cmdr decimal(6,1) default 0,
	wing_ja decimal(6,1) default 0,
	wing_cc decimal(6,1) default 0,
	board decimal(6,1) default 0,
	invest decimal(6,1) default 0,
	hqaa decimal(6,1) default 0 
);



declare @complete table
(
	ws_id int
);

--which cases are we including?
-- 0 : open only
-- 1 : closed only
-- 2 : all

if (@isComplete = 0)
	insert into @complete
	select ws_id from vw_WorkStatus where isFinal = 0
else if (@isComplete = 1)
	insert into @complete
	select ws_id from vw_WorkStatus where isFinal = 1
else
	insert into @complete
	select ws_id from vw_WorkStatus
	
	
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
	select p.cs_id, c.LONG_NAME + ' (' + c.PAS_CODE + ')' from pasCTE p
	join Command_Struct c on c.CS_ID = p.CS_ID
	where depth <= @command_level
	

declare unitCur cursor for
	select cs_id from #units
	
declare @unit_id int

open unitCur
fetch next from unitCur into @unit_id

/*
med_tech: 1
med_off : 2
unit_cmdr: 3
wing_ja: 4
wing_cc: 5
invest: 11
board: 8, 19
*/


while @@FETCH_STATUS = 0
begin
	delete from #children;
	
	insert into #children (cs_id)
	select child_id from Command_Struct_Tree 
	where parent_id = @unit_id 
	and view_type = @viewType;
	
	--med tech
	update	#units 
	set		med_tech = 
			(
				select	AVG(datediff(d, startDate, endDate)) 
				from	core_WorkStatus_Tracking a
						join vw_WorkStatus vws ON a.ws_id = vws.ws_id
						join Form348 f on f.lodId = a.refId
						join #children c on c.cs_id = coalesce(f.completed_by_unit, f.member_unit_id)
						join @complete stat on stat.ws_id = f.status
				where	vws.statusId = 1	--a.ws_id = 1
						and a.endDate between @beginDate and @endDate
						and f.deleted = 0	    
			)
	where	cs_id = @unit_id;
	
	--med officer
	update	#units 
	set		med_off = 
			(
				select	AVG(datediff(d, startDate, endDate)) 
				from	core_WorkStatus_Tracking a
						join vw_WorkStatus vws ON a.ws_id = vws.ws_id
						join Form348 f on f.lodId = a.refId
						join #children c on c.cs_id = coalesce(f.completed_by_unit, f.member_unit_id)
						join @complete stat on stat.ws_id = f.status
				where	vws.statusId = 3	-- a.ws_id = 2
						and a.endDate between @beginDate and @endDate
						and f.deleted = 0
			)
	where	cs_id = @unit_id;
	
	--unit cmdr
	update	#units 
	set		unit_cmdr = 
			(
				select	AVG(datediff(d, startDate, endDate)) 
				from	core_WorkStatus_Tracking a
						join vw_WorkStatus vws ON a.ws_id = vws.ws_id
						join Form348 f on f.lodId = a.refId
						join #children c on c.cs_id = coalesce(f.completed_by_unit, f.member_unit_id)
						join @complete stat on stat.ws_id = f.status
				where	vws.statusId = 4	-- a.ws_id = 3
						and a.endDate between @beginDate and @endDate
						and f.deleted = 0
			)
	where	cs_id = @unit_id;
	
	--wing ja
	update	#units 
	set		wing_ja = 
			(
				select	AVG(datediff(d, startDate, endDate)) 
				from	core_WorkStatus_Tracking a
						join vw_WorkStatus vws ON a.ws_id = vws.ws_id
						join Form348 f on f.lodId = a.refId
						join #children c on c.cs_id = coalesce(f.completed_by_unit, f.member_unit_id)
						join @complete stat on stat.ws_id = f.status
				where	vws.statusId = 5	--a.ws_id = 4
						and a.endDate between @beginDate and @endDate
						and f.deleted = 0
			)
	where	cs_id = @unit_id;
	
	--wing cc
	update	#units 
	set		wing_cc = 
			(
				select	AVG(datediff(d, startDate, endDate)) 
				from	core_WorkStatus_Tracking a
						join vw_WorkStatus vws ON a.ws_id = vws.ws_id
						join Form348 f on f.lodId = a.refId
						join #children c on c.cs_id = coalesce(f.completed_by_unit, f.member_unit_id)
						join @complete stat on stat.ws_id = f.status
				where	vws.statusId = 6	-- a.ws_id = 5
						and a.endDate between @beginDate and @endDate
						and f.deleted = 0
			)
	where	cs_id = @unit_id;
	
	--investigation
	update	#units 
	set		invest = 
			(
				select	AVG(datediff(d, startDate, endDate)) 
				from	core_WorkStatus_Tracking a
						join vw_WorkStatus vws ON a.ws_id = vws.ws_id
						join Form348 f on f.lodId = a.refId
						join #children c on c.cs_id = coalesce(f.completed_by_unit, f.member_unit_id)
						join @complete stat on stat.ws_id = f.status
				where	vws.statusId = 8	-- a.ws_id = 11
						and a.endDate between @beginDate and @endDate
						and f.deleted = 0
			)
	where	cs_id = @unit_id;
	
	--board
	update	#units 
	set		board = 
			(
				select	AVG(datediff(d, startDate, endDate)) 
				from	(
							select wst.refId, min(wst.startDate) as startDate, max(wst.endDate) as endDate 
							from	core_WorkStatus_Tracking wst
									join vw_WorkStatus vws ON wst.ws_id = vws.ws_id
							where	vws.statusId IN (11, 12, 13, 20, 21, 22, 169, 170)	-- wst.ws_id in (8, 10, 16,   18, 19, 20 )
									and wst.endDate between @beginDate and @endDate
							group	by wst.refId
						) a
						join Form348 f on f.lodId = a.refId
						join #children c on c.cs_id = coalesce(f.completed_by_unit, f.member_unit_id)
						join @complete stat on stat.ws_id = f.status and f.deleted = 0
			)
	where	cs_id = @unit_id;
	
	--board aa
	update	#units 
	set		hqaa = 
			(
				select	AVG(datediff(d, startDate, endDate)) 
				from	core_WorkStatus_Tracking a
						join vw_WorkStatus vws ON a.ws_id = vws.ws_id
						join Form348 f on f.lodId = a.refId
						join #children c on c.cs_id = coalesce(f.completed_by_unit, f.member_unit_id)
						join @complete stat on stat.ws_id = f.status
				where	vws.statusId IN (15, 24)	-- a.ws_id in (17,22)
						and a.endDate between @beginDate and @endDate
						and f.deleted = 0
			)
	where	cs_id = @unit_id;
		
		
	fetch next from unitCur into @unit_id;
end

close unitCur;
deallocate unitCur;


select
	cs_id, long_name, 
	coalesce(med_tech,0.0) as med_tech,
	coalesce(med_off,0.0) as med_off,
	coalesce(unit_cmdr,0.0) as unit_cmdr,
	coalesce(wing_ja,0.0) as wing_ja,
	coalesce(wing_cc,0.0) as wing_cc,
	coalesce(board,0.0) as board,
	coalesce(invest,0.0) as invest,
	coalesce(hqaa,0.0) as hqaa	
From #units
order by long_name

drop table #units
drop table #children
GO

