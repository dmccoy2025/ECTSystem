
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 1/25/2016
-- Description:	Inserts a new Sub Case Type record into the core_SubCaseType 
--				table.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_CaseType_sp_InsertSubCaseType]
	@name NVARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@name, '') = '')
	BEGIN
		RETURN
	END
	
	DECLARE @count INT = 0
	
	SELECT	@count = COUNT(*)
	FROM	core_SubCaseType sct
	WHERE	sct.Name = @name
	
	IF (@count <> 0)
	BEGIN
		RETURN
	END
	
	INSERT	INTO core_SubCaseType ([Name])
			VALUES (@name)
END
GO

