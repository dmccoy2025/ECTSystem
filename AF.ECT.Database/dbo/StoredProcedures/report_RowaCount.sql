
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	7/29/2016
-- Description:		Updated to no longer use a hard coded value for the LOD
--					cancel status.
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
-- Create date: 3/3/2021
-- Description:	Fixed bug from last update.
-- ============================================================================
CREATE PROCEDURE [dbo].[report_RowaCount]
(
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


	SELECT	RWOAs.ID, RWOAs.Description, SUM(RWOAs.TotalCount) AS TotalCount
	FROM	(
				-- Becuase RWOAs weren't implemented correctly for the original LOD workflow, then only get the most recent RWOA information from those cases...
				SELECT	ID, 
						Description,
						(
							SELECT	COUNT(*) AS TotalCount
							FROM	Form348 AS b
									JOIN vw_WorkStatus ws ON b.status = ws.ws_id
							WHERE	b.workflow = 1
									AND b.rwoa_reason = a.ID
									AND b.deleted = 0
									AND CAST(b.rwoa_date AS DATE) BETWEEN CAST(@beginDate AS DATE) AND CAST(@endDate AS DATE)
									AND b.member_compo = @compoNum
						)
						AS TotalCount  
				FROM	core_lkupRWOAReasons AS a

				UNION ALL

				-- Search the RWOA history for cases not created under the original LOD workflow...
				SELECT	a.ID, 
						a.Description,
						(
							SELECT	COUNT(*) AS TotalCount
							FROM	Rwoa r
									JOIN vw_WorkStatus ws ON r.workstatus = ws.ws_id
									JOIN Form348 b ON r.refId = b.lodId
							WHERE	ws.moduleId = 2		-- LOD module
									AND r.workflow <> 1	-- A non original LOD workflow (RWOAs didn't work properly for that workflow)
									AND r.reason_sent_back = a.ID
									AND CAST(r.date_sent AS DATE) BETWEEN CAST(@beginDate AS DATE) AND CAST(@endDate AS DATE)
									AND b.member_compo = @compoNum
						)
						AS TotalCount
				FROM	core_lkupRWOAReasons AS a
			) RWOAs
	GROUP	BY RWOAs.ID, RWOAs.Description
END
GO

