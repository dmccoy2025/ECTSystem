
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 2/26/2016
-- Description:	Inserts a new PH Section record into the PH_Section table.
-- ============================================================================
CREATE PROCEDURE [dbo].[PH_Section_sp_Insert]
	@name NVARCHAR(100),
	@parentName NVARCHAR(100),
	@fieldColumns INT,
	@isTopLevel BIT,
	@hasPageBreak BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (ISNULL(@name, '') = '')
	BEGIN
		RETURN
	END
	
	IF (@fieldColumns = 0)
	BEGIN
		RETURN
	END
	
	IF (@hasPageBreak IS NULL)
	BEGIN
		SET @hasPageBreak = 0
	END
	
	DECLARE @count INT = 0
	DECLARE @parentId INT = NULL
	
	-- Make sure a PH Section with this name does not already exist...
	SELECT	@count = COUNT(*)
	FROM	PH_Section s
	WHERE	s.Name = @name
	
	IF (@count <> 0)
	BEGIN
		RETURN
	END
	
	-- Find the parent Id if necessary...
	IF (ISNULL(@isTopLevel, 0) <> 1 AND ISNULL(@parentName, '') <> '')
	BEGIN
		SELECT	@parentId = s.Id
		FROM	PH_Section s
		WHERE	s.Name = @parentName
	END
	
	DECLARE @displayOrder INT = 1
	
	IF (ISNULL(@isTopLevel, 0) = 1)
	BEGIN
		SELECT	TOP 1 @displayOrder = s.DisplayOrder + 1
		FROM	PH_Section s
		WHERE	s.IsTopLevel = 1
		ORDER	BY s.DisplayOrder DESC
	END
	
	IF (@parentId IS NOT NULL)
	BEGIN
		SELECT	TOP 1 @displayOrder = s.DisplayOrder + 1
		FROM	PH_Section s
		WHERE	s.ParentId = @parentId
		ORDER	BY s.DisplayOrder DESC
	END
	
	BEGIN TRANSACTION

		INSERT	INTO	PH_Section ([Name], [ParentId], [FieldColumns], [IsTopLevel], [DisplayOrder], [PageBreak])
				VALUES	(@name, @parentId, @fieldColumns, @isTopLevel, @displayOrder, @hasPageBreak)
			
		IF (@@ERROR <> 0)
		BEGIN
			ROLLBACK TRANSACTION
			RETURN
		END
		
	COMMIT TRANSACTION
END
GO

