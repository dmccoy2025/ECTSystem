
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 4/20/2016
-- Description:	Returns the most recently created PH case for the specified
--				reporting period. 
-- ============================================================================
CREATE PROCEDURE [dbo].[PH_Workflow_sp_GetCaseIdForReportingPeriod]
	@reportingPeriod DATETIME,
	@wingRMUId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @phModuleId INT = 0
	
	SELECT	@phModuleId = m.moduleId
	FROM	core_lkupModule m
	WHERE	m.moduleName = 'PH Non-Clinical Tracking'
	
	SELECT	TOP 1 s.SC_Id
	FROM	Form348_sc s
			JOIN core_WorkStatus ws ON s.status = ws.ws_id
			JOIN core_StatusCodes sc ON ws.statusId = sc.statusId
	WHERE	s.Module_Id = @phModuleId
			AND s.ph_wing_rmu_id = @wingRMUId
			AND YEAR(s.ph_reporting_period) = YEAR(@reportingPeriod)
			AND MONTH(s.ph_reporting_period) = MONTH(@reportingPeriod)
			AND sc.isCancel = 0
	ORDER	BY s.created_date DESC
END
GO

