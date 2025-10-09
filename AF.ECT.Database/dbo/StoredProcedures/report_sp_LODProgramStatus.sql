
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 8/11/2016
-- Description:	Executes the LOD Program Status canned report.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	3/23/2017
-- Description:		Altered to no longer use totals for the unit view results
--					and the unit view results are no longer grouped together by
--					unit.
-- ============================================================================
CREATE PROCEDURE [dbo].[report_sp_LODProgramStatus]
	@quarter INT,
	@year INT,
	@unitId INT,
	@rptView INT,
	@groupByChildUnits BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- VALIDATE INPUT --
	IF (@unitId IS NULL)
	BEGIN
		RETURN
	END
	
	IF (ISNULL(@rptView, 0) = 0)
	BEGIN
		RETURN
	END

	IF (@groupByChildUnits IS NULL)
	BEGIN
		SET @groupByChildUnits = 0
	END
	
	-- DECLARE VARIABLES --
	DECLARE	@isComplete BIT = 1,
			@includeSubUnits BIT = 1,
			@currentRefId INT = 0,
			@currentIsFormal BIT = 0,
			@maxDepth INT = 2,
			@parentCSC_ID AS INT,
			@childUnitCount AS INT = 0,
			@currentUnitName NVARCHAR(100),
			@currentUnitId INT = 0

	DECLARE @Results TABLE
	(
		RefId INT,
		CaseId VARCHAR(50),
		DateCompleted DATETIME,
		MemberUnit NVARCHAR(100),
		UnitId INT,
		IsFormal BIT,
		DaysOpen INT,

		MedTechDays INT,
		MedOffDays INT,
		UnitCCDays INT,
		WingJADays INT,
		WingCCDays INT,
		WingSARCDays INT,
		PMDAys INT,
		IODays INT,
		FormalWingJADays INT,
		FormalWingCCDays INT,
		BoardTechDays INT,
		BoardJADays INT,
		BoardAdminDays INT,		-- Board A1
		BoardMedOffDays INT,	-- Board SG
		ApprAuthDays INT,
		FormalBoardTechDays INT,
		FormalBoardJADays INT,
		FormalBoardAdminDays INT,		-- Board A1
		FormalBoardMedOffDays INT,		-- Board SG
		FormalApprAuthDays INT,

		GroupedUnitId INT
	)

	DECLARE @StatusTotals TABLE
	(
		Status VARCHAR(100),
		Days INT
	)

	DECLARE @ChildUnits TABLE
	(
		csId INT,
		Name NVARCHAR(100),
		PAS NVARCHAR(4)
	)

	-- Create a curosr which will grab all of the open cases stored in the @Results table...
	DECLARE OpenCasesCursor CURSOR FOR	SELECT	r.RefId, r.IsFormal
										FROM	@Results r
	
	-- Create a cursor which will grab all of the child units stored in the @ChildUnits table...
	DECLARE ChildUnitsCursor CURSOR FOR SELECT	cu.csId, cu.Name
										FROM	@ChildUnits cu
	
	-- If necessary, find all of the child units of @unitId...using the non-medical reporting view because it returns the command structure...
	IF (@groupByChildUnits = 1)
	BEGIN
		SET @parentCSC_ID=(SELECT CSC_ID FROM COMMAND_STRUCT_CHAIN WHERE CS_ID=@unitId AND CHAIN_TYPE='PHA_NMREPORT')

		;WITH  childUnits (CS_ID, CSC_ID, CSC_ID_PARENT, CHAIN_TYPE, [Level], rn) 
		AS
		( 
			-- Anchor member   
			SELECT	CS_ID ,CSC_ID, CSC_ID_PARENT, CHAIN_TYPE, 0, convert(varchar(max),right(row_number() over (order by CSC_ID),10)) rn  
			FROM	COMMAND_STRUCT_CHAIN 
			WHERE	CSC_ID = @parentCSC_ID AND CHAIN_TYPE = 'PHA_NMREPORT' AND CS_ID IS NOT NULL
				
			UNION ALL 
				
			-- Recursive member
			SELECT	t1.CS_ID ,t1.CSC_ID, t1.CSC_ID_PARENT, t1.CHAIN_TYPE, childUnits.[Level] + 1,
					rn + '/' + convert(varchar(max),right(row_number() over (order by childUnits.CSC_ID),10))
			FROM	COMMAND_STRUCT_CHAIN t1  
					INNER JOIN childUnits ON t1.CSC_ID_PARENT = childUnits.CSC_ID AND t1.CHAIN_TYPE = childUnits.CHAIN_TYPE AND t1.cs_id IS NOT NULL
			WHERE	'/' + cast(t1.cs_id as varchar(20)) + '/' NOT LIKE '%/' + CAST(childUnits.cs_id AS VARCHAR(MAX)) + '/%'
					AND childUnits.[Level] + 1 < @maxDepth
		)
		INSERT	INTO	@ChildUnits ([csId], [Name], [PAS])
				SELECT	cu.CS_ID, cs.LONG_NAME, cs.PAS_CODE
				FROM	childUnits cu
						JOIN Command_Struct cs ON cu.CS_ID = cs.CS_ID
				--WHERE	cu.Level <> 0

		SELECT @childUnitCount = COUNT(*) FROM @ChildUnits cu WHERE cu.csId <> @unitId

		IF (@childUnitCount IS NULL)
		BEGIN
			SET @childUnitCount = 0
		END
	END


	-- CALCULATE RESULTS --

	-- Select all open LOD cases which meet the unit and rptview requirements of the user
	INSERT	INTO	@Results ([RefId], [CaseId], [DateCompleted], [MemberUnit], [UnitId], [IsFormal], [DaysOpen])
			SELECT	l.lodId AS RefId, l.[Case Id] AS CaseId, l.[Date Completed], l.[Member Unit] AS MemberUnit, l.member_unit_id AS UnitId,
					CASE
						WHEN l.Formal = 'No' THEN 0
						ELSE 1
					END AS IsFormal,
					CASE 
						WHEN l.isFinal = 1 THEN l.[Total Days]
						ELSE l.[Days Open]
					END AS DaysOpen
			FROM	vw_lod l
					JOIN vw_WorkStatus vws ON l.ws_id = vws.ws_id
			WHERE	l.isFinal = @isComplete
					AND vws.isCancel = 0
					AND
					(
						@quarter <= 0	-- No quarter was selected so the full annual report will be run...
						OR
						(
							@quarter > 0
							AND
							(
								DATEPART(MONTH, l.[Date Completed]) BETWEEN 
								(
									CASE
										WHEN @quarter = 1 THEN 1		-- Quarter 1 = Jan(1), Feb(2), Mar(3)
										WHEN @quarter = 2 THEN 4		-- Quarter 2 = Apr(4), May(5), June(6)
										WHEN @quarter = 3 THEN 7		-- Quarter 3 = July(7), Aug(8), Sept(9)
										WHEN @quarter = 4 THEN 10		-- Quarter 4 = Oct(10), Nov(11), Dec(12)
									END
								)
								AND
								(
									CASE
										WHEN @quarter = 1 THEN 3		-- Quarter 1 = Jan(1), Feb(2), Mar(3)
										WHEN @quarter = 2 THEN 6		-- Quarter 2 = Apr(4), May(5), June(6)
										WHEN @quarter = 3 THEN 9		-- Quarter 3 = July(7), Aug(8), Sept(9)
										WHEN @quarter = 4 THEN 12		-- Quarter 4 = Oct(10), Nov(11), Dec(12)
									END
								)
							)
						)
					)
					AND
					(
						DATEPART(YEAR, l.[Date Completed]) = @year
					)
					AND
					(
						(
							@includeSubUnits = 1 
							AND
							l.member_unit_id IN (SELECT child_id FROM Command_Struct_Tree WHERE parent_id = @unitId AND view_type = @rptView)
						)
						OR
						(
							@includeSubUnits = 0
							AND
							l.member_unit_id = @unitId
						)
					)
			ORDER	BY DaysOpen DESC

	-- For each case calculate the totals days spent within each step of the workflow, the total days in current status, and the total days open
	OPEN OpenCasesCursor

	FETCH NEXT FROM OpenCasesCursor INTO @currentRefId, @currentIsFormal

	WHILE @@FETCH_STATUS = 0
	BEGIN
		DELETE FROM	@StatusTotals

		-- Calculate the aggregated totals for each status this case has been in...
		INSERT	INTO	@StatusTotals ([Status], [Days])
				SELECT	ws.description, SUM(DATEDIFF(D, wst.startDate, ISNULL(wst.endDate, GETDATE()))) AS Days
				FROM	core_WorkStatus_Tracking wst
						JOIN vw_WorkStatus ws ON wst.ws_id = ws.ws_id
				WHERE	wst.module = 2
						AND wst.refId = @currentRefId
				GROUP	BY ws.description

		-- Get the totals for each status...
		UPDATE	@Results
		SET		MedTechDays =			(SELECT st.Days FROM @StatusTotals st WHERE st.Status = 'Medical Technician Input'),
				MedOffDays =			(SELECT st.Days FROM @StatusTotals st WHERE st.Status = 'Medical Officer Review'),
				UnitCCDays =			(SELECT st.Days FROM @StatusTotals st WHERE st.Status = 'Unit Commander Review'),
				WingJADays =			(SELECT st.Days FROM @StatusTotals st WHERE st.Status = 'Wing JA Review'),
				WingCCDays =			(SELECT st.Days FROM @StatusTotals st WHERE st.Status = 'Appointing Authority Review'),
				WingSARCDays =			(SELECT st.Days FROM @StatusTotals st WHERE st.Status = 'Wing SARC Input'),
				PMDays =				(SELECT st.Days FROM @StatusTotals st WHERE st.Status = 'Notify Formal Investigator'),
				BoardTechDays =			(SELECT st.Days FROM @StatusTotals st WHERE st.Status = 'AFRC LOD Board Review'),
				BoardJADays =			(SELECT st.Days FROM @StatusTotals st WHERE st.Status = 'LOD Board Legal Review'),
				BoardAdminDays =		(SELECT st.Days FROM @StatusTotals st WHERE st.Status = 'LOD Board Personnel Review'),
				BoardMedOffDays =		(SELECT st.Days FROM @StatusTotals st WHERE st.Status = 'LOD Board Medical Review'),
				ApprAuthDays =			(SELECT st.Days FROM @StatusTotals st WHERE st.Status = 'Approving Authority Action'),
				FormalBoardTechDays =	(SELECT st.Days FROM @StatusTotals st WHERE st.Status = 'Formal AFRC LOD Board Review'),
				FormalBoardJADays =		(SELECT st.Days FROM @StatusTotals st WHERE st.Status = 'Formal LOD Board Legal Review'),
				FormalBoardAdminDays =	(SELECT st.Days FROM @StatusTotals st WHERE st.Status = 'Formal LOD Board Personnel Review'),
				FormalBoardMedOffDays = (SELECT st.Days FROM @StatusTotals st WHERE st.Status = 'Formal LOD Board Medical Review'),
				FormalApprAuthDays =	(SELECT st.Days FROM @StatusTotals st WHERE st.Status = 'Formal Approving Authority Action'),
				IODays = (
							CASE
								WHEN @currentIsFormal = 1 THEN (SELECT st.Days FROM @StatusTotals st WHERE st.Status = 'Formal Investigation')
								ELSE NULL
							END
						),
				FormalWingJADays = (
									CASE
										WHEN @currentIsFormal = 1 THEN (SELECT st.Days FROM @StatusTotals st WHERE st.Status = 'Formal Action by Wing JA')
										ELSE NULL
									END
								),
				FormalWingCCDays = (
									CASE
										WHEN @currentIsFormal = 1 THEN (SELECT st.Days FROM @StatusTotals st WHERE st.Status = 'Formal Action by Appointing Authority')
										ELSE NULL
									END
								)
		WHERE	RefId = @currentRefId
		
		FETCH NEXT FROM OpenCasesCursor INTO @currentRefId, @currentIsFormal
	END

	CLOSE OpenCasesCursor
	DEALLOCATE OpenCasesCursor

	-- OUTPUT RESULTS --
	IF (@childUnitCount > 0 AND @groupByChildUnits = 1)
	BEGIN
		SELECT	r.UnitId AS UnitId, r.MemberUnit AS MemberUnit,
				pcs.CS_ID AS ParentUnitId, pcs.LONG_NAME AS ParentMemberUnit,
				r.DaysOpen AS DaysOpen,
				r.MedTechDays AS MedTechDays,
				r.MedOffDays AS MedOffDays,
				r.UnitCCDays AS UnitCCDays,
				r.WingJADays AS WingJADays,
				r.WingCCDays AS WingCCDays,
				r.WingSARCDays AS WingSARCDays,
				r.PMDAys AS PMDAys,
				r.IODays AS IODays,
				r.FormalWingJADays AS FormalWingJADays,
				r.FormalWingCCDays AS FormalWingCCDays,
				r.BoardTechDays AS BoardTechDays,
				r.BoardJADays AS BoardJADays,
				r.BoardAdminDays AS BoardAdminDays,
				r.BoardMedOffDays AS BoardMedOffDays,
				r.ApprAuthDays AS ApprAuthDays,
				r.FormalBoardTechDays AS FormalBoardTechDays,
				r.FormalBoardJADays AS FormalBoardJADays,
				r.FormalBoardAdminDays AS FormalBoardAdminDays,
				r.FormalBoardMedOffDays AS FormalBoardMedOffDays,
				r.FormalApprAuthDays AS FormalApprAuthDays
		FROM	@Results r
				JOIN Command_Struct_Chain csc ON r.UnitId = csc.CS_ID AND csc.CHAIN_TYPE = 'PHA_NMREPORT'
				JOIN Command_Struct_Chain pcsc ON csc.CSC_ID_PARENT = pcsc.CSC_ID
				JOIN Command_Struct pcs ON pcsc.CS_ID = pcs.CS_ID
		ORDER	BY r.UnitId
	END
	ELSE
	BEGIN
		-- Select individual cases directly associated with @unitId...
		SELECT	*
		FROM	@Results r
		WHERE	r.UnitId = @unitId
	END
END
GO

