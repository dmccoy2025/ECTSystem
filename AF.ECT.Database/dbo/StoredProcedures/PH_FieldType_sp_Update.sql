
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 2/26/2016
-- Description:	Updates an existing PH Field Type record in the PH_FieldType
--				table.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	9/11/2017
-- Description:		Modified to pass in the new Length field.
-- ============================================================================
CREATE PROCEDURE [dbo].[PH_FieldType_sp_Update]
	@id INT,
	@name NVARCHAR(100),
	@dataTypeId INT,
	@datasource NVARCHAR(100),
	@placeholder NVARCHAR(25),
	@color NVARCHAR(50),
	@length INT
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
	
	IF (ISNULL(@dataTypeId, 0) = 0)
	BEGIN
		RETURN
	END
	
	IF (@datasource = '')
	BEGIN
		SET @datasource = NULL
	END
	
	IF (@placeholder = '')
	BEGIN
		SET @placeholder = NULL
	END
	
	IF (@color = '')
	BEGIN
		SET @color = NULL
	END
	
	DECLARE @count INT = 0
	
	-- Make sure the PH Field Type being updated actually exists...
	SELECT	@count = COUNT(*)
	FROM	PH_FieldType ft
	WHERE	ft.Id = @id
	
	IF (@count <> 1)
	BEGIN
		RETURN
	END
	
	-- Make sure a PH Field Type with this name does not already exist...
	SET @count = 0
	
	SELECT	@count = COUNT(*)
	FROM	PH_FieldType ft
	WHERE	ft.Name = @name
			AND ft.Id <> @id
	
	IF (@count <> 0)
	BEGIN
		RETURN
	END
	
	BEGIN TRANSACTION
	
		UPDATE	PH_FieldType
		SET		Name = @name,
				DataTypeId = @dataTypeId,
				Datasource = @datasource,
				Placeholder = @placeholder,
				Color = @color,
				[Length] = @length
		WHERE	Id = @id

		IF (@@ERROR <> 0)
		BEGIN
			ROLLBACK TRANSACTION
			RETURN
		END
		
	COMMIT TRANSACTION
END
GO

