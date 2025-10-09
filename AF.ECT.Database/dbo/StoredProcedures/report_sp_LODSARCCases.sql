
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 10/24/2016
-- Description:	Executes the LOD SARC Cases canned report.
--
--		Restriction Status:
--			1 = ALL SARC cases
--			2 = Unrestricted SARC cases only
--			3 = Restricted SARC cases only
--
--		Completion Status:
--			1 = Both In-Progress and Completed cases
--			2 = In-Progress cases only
--			3 = Completed cases only
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	10/12/2017
-- Description:		o Now selects the module Id of a case in the results
-- ============================================================================
CREATE PROCEDURE [dbo].[report_sp_LODSARCCases]
	@beginDate DATE,
	@endDate DATE,
	@restrictionStatus INT,
	@completionStatus INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- VALIDATE INPUT --
	IF (ISNULL(@restrictionStatus, 0) = 0)
	BEGIN
		RETURN
	END

	IF (NOT (@restrictionStatus BETWEEN 1 AND 3))
	BEGIN
		RETURN
	END

	IF (ISNULL(@completionStatus, 0) = 0)
	BEGIN
		RETURN
	END

	IF (NOT (@completionStatus BETWEEN 1 AND 3))
	BEGIN
		RETURN
	END

	IF (@beginDate IS NULL AND (@restrictionStatus = 1 OR @restrictionStatus = 2))
	BEGIN
		SELECT @beginDate = MIN(created_date) FROM Form348 WHERE sarc = 1
	END

	IF (@beginDate IS NULL AND @restrictionStatus = 3)
	BEGIN
		SELECT @beginDate = MIN(created_date) FROM Form348 WHERE sarc = 1 AND restricted = 1
	END

	IF(@endDate IS NULL)
	BEGIN
		SET @endDate = GETDATE()
	END
	

	-- DECLARE VARIABLES --
	DECLARE @Results TABLE
	(
		RefId INT,
		ModuleId TINYINT,
		WorkflowId TINYINT,
		CaseId NVARCHAR(100),
		IsRestricted BIT,
		IsComplete BIT,
		WorkStatusId INT,
		StatusCodeName NVARCHAR(50),
		MemberUnit NVARCHAR(100),
		MemberUnitId INT,
		MemberName NVARCHAR(100),
		MemberSSN NVARCHAR(9)
	)


	-- SELECT DATA --

	-- Select Unrestricted Cases...
	IF (@restrictionStatus = 1 OR @restrictionStatus = 2)
	BEGIN
		INSERT	INTO	@Results ([RefId], [ModuleId], [WorkflowId], [CaseId], [IsRestricted], [IsComplete], [WorkStatusId], [StatusCodeName], [MemberUnit], [MemberUnitId], [MemberName], [MemberSSN])
				SELECT	f.lodId, ws.moduleId, f.workflow, f.case_id, 0, ws.isFinal, ws.ws_id, ws.description, f.member_unit, f.member_unit_id, f.member_name, f.member_ssn
				FROM	Form348 f
						JOIN vw_WorkStatus ws ON f.status = ws.ws_id
				WHERE	f.sarc = 1
						AND f.restricted = 0
						AND CONVERT(DATE, f.created_date) BETWEEN @beginDate AND @endDate
						AND ws.isCancel = 0
						AND (
								CASE 
									WHEN @completionStatus = 2 THEN 0	-- In-Progress
									WHEN @completionStatus = 3 THEN 1	-- Completed
									ELSE ws.IsFinal						-- All
								END
							) = ws.IsFinal
						 
	END

	-- Select restricted Cases...
	IF (@restrictionStatus = 1 OR @restrictionStatus = 3)
	BEGIN
		-- Select legacy restricted SARC cases from the LOD (Form348) table...
		INSERT	INTO	@Results ([RefId], [ModuleId], [WorkflowId], [CaseId], [IsRestricted], [IsComplete], [WorkStatusId], [StatusCodeName], [MemberUnit], [MemberUnitId], [MemberName], [MemberSSN])
				SELECT	f.lodId, ws.moduleId, f.workflow, f.case_id, 1, ws.isFinal, ws.ws_id, ws.description, f.member_unit, f.member_unit_id, f.member_name, f.member_ssn
				FROM	Form348 f
						JOIN vw_WorkStatus ws ON f.status = ws.ws_id
				WHERE	f.sarc = 1
						AND f.restricted = 1
						AND CONVERT(DATE, f.created_date) BETWEEN @beginDate AND @endDate
						AND ws.isCancel = 0
						AND (
								CASE 
									WHEN @completionStatus = 2 THEN 0	-- In-Progress
									WHEN @completionStatus = 3 THEN 1	-- Completed
									ELSE ws.IsFinal						-- All
								END
							) = ws.IsFinal


		-- Select cases from the Form348_SARC table which as of the 2017 Q2 (January 2017) release contains all new restricted SARC cases...
		INSERT	INTO	@Results ([RefId], [ModuleId], [WorkflowId], [CaseId], [IsRestricted], [IsComplete], [WorkStatusId], [StatusCodeName], [MemberUnit], [MemberUnitId], [MemberName], [MemberSSN])
				SELECT	f.sarc_id, ws.moduleId, f.workflow, f.case_id, 1, ws.isFinal, ws.ws_id, ws.description, f.member_unit, f.member_unit_id, f.member_name, f.member_ssn
				FROM	Form348_SARC f
						JOIN vw_WorkStatus ws ON f.status = ws.ws_id
				WHERE	CONVERT(DATE, f.created_date) BETWEEN @beginDate AND @endDate
						AND ws.isCancel = 0
						AND (
								CASE 
									WHEN @completionStatus = 2 THEN 0	-- In-Progress
									WHEN @completionStatus = 3 THEN 1	-- Completed
									ELSE ws.IsFinal						-- All
								END
							) = ws.IsFinal
	END


	-- OUTPUT RESULTS --
	SELECT	r.RefId, r.ModuleId, r.WorkflowId, r.CaseId, r.IsRestricted, r.IsComplete, r.WorkStatusId, r.StatusCodeName, r.MemberUnit, r.MemberUnitId, r.MemberName, r.MemberSSN
	FROM	@Results r
END
GO

