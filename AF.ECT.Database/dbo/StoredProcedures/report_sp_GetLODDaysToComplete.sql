
-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	7/29/2016
-- Description:		Updated to no longer use a hard coded values for the LOD
--					statuses.
-- ============================================================================
CREATE proc [dbo].[report_sp_GetLODDaysToComplete]
(
	@pbeginDate datetime,
	@pendDate datetime,
	@pisFormal bit 
)
AS
BEGIN
	CREATE TABLE #temp
	(
		lodId int,
		DaysToComplete int
	)
	
	if (@pbeginDate is null)
		select @pbeginDate = min(created_date) from form348
	if(@pendDate is null)
		SET @pendDate = getDate()
		
	if (@pbeginDate is null)
		select @pbeginDate = min(created_date) from form348
	if(@pendDate is null)
		SET @pendDate = getDate()


	SELECT	lodid, MIN(DATEDIFF(day, creation.actionDate, act.actionDate)) As daysToComplete
	FROM	dbo.Form348 AS lod 
			Inner Join dbo.core_LogAction AS act ON act.referenceId = lodId 
			JOIN vw_WorkStatus AS actWS ON act.newStatus = actWS.ws_id
			INNER JOIN dbo.core_LogAction AS creation ON creation.referenceId = lod.lodId
			JOIN vw_WorkStatus AS creationWS ON creation.status = creationWS.ws_id
			JOIN vw_WorkStatus AS creationNewWS ON creation.newStatus = creationNewWS.ws_id
	WHERE	act.moduleId = 2 
			AND (actWS.isFinal = 1 AND actWS.isCancel = 0)	-- act.newstatus = 13	-- Completed Status
			AND act.actionid = 15 
			AND formal_inv = @pisFormal 
			AND act.actionDate >= @pbeginDate 
			AND act.actionDate <= @pendDate 
			AND creationNewWS.description = 'Medical Officer Review' --creation.newStatus= 2	-- Medical Officer Review
			AND creationWS.description = 'Medical Technician Input' -- creation.status = 1		-- Medical Technician Input
			AND creation.actionId = 15
	GROUP	BY lodid

	INSERT	INTO	#temp (lodId, DaysToComplete)
			SELECT	lodid, MIN(DATEDIFF(day, creation.actionDate, act.actionDate)) As daysToComplete
			FROM	dbo.Form348 AS lod 
					Inner Join dbo.core_LogAction AS act ON act.referenceId = lodId 
					JOIN vw_WorkStatus AS actWS ON act.newStatus = actWS.ws_id
					INNER JOIN dbo.core_LogAction AS creation ON creation.referenceId = lod.lodId
					JOIN vw_WorkStatus AS creationWS ON creation.status = creationWS.ws_id
					JOIN vw_WorkStatus AS creationNewWS ON creation.newStatus = creationNewWS.ws_id
			WHERE	act.moduleId = 2 
					AND (actWS.isFinal = 1 AND actWS.isCancel = 0)	-- act.newstatus = 13	-- Completed Status
					AND act.actionid = 15 
					AND formal_inv = @pisFormal 
					AND act.actionDate >= @pbeginDate 
					AND act.actionDate <= @pendDate 
					AND creationNewWS.description = 'Medical Officer Review' --creation.newStatus= 2	-- Medical Officer Review
					AND creationWS.description = 'Medical Technician Input' -- creation.status = 1		-- Medical Technician Input
					AND creation.actionId = 15
			GROUP	BY lodid 

	SELECT	MAX(DaysToComplete) AS maxNumber
	FROM	#temp
END
GO

