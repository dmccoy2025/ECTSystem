
-- ============================================================================
-- Author:		Evan Morrison, Ken Barnett
-- Create date: 8/22/2016
-- Description:	Executes the LOD Compliance Accuracy report.
-- ============================================================================
CREATE PROCEDURE [dbo].[report_sp_LODComplianceAccuracy]
	@refIds tblIntegerList READONLY,
	@unitId INT,
	@groupByChildUnits BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF (ISNULL(@unitId, 0) = 0)
	BEGIN
		RETURN
	END

	IF (@groupByChildUnits IS NULL)
	BEGIN
		SET @groupByChildUnits = 0
	END

	
	-- DECLARE VARIABLES --
	DECLARE	@currentUnitId INT = 0,
			@currentUnitName NVARCHAR(100),
			@maxDepth INT = 2,
			@parentCSC_ID AS INT,
			@childUnitCount AS INT = 0,
			@currentRefId INT = 0,
			@currentAppointing VARCHAR(50),
			@currentApproving VARCHAR(50),
			@currentFormalAppointing VARCHAR(50),
			@currentFormalApproving VARCHAR(50),
			@currentAccuracy INT,
			@appointingFinding VARCHAR(50),
			@approvingFinding VARCHAR(50)

	DECLARE @Results TABLE
	(
		RefId INT,
		caseID VARCHAR(50),
		DateCompleted DATETIME,
		MemberUnit NVARCHAR(100),
		Appointing VARCHAR(50),
		Approving VARCHAR(50),
		FormalAppointing VARCHAR(50),
		FormalApproving VARCHAR(50),
		Accuracy INT,
		UnitId INT,
		GroupedUnitId INT
	)

	DECLARE @ChildUnits TABLE
	(
		csId INT,
		Name NVARCHAR(100),
		PAS NVARCHAR(4)
	)

	-- Create a cursor which will grab all of the records in the @Results table...
	DECLARE ResultsCursor CURSOR FOR	SELECT	r.RefId, r.Appointing, r.Approving, r.FormalAppointing, r.FormalApproving
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
	INSERT	INTO	@Results ([RefId],[caseID], [DateCompleted], [MemberUnit], [Appointing], [Approving], [FormalAppointing], [FormalApproving], [UnitId])
			SELECT	s.n AS [RefId], a.[Case Id] AS [caseID], a.[Date Completed] AS [DateCompleted], a.member_unit AS [MemberUnit], 
					find_wcc.Description AS [Appointing],find_baa.Description AS [Approving],
					CASE	
						WHEN 
							f_wccf.finding IS NULL THEN find_io.Description
							ELSE find_wccf.Description
						END AS [FormalAppointing],
						
					find_baaf.Description AS [FormalApproving],
					a.member_unit_id
			FROM	@refIds s
					INNER JOIN dbo.vw_lod AS a ON a.lodId = s.n
				
					LEFT OUTER JOIN dbo.FORM348_findings AS f_wcc ON f_wcc.LODID = a.lodId AND f_wcc.ptype = 5 
					LEFT OUTER JOIN dbo.core_lkUpFindings AS find_wcc ON find_wcc.Id = f_wcc.finding
				
					LEFT OUTER JOIN dbo.FORM348_findings AS f_wccf ON f_wccf.LODID = a.lodId AND f_wccf.ptype = 13
					LEFT OUTER JOIN dbo.core_lkUpFindings AS find_wccf ON find_wccf.Id = f_wccf.finding
				
					LEFT OUTER JOIN dbo.FORM348_findings AS f_io ON f_io.LODID = a.lodId AND f_io.ptype = 19
					LEFT OUTER JOIN dbo.core_lkUpFindings AS find_io ON find_io.Id = f_io.finding
				
					LEFT OUTER JOIN dbo.FORM348_findings AS f_baa ON f_baa.LODID = a.lodId AND f_baa.ptype = 10
					LEFT OUTER JOIN dbo.core_lkUpFindings AS find_baa ON find_baa.Id = f_baa.finding
					LEFT OUTER JOIN dbo.FORM348_findings AS f_baaf ON f_baaf.LODID = a.lodId AND f_baaf.ptype = 18
					LEFT OUTER JOIN dbo.core_lkUpFindings AS find_baaf ON find_baaf.Id = f_baaf.finding


	-- For each result item calculate its accuracy...this was moved into the stored procedure from the source code in order to accomodate the aggregation of results by unit...
	OPEN ResultsCursor

	FETCH NEXT FROM ResultsCursor INTO @currentRefId, @currentAppointing, @currentApproving, @currentFormalAppointing, @currentFormalApproving

	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @appointingFinding = NULL
		SET @approvingFinding = NULL
		SET @currentAccuracy = 0

		-- Determine final Appointing Authority (Wing CC) finding...
		IF (NOT ISNULL(dbo.TRIM(@currentFormalAppointing), '') = '')
		BEGIN
			SET @appointingFinding = dbo.TRIM(@currentFormalAppointing)
		END
		ELSE IF (NOT ISNULL(dbo.TRIM(@currentAppointing), '') = '')
		BEGIN
			SET @appointingFinding = dbo.TRIM(@currentAppointing)
		END

		-- Determine final Approving Authority finding...
		IF (NOT ISNULL(dbo.TRIM(@currentFormalApproving), '') = '')
		BEGIN
			SET @approvingFinding = dbo.TRIM(@currentFormalApproving)
		END
		ELSE IF (NOT ISNULL(dbo.TRIM(@currentApproving), '') = '')
		BEGIN
			SET @approvingFinding = dbo.TRIM(@currentApproving)
		END

		IF ((NOT ISNULL(@appointingFinding, '') = '') AND (NOT ISNULL(@approvingFinding, '') = ''))
		BEGIN
			IF (@approvingFinding = @appointingFinding)
			BEGIN
				SET @currentAccuracy = 100
			END
		END

		UPDATE	@Results
		SET		Accuracy = @currentAccuracy
		WHERE	RefId = @currentRefId
			
		FETCH NEXT FROM ResultsCursor INTO @currentRefId, @currentAppointing, @currentApproving, @currentFormalAppointing, @currentFormalApproving
	END

	CLOSE ResultsCursor
	DEALLOCATE ResultsCursor


	-- OUTPUT RESULTS --
	IF (@childUnitCount > 0 AND @groupByChildUnits = 1)
	BEGIN
		-- For each child unit determine which cases are equal to or subordinate to the child unit and set the GroupedUnitId value to be the id of the child unit...
		OPEN ChildUnitsCursor

		FETCH NEXT FROM ChildUnitsCursor INTO @currentUnitId, @currentUnitName

		WHILE @@FETCH_STATUS = 0
		BEGIN
			UPDATE	@Results
			SET		GroupedUnitId = @currentUnitId
			WHERE	UnitId IN (SELECT child_id FROM Command_Struct_Tree WHERE parent_id = @currentUnitId AND view_type = 2)

			FETCH NEXT FROM ChildUnitsCursor INTO @currentUnitId, @currentUnitName
		END

		CLOSE ChildUnitsCursor
		DEALLOCATE ChildUnitsCursor

		-- Select average accuracy results aggregated by their common GroupedUnitId...
		SELECT	r.GroupedUnitId AS UnitId, cs.LONG_NAME AS MemberUnit,
				AVG(r.Accuracy) AS Accuracy
		FROM	@Results r
				JOIN Command_Struct cs ON r.GroupedUnitId = cs.CS_ID
		GROUP	BY r.GroupedUnitId, cs.LONG_NAME
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

