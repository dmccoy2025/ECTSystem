
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 7/14/2016
-- Description:	Executes the PH Totals canned report to get the totals for the
--				Suicide Methods of the cases which meet the report parameters.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	6/30/2017
-- Description:		- Removed the RMU filter.
--					- Removed the NAF parameter
-- ============================================================================
CREATE PROCEDURE [dbo].[report_sp_PHTotalsSuicideMethodsReport]
	@unitId INT,
	@includeSubUnits BIT,
	@collocated TINYINT,
	@rptView INT,
	@beginReportingPeriod DATETIME,
	@endReportingPeriod DATETIME
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
	
	IF (@includeSubUnits IS NULL)
	BEGIN
		RETURN
	END

	IF (ISNULL(@collocated, 0) = 0)
	BEGIN
		RETURN
	END
	
	IF (ISNULL(@rptView, 0) = 0)
	BEGIN
		RETURN
	END
	
	IF (@beginReportingPeriod IS NULL)
	BEGIN
		RETURN
	END
	
	IF (@endReportingPeriod IS NULL)
	BEGIN
		RETURN
	END
	
	-- DECLARE VARIABLES --
	DECLARE @ReferenceIds TABLE
	(
		RefId INT
	)
	
	DECLARE @UnitIds TABLE
	(
		CSId INT
	)

	-- Determine which unit IDs are acceptable to select from...
	IF (@includeSubUnits = 1)
	BEGIN
		-- Find all of the subordinate units associate with @unitId
		INSERT	INTO	@UnitIds ([CSId])
				SELECT	t.child_id
				FROM	Command_Struct_Tree t
				WHERE	t.parent_id = @unitId 
						AND t.view_type = @rptView
	END
	ELSE
	BEGIN
		INSERT	INTO	@UnitIds ([CSId])
				VALUES	(@unitId)
	END

	-- Get all of the PH cases which meet the report conditions...
	--		@collocated: 1 = Both, 2 = collocated only, 3 = noncollocated only
	INSERT	INTO	@ReferenceIds ([RefId])
			SELECT	s.SC_Id
			FROM	vw_sc_ph_reporting s
			WHERE	s.[Reporting Period] BETWEEN @beginReportingPeriod AND @endReportingPeriod
					AND s.[PH Wing RMU ID] IN (SELECT DISTINCT CSId FROM @UnitIds)
					AND s.IsCollocated = CASE @collocated
											WHEN 2 THEN 1		-- Collocated Units only
											WHEN 3 THEN 0		-- Noncollocated Units only
											ELSE s.IsCollocated	-- Both Collocated and Noncollocated Units
										 END
	
	SELECT	fv.RefId, fv.SectionId, fv.FieldId, fv.FieldTypeId, fv.Value
	FROM	PH_Form_Value fv
			JOIN PH_Field f ON fv.FieldId = f.Id
			JOIN PH_FieldType ft ON fv.FieldTypeId = ft.Id
			JOIN @ReferenceIds r ON fv.RefId = r.RefId
	WHERE	f.Name = 'Suicide Method'
			AND ft.Name = 'Suicide Method'
END
GO

