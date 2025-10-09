/*
	Modified --Exclude cancelled cases
			 --Include birth month in select field
		20131216
			-- made completed_by_unit and member_unit separate conditions rather than using coalesce
			-- ignore date if ssn is provided
*/
-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	4/8/2015
-- Work Item:		TFS Bug 295
-- Description:		Added the "FinalFindings" column to the @temp table, and
--					selected the "FinalFindings" column in the insert select
--					statement. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	11/27/2015
-- Work Item:		TFS Task 289
-- Description:		1) Changed the size of case_id from 20 to 50.
--					2) Made it so only formal cases are selected.
--					3) Made it so the results are ordered by the case ID
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	1/8/2016
-- Work Item:		TFS Task 289
-- Description:		Modified the stored procedure to use 0 for the Final 
--					Findings field if the value is NULL. 
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	7/20/2016
-- Description:		Formal and informal cases are returned to be able to
--					start an appeal. Also return the value if the case is 
--					formal or informal 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	8/1/2016
-- Description:		Updated to no longer use a hard coded value for the LOD
--					cancel status.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	1/18/2017
-- Description:		Modified to no longer select LODs that are marked as
--					restricted SARC cases.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	6/7/2017
-- Description:		- Procedure now takes into account the member's current
--					unit when comparing against the unit of the user executing
--					the prodecure.
-- ============================================================================
-- Modified By:		Eric Kelley
-- Modified Date:	4/27/2021
-- Description:		- Updated Temp Table "Status" from TinyINT to INT.
-- ============================================================================
CREATE PROC [dbo].[report_sp_GetDisposition]
(
	@cs_id INT,
	@ssn VARCHAR(10),
	@view INT,
	@beginDate DATETIME,
	@endDate DATETIME,
	@isComplete TINYINT,
	@includeSubordinate BIT
)
AS
BEGIN
	SET NOCOUNT ON;
	
	IF (@ssn = '')
		SET @ssn = null;
	IF (@beginDate is null or @ssn is not null) -- if ssn is provided, then ignore date
		SELECT @beginDate = MIN(created_date) FROM form348
	IF(@endDate is null or @ssn is not null) -- if ssn is provided, then ignore date
		SET @endDate = GETDATE()

	DECLARE @temp Table
	(
		lodid INT,
		case_id VARCHAR(50),
		LastName NVARCHAR(100),
		ssn VARCHAR(4),
		member_unit_id INT,
		unit VARCHAR(100),
		created_date DATETIME,
		status INT,
		isFinal BIT,
		FinalFindings TINYINT,
		completed_by_unit INT,
		birthmonth VARCHAR(40),
		birthmonthNum INT,
		isFormal BIT,
		IsPostProcessingComplete BIT,
		caseUnitId INT,
		memberCurrentUnitId INT
	)
 
	INSERT	INTO	@temp
			SELECT	a.lodid, 
					a.case_id,
					a.member_name,
					Right(a.member_ssn,4) AS ssn,
					a.member_unit_id,
					a.member_unit,
					a.created_date,
					a.status  ,
					vw.IsFinal,
					ISNULL(a.FinalFindings, 0),
					a.completed_by_unit,
					datename(mm, a.member_DOB),
					datepart(mm, a.member_DOB),
					a.formal_inv,
					a.is_post_processing_complete,
					a.member_unit_id,
					mCS.CS_ID
			FROM	form348 a
					LEFT JOIN vw_workstatus vw ON vw.ws_id = a.status
					LEFT JOIN MemberData md ON a.member_ssn = md.SSAN
					LEFT JOIN Command_Struct mCS ON md.PAS_NUMBER = mCS.PAS_CODE
			WHERE	(
						CASE a.is_post_processing_complete 
							WHEN 1 THEN mCS.CS_ID 
							ELSE a.member_unit_id 
						END IN (SELECT child_id FROM Command_Struct_Tree WHERE parent_id=@cs_id AND view_type=@view) 
					)
					AND
					(
						CASE 
							WHEN @isComplete = 1 THEN 1 
							WHEN @isComplete = 0 THEN 0 
							ELSE vw.IsFinal
						END = vw.IsFinal 
					)
					AND (DATEDIFF(dd, 0, a.created_date) BETWEEN DATEDIFF(dd, 0,@beginDate) AND DATEDIFF(dd, 0,@endDate ))
					AND a.member_ssn = IsNull(@ssn,a.member_ssn) 
					AND vw.isCancel = 0
					AND a.deleted = 0
					AND 
					(
						CASE
							WHEN a.sarc = 1 THEN a.restricted
							ELSE 0
						END = 0
					)
		
	IF (@includeSubordinate = 0)
		SELECT  * 
		FROM  @temp 
		WHERE CASE IsPostProcessingComplete WHEN 1 THEN memberCurrentUnitId ELSE caseUnitId END = @cs_id
		ORDER BY case_id
	ELSE
		SELECT *
		FROM @Temp
		ORDER BY case_id
END
GO

