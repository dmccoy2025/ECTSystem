
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 1/25/2016
-- Description:	Updates an existing Sub Case Type record in the
--				core_SubCaseType table.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_CaseType_sp_UpdateSubCaseType]
	@id INT,
	@name NVARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@id, 0) = 0)
	BEGIN
		RETURN
	END
	
	IF (ISNULL(@name, '') = '')
	BEGIN
		RETURN
	END
	
	DECLARE @count INT = 0
	
	-- Make sure the sub case type being updated actually exists...
	SELECT	@count = COUNT(*)
	FROM	core_SubCaseType sct
	WHERE	sct.Id = @id
	
	IF (@count <> 1)
	BEGIN
		RETURN
	END
	
	-- Check if a sub case type with the new name already exists...
	SET @count = 0
	
	SELECT	@count = COUNT(*)
	FROM	core_SubCaseType sct
	WHERE	sct.Name = @name
			AND sct.Id <> @id
	
	IF (@count <> 0)
	BEGIN
		RETURN
	END
	
	-- Update the record...
	UPDATE	core_SubCaseType
	SET		Name = @name
	WHERE	Id = @id
END
GO

