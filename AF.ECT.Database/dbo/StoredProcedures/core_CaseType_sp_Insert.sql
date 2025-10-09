
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 1/25/2016
-- Description:	Inserts a new Case Type record into the core_CaseType table.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_CaseType_sp_Insert]
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
	FROM	core_CaseType ct
	WHERE	ct.Name = @name
	
	IF (@count <> 0)
	BEGIN
		RETURN
	END
	
	INSERT	INTO core_CaseType ([Name])
			VALUES (@name)
END
GO

