
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
CREATE procedure [dbo].[report_sp_LODActivity]
(
	 @cs_id int,
	 @view int,
	 @beginDate datetime,
	 @endDate datetime ,
	 @isComplete tinyint 	 
)
AS
BEGIN
	SET NOCOUNT ON;
 
	if (@beginDate is null)
		select @beginDate = min(created_date) from form348
	if(@endDate is null)
		SET @endDate = getDate()

    SELECT	a.lodid, a.case_id, a.member_name 'Name', RIGHT(a.member_ssn, 4) 'SSN'
			,a.member_unit, a.member_unit_id as cs_id,
			(vw.description) as description
    FROM	form348 a
			LEFT JOIN vw_workstatus vw ON  vw.ws_id=a.status 
	WHERE	coalesce(a.completed_by_unit, a.member_unit_id) in (SELECT child_id FROM Command_Struct_Tree WHERE parent_id=@cs_id and view_type=@view )
			AND	( DATEDIFF(dd, 0, a.created_date ) BETWEEN   DATEDIFF(dd, 0,@beginDate ) AND  DATEDIFF(dd, 0,@endDate ) )
			AND 
			(
								CASE 
								WHEN @isComplete=1 THEN    1 
								WHEN @isComplete=0 THEN    0 
								ELSE  vw.IsFinal
								END =vw.IsFinal 
				)
			 AND 	vw.isCancel = 0   --Exclude Cancelled cases
			 AND 	a.deleted=0   --Exclude deleted cases
END
GO

