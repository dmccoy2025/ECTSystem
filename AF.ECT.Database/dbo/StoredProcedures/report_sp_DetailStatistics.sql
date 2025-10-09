
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
CREATE PROC [dbo].[report_sp_DetailStatistics]
(
	@userId int,
	@beginDate datetime,
	@endDate datetime,
	@category varchar(50)

)
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @chain_type INT, @cs_id INT, @firstPtype int, @secondPtype int
	select @cs_id = unit_id, @chain_type = ViewType from vw_users where userid = @userId

 
	if (@category = 'lodCount' )
	BEGIN
		SELECT	a.lodid, 
				a.case_id,
				a.member_name,
				a.member_unit,
				a.created_date,
				convert(char(11),   a.created_date , 101) ReceiveDate
		FROM	form348 a
				LEFT JOIN (SELECT max(startDate) ReceiveDate, refId FROM core_WorkStatus_Tracking GROUP BY refId) t ON t.refId = a.lodid
		WHERE	coalesce(a.completed_by_unit,a.member_unit_id)				 	 
				IN(SELECT child_id FROM Command_Struct_Tree WHERE parent_id=@cs_id and view_type=@chain_type ) 
				AND (DATEDIFF(dd, 0, a.created_date ) BETWEEN   DATEDIFF(dd, 0,@beginDate ) AND  DATEDIFF(dd, 0,@endDate ) )
				AND a.deleted=0	 
	END  
 
	IF (@category = 'closedWing')
	BEGIN 
		SELECT	a.lodid, 
				a.case_id,
				a.member_name,
				a.member_unit,
				a.created_date,
				convert(char(11), ISNULL(t.ReceiveDate, a.created_date), 101) ReceiveDate		
		FROM	form348 a  
				LEFT JOIN (SELECT max(startDate) ReceiveDate, refId FROM core_WorkStatus_Tracking GROUP BY refId) t ON t.refId = a.lodid
				INNER JOIN vw_WorkStatus v on v.ws_id =a.status 
		WHERE	v.isFinal =1 	 
				AND (DATEDIFF(dd, 0,  a.created_date ) BETWEEN   DATEDIFF(dd, 0,@beginDate ) AND  DATEDIFF(dd, 0,@endDate ) )  
				AND ( a.FinalDecision like '%Closed by Appointing Auth%' or FinalDecision Like '%Closed by Wing Commander%')
				AND  a.deleted=0
				AND  v.isCancel = 0
				AND  a.completed_by_unit IN (SELECT child_id FROM Command_Struct_Tree WHERE parent_id=@cs_id and view_type=@chain_type)	 
	END
	
	IF (@category = 'closedRLB')
	BEGIN 
		SELECT 	a.lodid, 
				a.case_id,
				a.member_name,
				a.member_unit,
				a.created_date,
				convert(char(11), ISNULL(t.ReceiveDate, a.created_date), 101) ReceiveDate		
		FROM	form348 a  
				LEFT JOIN (SELECT max(startDate) ReceiveDate, refId FROM core_WorkStatus_Tracking GROUP BY refId) t ON t.refId = a.lodid
				INNER JOIN vw_WorkStatus v on v.ws_id =a.status 
		WHERE	v.isFinal =1 	 
				AND(DATEDIFF(dd, 0,  a.created_date ) BETWEEN   DATEDIFF(dd, 0,@beginDate ) AND  DATEDIFF(dd, 0,@endDate ) )
				AND (a.FinalDecision like '%Closed by LOD Board%'  )
				AND a.deleted=0
				AND v.isCancel = 0
				AND	a.completed_by_unit IN (SELECT child_id FROM Command_Struct_Tree WHERE parent_id=@cs_id and view_type=@chain_type)
	END

	IF(@category = 'conflictWCRLB' or @category = 'conflictWCAA' or @category = 'conflictRLBAA')
		EXEC report_sp_DetailConflictStat @userId, @beginDate, @endDate, @category
END
GO

