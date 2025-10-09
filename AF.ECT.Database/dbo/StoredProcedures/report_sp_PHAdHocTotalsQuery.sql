
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 6/6/2016
-- Description:	Executes the PH Totals Ad Hoc report.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	12/5/2016
-- Description:		Changed the @userUnit validation check to see if the input
--					parameter is NULL instead of 0. System Admins belong to a 
--					unit that has an ID of 0.
-- ============================================================================
CREATE PROCEDURE [dbo].[report_sp_PHAdHocTotalsQuery]
	@phQueryId INT,
	@userId INT,
	@scope INT,
	@userUnit INT,
	@rptView INT,
	@workflowConditions NVARCHAR(MAX),
	@phFormConditions NVARCHAR(MAX),
	@phFormSortField NVARCHAR(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- VALIDATE INPUT --
	IF (ISNULL(@phQueryId, 0) = 0)
	BEGIN
		RETURN
	END
	
	IF (ISNULL(@userId, 0) = 0)
	BEGIN
		RETURN
	END
	
	IF (ISNULL(@scope, 0) = 0)
	BEGIN
		RETURN
	END
	
	IF (@userUnit IS NULL)
	BEGIN
		RETURN
	END
	
	IF (ISNULL(@rptView, 0) = 0)
	BEGIN
		RETURN
	END
	
	IF (@workflowConditions IS NULL)
	BEGIN
		SET @workflowConditions = '1 = 1'
	END
	
	IF (@phFormConditions IS NULL)
	BEGIN
		SET @phFormConditions = '1 = 1'
	END
	
	IF (ISNULL(@phFormSortField, '') = '')
	BEGIN
		SET @phFormSortField = ''
	END
	ELSE
	BEGIN
		SET @phFormSortField = 'ORDER BY ' + @phFormSortField
	END
	
	IF (@workflowConditions LIKE '%update %' OR @workflowConditions LIKE '%delete %')
	BEGIN
		RETURN
	END
	
	IF (@phFormConditions LIKE '%select %' OR @phFormConditions LIKE '%update %' OR @phFormConditions LIKE '%delete %')
	BEGIN
		RETURN
	END
	
	IF (@phFormSortField LIKE '%select %' OR @phFormSortField LIKE '%update %' OR @phFormSortField LIKE '%delete %')
	BEGIN
		RETURN
	END
	
	-- DECLARE VARIABLES --
	DECLARE @ReferenceIds TABLE
	(
		RefId INT
	)

	CREATE TABLE #Totals
	(
		SectionId INT,
		FieldId INT,
		FieldTypeId INT,
		Total BIGINT
	)

	DECLARE @currentSectionId INT,
			@currentFieldId INT,
			@currentFieldTypeId INT,
			@currentTotal BIGINT,
			@dynamicSQL NVARCHAR(MAX)
	
	-- Create a curosr which will grab all of the Integer based PH Form Field records for the Form Fields specified in the query...
	DECLARE FormFieldCursor CURSOR FOR	SELECT	ff.SectionId, ff.FieldId, ff.FieldTypeId
										FROM	PH_Form_Field ff
												JOIN rpt_ph_user_query_output_fields o ON (ff.SectionId = o.SectionId AND ff.FieldId = o.FieldId AND ff.FieldTypeId = o.FieldTypeId)
												JOIN PH_Section s ON o.SectionId = s.Id
												JOIN PH_FieldType ft ON o.FieldTypeId = ft.Id
												JOIN core_lkupDataType dt ON ft.DataTypeId = dt.Id
										WHERE	o.PHQueryId = @phQueryId
												AND dt.Name = 'Integer'
										ORDER	BY s.DisplayOrder

	-- Get all of the PH cases which meet the report case conditions...
	SET @dynamicSQL = '
			SELECT	s.SC_Id
			FROM	vw_sc_ph_reporting s
			WHERE	(' + @workflowConditions + ')
					AND ((' + CONVERT(NVARCHAR(3), @scope) + ' = 3) OR (' + CONVERT(NVARCHAR(3), @scope) + ' = 2) OR (' + CONVERT(NVARCHAR(3), @scope) + ' = 1 AND s.[PH Wing RMU ID] IN (SELECT child_id FROM Command_Struct_Tree WHERE parent_id = ' + CONVERT(NVARCHAR(25), @userUnit) + ' AND view_type = ' + CONVERT(NVARCHAR(3), @rptView) + ')) OR s.created_by = ' + CONVERT(NVARCHAR(25), @userId) + ')'
	
	INSERT	INTO	@ReferenceIds ([RefId])
			EXEC	sp_executesql @dynamicSQL

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
		
		INSERT	INTO	#Totals ([SectionId], [FieldId], [FieldTypeId], [Total])
				VALUES	(@currentSectionId, @currentFieldId, @currentFieldTypeId, ISNULL(@currentTotal, 0))
		
		FETCH NEXT FROM FormFieldCursor INTO @currentSectionId, @currentFieldId, @currentFieldTypeId
	END

	CLOSE FormFieldCursor
	DEALLOCATE FormFieldCursor


	-- OUTPUT TOTALS --
	SET @dynamicSQL = '
		SELECT	s.Name + '' | '' + f.Name + '' | '' + ft.Name AS [Form Field], ff.Total
		FROM	#Totals ff
				JOIN PH_Section s ON ff.SectionId = s.Id
				JOIN PH_Field f ON ff.FieldId = f.Id
				JOIN PH_FieldType ft ON ff.FieldTypeId = ft.Id
		WHERE	(' + @phFormConditions + ') ' + @phFormSortField
	
	EXEC sp_executesql @dynamicSQL
END
GO

