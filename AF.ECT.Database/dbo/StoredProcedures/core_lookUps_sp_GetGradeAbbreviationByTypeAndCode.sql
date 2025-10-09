
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 11/11/2016
-- Description:	Inserts a new PH Field record into the PH_Field table.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookUps_sp_GetGradeAbbreviationByTypeAndCode]
	@abbreviationTypeName NVARCHAR(100),
	@gradeCode INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@abbreviationTypeName, '') = '')
	BEGIN
		RETURN
	END

	IF (ISNULL(@gradeCode, -1) = -1)
	BEGIN
		RETURN
	END

	IF (NOT EXISTS (SELECT * FROM core_lkupGrade g WHERE g.CODE = @gradeCode))
	BEGIN
		RETURN
	END

	DECLARE @abbreviationTypeId INT = 0

	SET @abbreviationTypeId = (SELECT gat.Id FROM core_lkupGradeAbbreviationType gat WHERE gat.Name = @abbreviationTypeName)

	IF (ISNULL(@abbreviationTypeId, 0) = 0)
	BEGIN
		RETURN
	END

	SELECT	ga.Abbreviation
	FROM	GradeAbbreviation ga
	WHERE	ga.GradeCode = @gradeCode
			AND ga.AbbreviationTypeId = @abbreviationTypeId

	--SELECT	TOP 1 ga.Abbreviation
	--FROM	GradeAbbreviation ga
	--		JOIN core_lkupGrade g ON ga.GradeCode = g.CODE
	--		JOIN core_lkupGradeAbbreviationType gat ON ga.AbbreviationTypeId = gat.Id
	--WHERE	ga.CODE = @gradeCode
	--		AND gat.Name = @abbreviationTypeName
END
GO

