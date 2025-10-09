
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 2/2/2016
-- Description:	Updates an existing Completed By Group record in the 
--				core_CompletedByGroup table.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_CompletedByGroup_sp_Update]
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
	
	-- Make sure the record being updated actually exists...
	SELECT	@count = COUNT(*)
	FROM	core_CompletedByGroup cbg
	WHERE	cbg.Id = @id
	
	IF (@count <> 1)
	BEGIN
		RETURN
	END
	
	-- Check if a record with the new name already exists...
	SET @count = 0
	
	SELECT	@count = COUNT(*)
	FROM	core_CompletedByGroup cbg
	WHERE	cbg.Name = @name
			AND cbg.Id <> @id
	
	IF (@count <> 0)
	BEGIN
		RETURN
	END
	
	-- Update the record...
	UPDATE	core_CompletedByGroup
	SET		Name = @name
	WHERE	Id = @id
END
GO

