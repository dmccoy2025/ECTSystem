--exec report_sp_GetDisapprovedLod 1,null,null

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
CREATE PROCEDURE [dbo].[report_sp_GetDisapprovedLod]
(
	 @userId int,
     @beginDate datetime,
	 @endDate datetime
)
AS
BEGIN
	SET NOCOUNT ON;


 
	if (@beginDate is null)
		select @beginDate = min(created_date) from form348
	if(@endDate is null)
		SET @endDate = getDate()
	
	
	DECLARE @chain_type INT, @cs_id INT
	select @cs_id = unit_id, @chain_type = ViewType from vw_users  where userID= @userId
 
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
 
 
	INSERT	INTO	@count (ptypeId, finding)
			SELECT	b.ptype, b.finding 
			FROM	form348 a 
					INNER JOIN   form348_findings b on a.lodid = b.lodid 
					/* AND 
					( 
					b.ptype=5 and (select  id from form348_findings where  ptype=10 and lodId=a.lodId  )is null 
					or
					b.ptype=10
					)*/
					INNER JOIN vw_WorkStatus v on v.ws_id=a.status  
					LEFT JOIN (SELECT max(startDate) ReceiveDate, refId FROM core_WorkStatus_Tracking GROUP BY refId) t ON t.refId = a.lodId
			WHERE	v.isFinal =1 
					AND (v.isCancel = 0)
					AND (a.deleted=0)
					AND (b.ptype IN (5,10)) 
					AND (b.finding IN (2,4,5))
					AND (a.completed_by_unit IN (SELECT child_id FROM Command_Struct_Tree WHERE parent_id=@cs_id and view_type=@chain_type )
					AND (DATEDIFF(dd, 0, t.ReceiveDate  ) BETWEEN   DATEDIFF(dd, 0,@beginDate ) AND  DATEDIFF(dd, 0,@endDate ) )
					AND
					(
						b.ptype=5 And a.FinalDecision like '%Closed by Appointing Auth%' or FinalDecision Like '%Closed by Wing Commander%'
						or 
						b.ptype=10 And a.FinalDecision like '%Closed by Approving Auth:%' 
					) 
					AND   a.FinalFindings in (2,4,5)
					)

	INSERT	INTO	@temp(pTypeId, pName)
			select	id, roleName from core_lkupPersonnelTypes where id IN(5,10)

	UPDATE	@temp 
	SET		eptid = 2,
			eptCount = (select count(*) from @count b where b.ptypeId = t.pTypeid and b.finding  = 2),
			nIlodId = 4,
			nILodCount = (select count(*) from @count b where b.ptypeId = t.pTypeid and b.finding  = 4),
			nIlodNotId = 5,
			nIlodNotCount = (select count(*) from @count b where b.ptypeId = t.pTypeid and b.finding  = 5)  
	from	@temp t
	WHERE	t.pTypeid IN (5,10)

	select * from @temp
END
GO

