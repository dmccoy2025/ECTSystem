--execute report_sp_GetPhsicianCanceledLOD 0,1,7,null,null

-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	11/27/2015
-- Work Item:		TFS Task 289
-- Description:		Changed the size of case_id from 20 to 50.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	7/29/2016
-- Description:		Updated to no longer use a hard coded value for the LOD
--					cancel status.
-- ============================================================================
CREATE PROCEDURE [dbo].[report_sp_GetPhsicianCanceledLOD]
(
	@cs_id INT,
	@viewType int,
	@ssn varchar(10),
	@beginDate datetime,
	@endDate datetime,
	@includeSubordinate bit
)
AS
BEGIN
	SET NOCOUNT ON;

	if (@ssn = '')
		SET @ssn = null;
	if (@beginDate is null)
		select @beginDate = min(created_date) from form348
	if(@endDate is null)
		SET @endDate = getDate()

	DECLARE @temp Table
	(
		lodid int,
		case_id varchar(50),
		ssn varchar(4),
		member_name nvarchar(100),
		member_unit_id int,
		member_unit varchar(100),
		reason varchar(4000),
		status tinyint	,
		completed_by_unit int,
		birthmonth varchar(40) 
	)

	INSERT	INTO	@temp
			SELECT	a.lodId,    
					a.case_id,  
					Right(a.member_ssn,4) as ssn,
					a.member_name, 
					a.member_unit_id int,
					a.member_unit, 
					c.Description + ' : ' + b.physician_cancel_explanation as CancelReason ,
					a.status,
					completed_by_unit,
					datename(mm, a.member_DOB)
			FROM	Form348 AS a 
					INNER JOIN Form348_Medical AS b ON a.lodId = b.lodid 
					INNER JOIN vw_WorkStatus vw ON a.status = vw.ws_id
					INNER JOIN core_lkupCancelReason AS c ON b.physician_cancel_reason = c.Id
					LEFT JOIN (select refid, MAX(enddate) CompletedDate from core_WorkStatus_Tracking group by refId) comp on comp.refId = a.lodId
			WHERE   (vw.isCancel = 1) 
					AND 
					(
						b.physician_cancel_reason is not null 
						and 
						b.physician_cancel_reason <> 0
					) 
					AND a.deleted=0 	 
					AND a.completed_by_unit IN ( SELECT child_id FROM Command_Struct_Tree t 	WHERE parent_id = @cs_id AND view_type = @viewType) 
					AND (DATEDIFF(dd, 0, comp.CompletedDate ) BETWEEN   DATEDIFF(dd, 0,@beginDate ) AND  DATEDIFF(dd, 0,@endDate ) )
					AND	a.member_ssn = IsNull(@ssn,a.member_ssn) 

 		
	SELECT  * 
	FROM	@temp 
	WHERE	CASE 
				WHEN	@includeSubordinate = 0   THEN    completed_by_unit
				ELSE @cs_id 
			END =@cs_id
END
GO

