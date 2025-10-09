
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 8/18/2016
-- Description:	Executes the LOD QA and Compliance report.
-- ============================================================================

CREATE PROCEDURE [dbo].[report_sp_LODCompliance]
	@quarter INT,
	@year INT,
	@unitId INT,
	@rptView INT
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

	-- DECLARE VARIABLES --
	DECLARE	@isComplete BIT = 1,
			@includeSubUnits BIT = 1,
			@currentRefId INT = 0,
			@currentIsFormal BIT = 0
			
	DECLARE @Cases TABLE
	(
		lodId INT
	)
	
	INSERT INTO @Cases ([lodId])
		SELECT l.lodId as lodId
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
					
	
	SELECT *
	FROM @Cases

END
GO

