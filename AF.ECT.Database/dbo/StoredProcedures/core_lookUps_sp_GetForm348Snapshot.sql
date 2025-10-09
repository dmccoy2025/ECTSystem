
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 8/16/2017
-- Description:	Returns a list of meta data for 348 document generated in a
--				specified time period.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookUps_sp_GetForm348Snapshot]
	@beginDate DATETIME,
	@endDate DATETIME
AS
BEGIN
	IF (@beginDate IS NULL)
		SET @beginDate = GETDATE()

	IF (@endDate IS NULL)
		SET @endDate = GETDATE()

	SELECT	
			A.lodId,
			A.case_id, 
			A.doc_group_id, 
			G.DateCreated, 
			G.DateLastModified, 
			D.OriginalFileName 
	FROM 
			[ALOD].[dbo].[Form348] A 
			INNER JOIN [ALOD].[dbo].[vw_WorkStatus] ws ON A.status = ws.ws_id
			INNER JOIN [SRXLite].[dbo].[DX_Groups] G ON A.doc_group_id = G.GroupID 
			INNER JOIN [SRXLite].[dbo].[DX_GroupDocuments] GD ON GD.GroupID = A.doc_group_id 
			INNER JOIN [SRXLite].[dbo].[DX_Documents] D ON GD.DocID = D.DocID 
	WHERE 
			G.DateLastModified BETWEEN @beginDate AND @endDate
			AND ws.isFinal = 1
			AND ws.isCancel = 0
			AND D.DocTypeID = 74
	ORDER BY 
			G.DateLastModified DESC
END
GO

