

-- =============================================
-- Description:	 From ECT, reports to track LOD and Special Cases which have been returned for action  
-- =============================================
CREATE PROCEDURE [dbo].[SpecialCaseRFAByUnitTEST]
	-- Add the parameters for the stored procedure here
	 @beginDate datetime = 0, 
	 @endDate datetime = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
--DECLARE @beginDate	DATETIME = '2017-11-01'		-- CHANGE THE YEAR
--DECLARE @endDate	DATETIME = '2018-10-31'		-- CHANGE THE YEAR



-------------------------------------------------------------------------------
------------------------ IGNORE EVERYTHING BELOW HERE -------------------------
-------------------------------------------------------------------------------

-- 4; PHA_WORK; RMU View (Physical Responsibility)

DECLARE	@unitId INT = 18,
		@maxDepth INT = 20,
		@parentCSC_ID AS INT,
		@currentUnitName NVARCHAR(100),
		@currentUnitId INT = 0,
		@currentPAS NVARCHAR(4),
		@rptViewName VARCHAR(20) = 'PHA_WORK',
		@rptViewId TINYINT = 4,
		@reasonsAndCounts NVARCHAR(MAX),
		@totalCases INT = NULL,
		@totalCasesWithNoRWOA INT = NULL,
		@casesWithOneRWOA INT = NULL,
		@casesWithMoreThanOneRWOA INT = NULL

DECLARE @AllCases TABLE
(
	RefId INT,
	CaseId VARCHAR(100),
	ModuleId TINYINT,
	WorkflowId TINYINT,
	MemberUnitId INT
)

DECLARE @RawRwoaResults TABLE
(
	ModuleId TINYINT,
	RwoaId INT,
	RefId INT,
	WorkflowId TINYINT,
	DateSent DATETIME,
	ExplanationForSendingBack VARCHAR(MAX),
	ReasonSentBackId TINYINT,
	ReasonSentBack VARCHAR(150),
	CaseId VARCHAR(100),
	MemberUnitId INT
)

DECLARE @GroupedRwoaResults TABLE
(
	ModuleId TINYINT,
	RwoaId INT,
	RefId INT,
	WorkflowId TINYINT,
	DateSent DATETIME,
	ExplanationForSendingBack VARCHAR(MAX),
	ReasonSentBackId TINYINT,
	ReasonSentBack VARCHAR(150),
	CaseId VARCHAR(100),
	GroupedUnitId INT,
	GroupedUnitName NVARCHAR(100),
	GroupedPAS NVARCHAR(4)
)

DECLARE @GroupedRwoaCountResults TABLE
(
	GroupedUnitId INT,
	GroupedUnitName NVARCHAR(100),
	GroupedPAS NVARCHAR(4),
	TotalCases INT,
	TotalCasesWithRWOAs INT,
	TotalCasesNoRWOAs INT,
	TotalCasesOneRWOA INT,
	TotalCasesMoreThanOneRWOA INT,
	TotalRWOAs INT,
	Reasons VARCHAR(MAX)
)

DECLARE @ChildUnits TABLE
(
	csId INT,
	Name NVARCHAR(100),
	PAS NVARCHAR(4)
)

-- Create a cursor which will grab all of the child units stored in the @ChildUnits table...
DECLARE ChildUnitsCursor CURSOR FOR SELECT	cu.csId, cu.Name, cu.PAS
									FROM	@ChildUnits cu

DECLARE RwoaUnitsCursor CURSOR FOR	SELECT	r.GroupedUnitId
									FROM	@GroupedRwoaCountResults r
									WHERE	r.TotalRWOAs > 0


-- Grab all of the RMUs
INSERT	INTO	@ChildUnits ([csId], [Name], [PAS])
		SELECT	r.cs_id, cs.LONG_NAME, cs.PAS_CODE
		FROM	core_lkupRMUs r
				JOIN Command_Struct cs ON r.cs_id = cs.CS_ID


-- Get all special case RWOAs
INSERT	INTO	@RawRwoaResults ([ModuleId], [RwoaId], [RefId], [WorkflowId], [DateSent], [ExplanationForSendingBack], [ReasonSentBackId], [ReasonSentBack], [CaseId], [MemberUnitId])
		SELECT	moduleId, r.rwoaId, r.refId, r.workflow, r.date_sent, r.explanation_for_sending_back, r.reason_sent_back, rr.Description, c.case_id, c.Member_Unit_Id
		FROM	core_StatusCodes sc
				JOIN core_WorkStatus ws ON ws.statusId = sc.statusId
				JOIN Rwoa r ON r.workstatus = ws.ws_id 
				JOIN core_lkupRWOAReasons rr ON r.reason_sent_back = rr.ID
				JOIN form348_SC c ON c.SC_Id = r.refId AND c.workflow = r.workflow
		WHERE	(
					ws.isBoardStatus = 0 
					OR sc.description = 'MEB Return Without Action' 
					OR sc.description = 'PEPP Return Without Action Holding' 
					OR sc.description = 'RS Recruiter Comments'
				)
				AND moduleId NOT IN (2, 5, 16, 22, 24, 25, 26)
				AND sc.isCancel = 0
				AND sc.isFinal = 0
				AND sc.groupId IN (3, 98, 88)
				AND r.date_sent BETWEEN @beginDate AND @endDate
		ORDER	BY sc.statusId ASC


-- Get all special cases which have been submitted to a board level step at least once...
INSERT	INTO	@AllCases ([RefId], [CaseId], [ModuleId], [WorkflowId], [MemberUnitId])
		SELECT	DISTINCT s.SC_Id, s.case_id, s.Module_Id, s.workflow, s.Member_Unit_Id
		FROM	Form348_SC s
				JOIN vw_WorkStatus ws ON s.status = ws.ws_id
				JOIN core_WorkStatus_Tracking wst ON (s.SC_Id = wst.refId AND s.Module_Id = wst.module)
				JOIN vw_WorkStatus wswst ON wst.ws_id = wswst.ws_id
		WHERE	ws.isCancel = 0
				AND wswst.groupId IN (3, 98) -- Board Members (Board Medical, HQ AFRC Technician)
				AND wst.startDate BETWEEN @beginDate AND @endDate


-- Look over each child unit...
OPEN ChildUnitsCursor

FETCH NEXT FROM ChildUnitsCursor INTO @currentUnitId, @currentUnitName, @currentPAS

WHILE @@FETCH_STATUS = 0
BEGIN
	SET @casesWithOneRWOA = NULL
	SET @casesWithMoreThanOneRWOA = NULL
	SET @totalCases = NULL
	SET @totalCasesWithNoRWOA = NULL

	-- Determine number of cases that have only one RWOA...
	SELECT	@casesWithOneRWOA = COUNT(*)
	FROM	(
				SELECT	r.RefId, COUNT (r.RefId) AS RWOAs
				FROM	@RawRwoaResults r
				WHERE	r.MemberUnitId IN (SELECT child_id FROM Command_Struct_Tree WHERE parent_id = @currentUnitId AND view_type = @rptViewId)
				GROUP	BY r.RefId
			) AS CaseRWOACounts
	WHERE	CaseRWOACounts.RWOAs = 1


	-- Determine number of cases that more than one RWOA...
	SELECT	@casesWithMoreThanOneRWOA = COUNT(*)
	FROM	(
				SELECT	r.RefId, COUNT (r.RefId) AS RWOAs
				FROM	@RawRwoaResults r
				WHERE	r.MemberUnitId IN (SELECT child_id FROM Command_Struct_Tree WHERE parent_id = @currentUnitId AND view_type = @rptViewId)
				GROUP	BY r.RefId
			) AS CaseRWOACounts
	WHERE	CaseRWOACounts.RWOAs > 1


	-- Determine all special cases involved...
	SELECT	@totalCases = COUNT(*)
	FROM	(
				SELECT	c.RefId
				FROM	@AllCases c
				WHERE	c.MemberUnitId IN (SELECT child_id FROM Command_Struct_Tree WHERE parent_id = @currentUnitId AND view_type = @rptViewId)

				UNION

				SELECT	r.RefId
				FROM	@RawRwoaResults r
				WHERE	r.MemberUnitId IN (SELECT child_id FROM Command_Struct_Tree WHERE parent_id = @currentUnitId AND view_type = @rptViewId)
			) AS TotalCases


	-- Determine all special cases with no RWOAs...
	SELECT	@totalCasesWithNoRWOA = COUNT(*)
	FROM	(
				SELECT	c.RefId
				FROM	@AllCases c
				WHERE	c.MemberUnitId IN (SELECT child_id FROM Command_Struct_Tree WHERE parent_id = @currentUnitId AND view_type = @rptViewId)

				EXCEPT

				SELECT	r.RefId
				FROM	@RawRwoaResults r
				WHERE	r.MemberUnitId IN (SELECT child_id FROM Command_Struct_Tree WHERE parent_id = @currentUnitId AND view_type = @rptViewId)
			) AS TotalCasesWithNoRWOAs


	-- Calculate RWOA total for the current child unit...
	INSERT	INTO	@GroupedRwoaCountResults ([GroupedUnitId], [GroupedUnitName], [GroupedPAS], [TotalRWOAs], [TotalCasesOneRWOA], [TotalCasesMoreThanOneRWOA], [TotalCasesWithRWOAs], [TotalCases], [TotalCasesNoRWOAs])
			SELECT	@currentUnitId, @currentUnitName, @currentPAS, COUNT(*), @casesWithOneRWOA, @casesWithMoreThanOneRWOA, COUNT(DISTINCT r.RefId), @totalCases, @totalCasesWithNoRWOA
			FROM	@RawRwoaResults r
			WHERE	r.MemberUnitId IN (SELECT child_id FROM Command_Struct_Tree WHERE parent_id = @currentUnitId AND view_type = @rptViewId)


	-- Group RWOAs into the appropriate Wing/Group...
	INSERT	INTO	@GroupedRwoaResults ([GroupedUnitId], [GroupedUnitName], [GroupedPAS], [ModuleId], [RwoaId], [RefId], [WorkflowId], [DateSent], [ExplanationForSendingBack], [ReasonSentBackId], [ReasonSentBack], [CaseId])
			SELECT	@currentUnitId, @currentUnitName, @currentPAS, r.ModuleId, r.RwoaId, r.RefId, r.WorkflowId, r.DateSent, r.ExplanationForSendingBack, r.ReasonSentBackId, r.ReasonSentBack, r.CaseId
			FROM	@RawRwoaResults r
			WHERE	r.MemberUnitId IN (SELECT child_id FROM Command_Struct_Tree WHERE parent_id = @currentUnitId AND view_type = @rptViewId)
	
	FETCH NEXT FROM ChildUnitsCursor INTO @currentUnitId, @currentUnitName, @currentPAS
END

CLOSE ChildUnitsCursor
DEALLOCATE ChildUnitsCursor


-- Look over each unit which has RWOAs...
OPEN RwoaUnitsCursor

FETCH NEXT FROM RwoaUnitsCursor INTO @currentUnitId

WHILE @@FETCH_STATUS = 0
BEGIN
	SET @reasonsAndCounts = ''

	-- Build the reason counts string...
	SELECT	@reasonsAndCounts = (@reasonsAndCounts + (RwoaReasonsAndCounts.ReasonSentBack + ' (' + CONVERT(VARCHAR(20), RwoaReasonsAndCounts.ReasonCount) + '); '))
	FROM 
	(
		SELECT	r.ReasonSentBackId, r.ReasonSentBack, COUNT(*) AS ReasonCount
		FROM	@GroupedRwoaResults r
		WHERE	r.GroupedUnitId = @currentUnitId
		GROUP	BY r.ReasonSentBackId, r.ReasonSentBack
	) AS RwoaReasonsAndCounts
	ORDER	BY RwoaReasonsAndCounts.ReasonCount DESC

	UPDATE	@GroupedRwoaCountResults
	SET		Reasons = @reasonsAndCounts
	WHERE	GroupedUnitId = @currentUnitId

	FETCH NEXT FROM RwoaUnitsCursor INTO @currentUnitId
END

CLOSE RwoaUnitsCursor
DEALLOCATE RwoaUnitsCursor


-- OUTPUT RESULTS --
SELECT	r.GroupedUnitName AS [Unit], 
		r.GroupedPAS AS [PAS], 
		r.TotalCases AS [Total Cases], 
		r.TotalCasesNoRWOAs AS [Total Cases With No RWOAs], 
		r.TotalCasesWithRWOAs AS [Total Cases With RWOAs], 
		r.TotalCasesOneRWOA AS [Total Cases With One RWOA], 
		r.TotalCasesMoreThanOneRWOA AS [Total Cases With More Than One RWOA], 
		r.TotalRWOAs AS [Total RWOAs], 
		r.Reasons
FROM	@GroupedRwoaCountResults r
ORDER	BY r.TotalRWOAs DESC, r.GroupedUnitName


end
GO

