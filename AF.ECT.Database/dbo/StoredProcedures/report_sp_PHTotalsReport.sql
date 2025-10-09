
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 6/9/2016
-- Description:	Executes the PH Totals canned report.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	6/30/2017
-- Description:		- Removed the RMU filter.
--					- Removed the NAF parameter
-- ============================================================================
CREATE PROCEDURE [dbo].[report_sp_PHTotalsReport]
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

	DECLARE @Totals TABLE
	(
		SectionId INT,
		FieldId INT,
		FieldTypeId INT,
		Total BIGINT
	)
	
	DECLARE @UnitIds TABLE
	(
		CSId INT
	)

	DECLARE @currentSectionId INT,
			@currentFieldId INT,
			@currentFieldTypeId INT,
			@currentTotal BIGINT
	
	-- Create a curosr which will grab all of the Integer based PH Form Field records for the Form Fields specified in the query...
	DECLARE FormFieldCursor CURSOR FOR	SELECT	ff.SectionId, ff.FieldId, ff.FieldTypeId
										FROM	PH_Form_Field ff
												JOIN PH_Section s ON ff.SectionId = s.Id
												JOIN PH_FieldType ft ON ff.FieldTypeId = ft.Id
												JOIN core_lkupDataType dt ON ft.DataTypeId = dt.Id
										WHERE	dt.Name = 'Integer'
										ORDER	BY s.DisplayOrder

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

	SET	@currentSectionId = 0
	SET	@currentFieldId = 0
	SET	@currentFieldTypeId = 0
	SET	@currentTotal = 0

	OPEN FormFieldCursor

	-- Loop through each Integer based PH Form Field record...
	FETCH NEXT FROM FormFieldCursor INTO @currentSectionId, @currentFieldId, @currentFieldTypeId

	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET	@currentTotal = 0
		
		-- Calculate the total for this field record accross all PH Form Values which map to the reference IDs selected earlier...
		SELECT	@currentTotal = SUM(CONVERT(BIGINT, ISNULL(fv.Value, 0)))
		FROM	PH_Form_Value fv
		WHERE	fv.SectionId = @currentSectionId
				AND fv.FieldId = @currentFieldId
				AND fv.FieldTypeId = @currentFieldTypeId
				AND fv.RefId IN (SELECT	r.RefId FROM @ReferenceIds r)
		
		INSERT	INTO	@Totals ([SectionId], [FieldId], [FieldTypeId], [Total])
				VALUES	(@currentSectionId, @currentFieldId, @currentFieldTypeId, ISNULL(@currentTotal, 0))
		
		FETCH NEXT FROM FormFieldCursor INTO @currentSectionId, @currentFieldId, @currentFieldTypeId
	END

	CLOSE FormFieldCursor
	DEALLOCATE FormFieldCursor


	-- OUTPUT TOTALS --
	SELECT	ff.SectionId, ff.FieldId, ff.FieldTypeId, ff.Total
	FROM	@Totals ff
END
GO

