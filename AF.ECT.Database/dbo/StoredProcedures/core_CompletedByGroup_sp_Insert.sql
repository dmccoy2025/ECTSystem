
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 2/2/2016
-- Description:	Inserts a new Completed By Group record into the
--				core_CompletedByGroup table.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_CompletedByGroup_sp_Insert]
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
	FROM	core_CompletedByGroup cbg
	WHERE	cbg.Name = @name
	
	IF (@count <> 0)
	BEGIN
		RETURN
	END
	
	INSERT	INTO core_CompletedByGroup ([Name])
			VALUES (@name)
END
GO

