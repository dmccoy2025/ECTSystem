--exec report_OpenStatusDetail 20151553,7,'6/2/2009','7/21/2009'

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
CREATE PROCEDURE [dbo].[report_OpenStatusDetail]
(
	@csId INT,
	@view INT,
	@beginDate DATETIME,
	@endDate DATETIME
)
AS
BEGIN
	SET NOCOUNT ON;
 
	IF (@beginDate IS NULL)
	   SELECT @beginDate = MIN(created_date) FROM form348
	IF(@endDate IS NULL)
		SET @endDate = GETDATE()

	SELECT  a.lodId, 
			a.case_id, 
			a.member_name, 
			b.Description AS description, 	 
			a.member_unit, 
			a.created_date,
			RIGHT(a.member_ssn, 4) 'SSN'
	FROM    Form348 AS a 
			LEFT OUTER JOIN vw_WorkStatus AS b ON  a.status = b.ws_id 
	WHERE	a.deleted = 0	
			AND DATEDIFF(dd, 0, a.created_date ) BETWEEN DATEDIFF(dd, 0,@beginDate ) AND  DATEDIFF(dd, 0,@endDate ) 
			AND a.member_unit_id =@csId  
			AND b.IsFinal = 0
			AND b.isCancel = 0
END
GO

