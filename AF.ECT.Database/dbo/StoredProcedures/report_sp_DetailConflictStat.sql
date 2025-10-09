--completed_by_unit is being checked 

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
CREATE PROC [dbo].[report_sp_DetailConflictStat]
(
	@userId int,
	@beginDate datetime,
	@endDate datetime,
	@category varchar(50)

)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @wc int, @aa int, @rlb int
	DECLARE @chain_type INT, @cs_id INT, @firstPtype int, @secondPtype int
	select @cs_id = unit_id, @chain_type = ViewType from vw_users where userid = @userId

 

	SET @wc = 5;
	SET @aa = 10;
	SET @rlb = 6;

    IF (@category = 'conflictWCAA')
		BEGIN
			SET @firstPtype = @wc;
			SET @secondPtype = @aa;
		END

	if (@category = 'conflictWCRLB')
		BEGIN
			SET @firstPtype = @wc;
			SET @secondPtype = @rlb;
		END

	if (@category = 'conflictRLBAA')
	    BEGIN
			SET @firstPtype = @rlb;
			SET @secondPtype = @aa;
		END

	 
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

	INSERT	INTO	@temp1(lodid,finding)
			SELECT  DISTINCT a.lodid, b.finding
			FROM	form348 a 
					INNER JOIN vw_WorkStatus v on v.ws_id =a.status 
					INNER JOIN form348_findings b on a.lodid = b.lodid 
			WHERE	v.isFinal = 1
					AND v.isCancel = 0
					AND a.deleted=0
					AND b.ptype = @firstPtype
					AND (DATEDIFF(dd, 0, a.created_date ) BETWEEN   DATEDIFF(dd, 0,@beginDate ) AND  DATEDIFF(dd, 0,@endDate ) )
					AND (
							@firstPtype=5 and b.finding in ( 1,2,3,4,5,6)
							or 
							@firstPtype=10 and b.finding in ( 1,2,3,4,5,6) 
							or 
							@firstPtype=6 and b.finding in ( 1,3) 
     
						 ) 
					AND a.completed_by_unit IN(SELECT child_id FROM Command_Struct_Tree WHERE parent_id=@cs_id and view_type=@chain_type 
					)
			ORDER	BY a.lodid

	INSERT	INTO	@temp2(lodid,finding)
			SELECT  DISTINCT a.lodid, b.finding
			FROM	form348 a 
					INNER JOIN vw_WorkStatus v on v.ws_id =a.status 
					INNER JOIN  form348_findings b on a.lodid = b.lodid 
			WHERE	v.isFinal =1 	
					AND v.isCancel = 0
					AND (b.ptype = @secondPtype)
					AND a.deleted=0
					AND (DATEDIFF(dd, 0, a.created_date ) BETWEEN   DATEDIFF(dd, 0,@beginDate ) AND  DATEDIFF(dd, 0,@endDate ) ) 
					AND 
						 (
							@secondPtype=5 and b.finding in ( 1,2,3,4,5,6)
							or 
							@secondPtype=10 and b.finding in ( 1,2,3,4,5,6) 
							or 
							@secondPtype=6 and b.finding in ( 1,3) 
     
						 ) 
					AND a.completed_by_unit IN(SELECT child_id FROM Command_Struct_Tree WHERE parent_id=@cs_id and view_type=@chain_type 
					)
			ORDER	BY a.lodid

	declare @i int

	 
	SELECT  DISTINCT a.lodid,
			c.case_id,
			c.member_name,
			c.member_unit,
			c.created_date,
			fndA.Description  findingA,
			fndB.Description  findingB,
			convert(char(11), ISNULL(t.ReceiveDate, c.created_date), 101) ReceiveDate
	FROM	@temp1 a INNER JOIN @temp2 b ON a.lodid = b.lodid 
			INNER JOIN form348 c on c.lodid=a.lodid 
			LEFT JOIN core_lkUpFindings fndA on fndA.Id=a.finding 
			LEFT JOIN core_lkUpFindings fndB on fndB.Id=b.finding 
			LEFT JOIN (SELECT max(startDate) ReceiveDate, refId FROM core_WorkStatus_Tracking GROUP BY refId) t ON t.refId = c.lodid
	WHERE	isnull(a.finding,'') <> isnull(b.finding,'')
END
GO

