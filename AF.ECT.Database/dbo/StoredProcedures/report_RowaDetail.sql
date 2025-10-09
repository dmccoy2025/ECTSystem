
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
-- Modified By:		Ken Barnett
-- Modified Date:	11/22/2016
-- Description:		Updated to no longer pull RWOA information from the RWOA
--					table as that data will rarely exist for LOD cases under
--					the original LOD workflow.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	2/15/2017
-- Description:		Updated to use the RWOA history table for cases created
--					under the new AFI.
-- ============================================================================
-- Author:		Michael van Diest
-- Create date: 12/1/2020
-- Description:	Updated to include compo.
-- ============================================================================
-- Author:		Michael van Diest
-- Create date: 12/1/2020
-- Description:	Fixed bug from last update
-- ============================================================================
CREATE PROCEDURE [dbo].[report_RowaDetail]
(
	@rwoaId INT,
	@beginDate DATETIME,
	@endDate DATETIME,
	@compo varchar(10)
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @compoNum int;

	IF (@beginDate IS NULL)
		SELECT @beginDate = MIN(created_date) FROM form348
	IF (@endDate IS NULL)
		SET @endDate = GETDATE()
	IF (@compo IS NULL)
		RAISERROR('Compo required', 11, 1);
		
	SELECT @compoNum = compo FROM core_lkupCompo WHERE abbreviation = @compo;

	SELECT	RWOADetails.lodId, 
			RWOADetails.case_id, 
			RWOADetails.member_name, 
			RWOADetails.member_unit, 
			RWOADetails.rwoa_reason, 
			RWOADetails.rwoa_explanation, 
			RWOADetails.rwoa_date,
			RWOADetails.created_date
	FROM	(
				SELECT  f.lodId, 
						f.case_id, 
						f.member_name, 
						f.member_unit, 
						b.Description AS rwoa_reason, 
						f.rwoa_explantion AS rwoa_explanation, 
						f.rwoa_date,
						f.created_date
				FROM    Form348 AS f
						JOIN vw_WorkStatus ws ON f.status = ws.ws_id
						JOIN core_lkupRWOAReasons AS b ON f.rwoa_reason = b.ID
				WHERE   f.deleted = 0  
						AND f.workflow = 1
						AND f.rwoa_reason = @rwoaId
						AND CAST(f.rwoa_date AS DATE) BETWEEN CAST(@beginDate AS DATE) AND CAST(@endDate AS DATE)
						AND f.member_compo = @compoNum

				UNION ALL
			
				SELECT	f.lodId,
						f.case_id,
						f.member_name,
						f.member_unit,
						rr.Description AS rwoa_reason,
						r.explanation_for_sending_back AS rwoa_explanation,
						r.date_sent AS rwoa_date,
						f.created_date
				FROM	Form348 AS f
						JOIN vw_WorkStatus ws ON f.status = ws.ws_id
						JOIN Rwoa r ON f.lodId = r.refId AND r.workflow = f.workflow
						JOIN core_lkupRWOAReasons rr ON r.reason_sent_back = rr.ID
				WHERE	f.deleted = 0
						AND f.workflow <> 1
						AND rr.ID = @rwoaId
						AND CAST(r.date_sent AS DATE) BETWEEN CAST(@beginDate AS DATE) AND CAST(@endDate AS DATE)
						AND f.member_compo = @compoNum
			) AS RWOADetails
	ORDER	BY RWOADetails.case_id

END
GO

