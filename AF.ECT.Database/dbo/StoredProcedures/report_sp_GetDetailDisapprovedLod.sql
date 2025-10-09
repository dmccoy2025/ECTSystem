
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
CREATE PROCEDURE [dbo].[report_sp_GetDetailDisapprovedLod]
(
	 @userId int,
     @beginDate datetime,
	 @endDate datetime,
	 @ptype int,
	 @finding int
)
AS
BEGIN
	SET NOCOUNT ON;
	
	if (@beginDate is null)
		select @beginDate = min(created_date) from form348
	if(@endDate is null)
		SET @endDate = getDate()

	DECLARE @chain_type INT, @cs_id INT
	select @cs_id = unit_id, @chain_type = ViewType from vw_users where userID=@userId

	DECLARE @child_units TABLE 
	(
		child_id int  
	)
 
	INSERT	INTO	@child_units(child_id)	
			SELECT	child_id 
			FROM	Command_Struct_Tree 
			WHERE	parent_id=@cs_id and view_type=@chain_type 
	


	SELECT 	a.lodid, 
			a.case_id,
			a.member_name,
			a.member_unit,
			convert(char(11),   a.created_date , 101) created_date
			,m.event_nature_type 
	FROM	form348 a
			INNER JOIN vw_WorkStatus v on v.ws_id=a.status  
			INNER JOIN  form348_findings b on a.lodid = b.lodid 
			LEFT JOIN 
			(SELECT max(startDate) ReceiveDate, refId FROM core_WorkStatus_Tracking GROUP BY refId) t ON t.refId = a.lodId

			/*  AND 
			( 
			@ptype=5 and (select  id from form348_findings where  ptype=10 and lodId=a.lodId  )is null
			or
			@ptype=10
			)*/
			INNER JOIN  form348_medical m  on m.lodid = a.lodid 
	WHERE	v.isFinal =1 
			AND (b.ptype = @ptype) 
			AND (a.deleted=0)
			AND (v.isCancel = 0)
			AND (b.finding = @finding)
			AND a.completed_by_unit  IN( SELECT child_id FROM @child_units  )		 
			AND a.FinalFindings in (2,4,5)
			AND	(DATEDIFF(dd, 0, t.ReceiveDate ) BETWEEN   DATEDIFF(dd, 0,@beginDate ) AND  DATEDIFF(dd, 0,@endDate ) )
			AND (
			@ptype=5 And ( a.FinalDecision like '%Closed by Appointing Auth%' or FinalDecision Like '%Closed by Wing Commander%' )
			or 
			@ptype=10 And a.FinalDecision like '%Closed by Approving Auth:%'   )
END
GO

