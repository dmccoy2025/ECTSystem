
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 3/28/2016
-- Description:	Gets all of the PH cases associated with the specified wing RMU
--				Id.
-- ============================================================================
CREATE PROCEDURE [dbo].[PH_Workflow_sp_GetCasesByWingRMU]
	@wingRMUId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@wingRMUId, 0) = 0)
	BEGIN
		RETURN
	END
	
	DECLARE @phModuleId INT = 0
	
	SELECT	@phModuleId = m.moduleId
	FROM	core_lkupModule m
	WHERE	m.moduleName = 'PH Non-Clinical Tracking'
	
	IF (ISNULL(@phModuleId, 0) = 0)
	BEGIN
		RETURN
	END
	
	SELECT	s.SC_Id AS [RefId], 
			s.case_id AS [Case_Id], 
			s.Module_Id,
			sc.description AS [Status], 
			s.created_date AS [Created_Date],
			CONVERT(char(11), comp.CompletedDate, 101) AS [Completed_Date],
			DATENAME(MONTH, s.ph_reporting_period) + ' ' + DATENAME(YEAR, s.ph_reporting_period) AS [Reporting_Period]
	FROM	Form348_SC s
			JOIN core_WorkStatus ws ON s.status = ws.ws_id
			JOIN core_StatusCodes sc ON ws.statusId = sc.statusId
			LEFT OUTER JOIN (
				SELECT	MAX(startDate) AS ReceiveDate, ws_id, refId
				FROM	dbo.core_WorkStatus_Tracking
				WHERE	module = @phModuleId
				GROUP BY ws_id, refId
			) AS track ON track.refId = s.SC_Id AND track.ws_id = s.status
			LEFT OUTER JOIN (
				SELECT	refId, MAX(endDate) AS CompletedDate
				FROM	dbo.core_WorkStatus_Tracking AS core_WorkStatus_Tracking_1 INNER JOIN
						dbo.vw_WorkStatus ws on core_WorkStatus_Tracking_1.ws_id = ws.ws_id
				WHERE	module = @phModuleId AND ws.isFinal = 1 AND ws.description NOT LIKE '%Cancel%'
				GROUP BY refId
			) AS comp ON comp.refId = s.SC_Id
	WHERE	s.Module_Id = @phModuleId
			AND s.ph_wing_rmu_id = @wingRMUId
	ORDER	BY s.ph_reporting_period DESC
END
GO

