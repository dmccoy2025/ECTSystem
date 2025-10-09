
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 8/23/2016
-- Description:	Executes the LOD Compliance Quality canned report.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	10/19/2016
-- Description:		Updated the stored procedure to be more efficient by using
--					local temp tables with indexes instead of table variables
--					and no longer using a cursor to iterate over the #Results
--					table.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	12/12/2016
-- Description:		Updated the stored procedure to use the Rowa table for 
--					cases created after the release of the new AFI LOD.
-- ============================================================================
CREATE PROCEDURE [dbo].[report_sp_LODCompliance_Quality]
	@refIds tblIntegerList READONLY,
	@unitId INT,
	@groupByChildUnits BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- VALIDATE INPUT --
	IF ((SELECT COUNT(*) FROM @refIds) = 0)
	BEGIN
		RETURN
	END

	IF (ISNULL(@unitId, 0) = 0)
	BEGIN
		RETURN
	END

	IF (@groupByChildUnits IS NULL)
	BEGIN
		SET @groupByChildUnits = 0
	END

	
	-- DECLARE VARIABLES --
	DECLARE	@currentRefId INT = 0,
			@currentIsFormal BIT = 0,
			@cutoverDate DATE = '2017-01-31',		-- The date (YYYY-MM-DD) which the LOD workflow was properly configured to handle RWOAs in production...
			@currentUnitId INT = 0,
			@currentUnitName NVARCHAR(100),
			@maxDepth INT = 2,
			@parentCSC_ID AS INT,
			@childUnitCount AS INT = 0

	DECLARE @iRwCnt int --rowcount

	CREATE TABLE #ActionLogs
	(
		LogId INT,
		RefId INT
	)

	CREATE TABLE #CompletedCases
	(
		RefId INT,
		IsFormal BIT
	)

	CREATE TABLE #Results
	(
		RefId INT,
		CaseId VARCHAR(50),
		DateCompleted DATETIME,
		MemberUnit NVARCHAR(100),
		UnitId INT,
		IsFormal BIT,
		TotalRWOA INT,
		ReasonOneTotal INT,				-- Cancel LOD
		ReasonTwoTotal INT,				-- Multiple Diagnoses
		ReasonThreeTotal INT,			-- No Orders
		ReasonFourTotal INT,			-- Wrong Orders
		ReasonFiveTotal INT,			-- Wrong Diagnosis
		ReasonSixTotal INT,				-- Order do not Cover Active Duty Service in Question
		ReasonSevenTotal INT,			-- Police Report Not Included with MVAs
		ReasonEightTotal INT,			-- No Medical Documentation
		ReasonNineTotal INT,			-- Insufficient Medical Documentation
		ReasonTenTotal INT,				-- Supporting Documentation Pertains to Different Individual
		ReasonElevenTotal INT,			-- Supporting Documentation is Distorted/Unreadable
		ReasonTwelveTotal INT,			-- Information Does Not Match with Main Packet
		ReasonThirteenTotal INT,		-- Other

		GroupedUnitId INT
	)

	CREATE TABLE #ReasonTotals
	(
		Reason INT,
		Total INT
	)

	CREATE CLUSTERED INDEX idx_tmpReasonTotals ON #ReasonTotals([Reason]) WITH FILLFACTOR = 100

	DECLARE @ChildUnits TABLE
	(
		csId INT,
		Name NVARCHAR(100),
		PAS NVARCHAR(4)
	)

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
	INSERT	INTO	#Results ([RefId], [CaseId], [DateCompleted], [MemberUnit], [UnitId], [IsFormal])
			SELECT	l.lodId AS RefId, l.[Case Id] AS CaseId, l.[Date Completed], l.[Member Unit] AS MemberUnit, l.member_unit_id AS UnitId,
					CASE
						WHEN l.Formal = 'No' THEN 0
						ELSE 1
					END AS IsFormal
			FROM	vw_lod l
					JOIN @refIds r ON l.lodId = r.n
					JOIN vw_WorkStatus vws ON l.ws_id = vws.ws_id
			ORDER	BY CaseId DESC

	-- For each case calculate the totals days spent within each step of the workflow, the total days in current status, and the total days open
	INSERT	INTO	#ActionLogs ([LogId], [RefId])
			SELECT	la.logId, r.RefId
			FROM	#Results r
					JOIN core_LogAction la ON r.RefId = la.referenceId
					JOIN core_lkupAction a ON la.actionId = a.actionId
					JOIN core_WorkStatus prevWS ON la.status = prevWS.ws_id
					JOIN core_WorkStatus newWS ON la.newStatus = newWS.ws_id
			WHERE	la.moduleId = 2
					AND a.actionName = 'Status Changed'
					AND prevWS.isBoardStatus = 1
					AND newWS.isBoardStatus = 0
					AND (
							la.notes LIKE '%return%'
							OR
							la.notes LIKE '%rwoa%'
						)

	CREATE CLUSTERED INDEX idx_tmpActionLogs ON #ActionLogs([LogId]) WITH FILLFACTOR = 100

	INSERT	INTO	#CompletedCases ([RefId], [IsFormal])
			SELECT	r.RefId, r.IsFormal
			FROM	#Results r

	SET @iRwCnt = @@ROWCOUNT

	CREATE CLUSTERED INDEX idx_tmpCompletedCases ON #CompletedCases([RefId]) WITH FILLFACTOR = 100

	WHILE (@iRwCnt > 0)
	BEGIN
		SELECT	TOP 1 @currentRefId = cc.RefId
		FROM	#CompletedCases cc

		--PRINT 'Processing ' + CONVERT(VARCHAR(15), @currentRefId) + '...'

		SET @iRwCnt = @@ROWCOUNT --ensure that we still have data

		IF (@iRwCnt > 0)
		BEGIN

			-- Else calculate Reason Totals
			DELETE FROM	#ReasonTotals

			-- Determine which method needs to be used for compiling the RWOA data...
			IF ((SELECT f.workflow FROM Form348 f WHERE f.lodId = @currentRefId) <> 1)
			BEGIN
				-- Calculate the totals for each reason this case has a RWOA record for...
				INSERT	INTO	#ReasonTotals ([Reason], [Total])
						SELECT	r.reason_sent_back AS Reason, COUNT(r.reason_sent_back) AS Total
						FROM	Rwoa r
								JOIN core_Workflow w ON r.workflow = w.workflowId
								JOIN core_lkupModule m ON w.moduleId = m.moduleId
								JOIN core_lkupRWOAReasons rr ON r.reason_sent_back = rr.ID
						WHERE	m.moduleId = 2
								AND r.refId = @currentRefId
						GROUP	BY r.reason_sent_back
			END
			ELSE
			BEGIN
				-- If case created date is before 2016 Q4 release then artificially create the total by looking at tracking data; TotalRWOA = value; ReasonThirteenTotal = value, ReasonOneTotal = null, etc...
				INSERT	INTO	#ReasonTotals ([Reason], [Total])
						SELECT	13, COUNT(la.logId)
						FROM	#ActionLogs la
						WHERE	la.RefId = @currentRefId
			END


			-- Get the totals for each status...
			UPDATE	#Results
			SET		ReasonOneTotal =		(SELECT st.Total FROM #ReasonTotals st WHERE st.Reason = 1),
					ReasonTwoTotal =		(SELECT st.Total FROM #ReasonTotals st WHERE st.Reason = 2),
					ReasonThreeTotal =		(SELECT st.Total FROM #ReasonTotals st WHERE st.Reason = 3),
					ReasonFourTotal =		(SELECT st.Total FROM #ReasonTotals st WHERE st.Reason = 4),
					ReasonFiveTotal =		(SELECT st.Total FROM #ReasonTotals st WHERE st.Reason = 5),
					ReasonSixTotal =		(SELECT st.Total FROM #ReasonTotals st WHERE st.Reason = 6),
					ReasonSevenTotal =		(SELECT st.Total FROM #ReasonTotals st WHERE st.Reason = 7),
					ReasonEightTotal =		(SELECT st.Total FROM #ReasonTotals st WHERE st.Reason = 8),
					ReasonNineTotal =		(SELECT st.Total FROM #ReasonTotals st WHERE st.Reason = 9),
					ReasonTenTotal =		(SELECT st.Total FROM #ReasonTotals st WHERE st.Reason = 10),
					ReasonElevenTotal =		(SELECT st.Total FROM #ReasonTotals st WHERE st.Reason = 11),
					ReasonTwelveTotal =		(SELECT st.Total FROM #ReasonTotals st WHERE st.Reason = 12),
					ReasonThirteenTotal =	(SELECT st.Total FROM #ReasonTotals st WHERE st.Reason = 13),
					TotalRWOA = NULL
			WHERE	RefId = @currentRefId

			DELETE	FROM #CompletedCases 
					WHERE RefId = @currentRefId --remove processed record
		END
	END

	DROP TABLE #ReasonTotals
	DROP TABLE #CompletedCases
	DROP TABLE #ActionLogs

	-- OUTPUT RESULTS --
	IF (@childUnitCount > 0 AND @groupByChildUnits = 1)
	BEGIN
		-- For each child unit determine which cases are equal to or subordinate to the child unit and set the GroupedUnitId value to be the id of the child unit...
		OPEN ChildUnitsCursor

		FETCH NEXT FROM ChildUnitsCursor INTO @currentUnitId, @currentUnitName

		WHILE @@FETCH_STATUS = 0
		BEGIN
			UPDATE	#Results
			SET		GroupedUnitId = @currentUnitId
			WHERE	UnitId IN (SELECT child_id FROM Command_Struct_Tree WHERE parent_id = @currentUnitId AND view_type = 2)

			FETCH NEXT FROM ChildUnitsCursor INTO @currentUnitId, @currentUnitName
		END

		CLOSE ChildUnitsCursor
		DEALLOCATE ChildUnitsCursor

		-- Select results aggregated by their common GroupedUnitId...
		SELECT	r.GroupedUnitId AS UnitId, cs.LONG_NAME AS MemberUnit,
				SUM(r.ReasonOneTotal) AS ReasonOneTotal,
				SUM(r.ReasonTwoTotal) AS ReasonTwoTotal,
				SUM(r.ReasonThreeTotal) AS ReasonThreeTotal,
				SUM(r.ReasonFourTotal) AS ReasonFourTotal,
				SUM(r.ReasonFiveTotal) AS ReasonFiveTotal,
				SUM(r.ReasonSixTotal) AS ReasonSixTotal,
				SUM(r.ReasonSevenTotal) AS ReasonSevenTotal,
				SUM(r.ReasonEightTotal) AS ReasonEightTotal,
				SUM(r.ReasonNineTotal) AS ReasonNineTotal,
				SUM(r.ReasonTenTotal) AS ReasonTenTotal,
				SUM(r.ReasonTwelveTotal) AS ReasonTwelveTotal,
				SUM(r.ReasonThirteenTotal) AS ReasonThirteenTotal
		FROM	#Results r
				JOIN Command_Struct cs ON r.GroupedUnitId = cs.CS_ID
		GROUP	BY r.GroupedUnitId, cs.LONG_NAME
	END
	ELSE
	BEGIN
		-- Select individual cases directly associated with @unitId...
		SELECT	*
		FROM	#Results r
		WHERE	r.UnitId = @unitId
	END

	DROP TABLE #Results
END
GO

