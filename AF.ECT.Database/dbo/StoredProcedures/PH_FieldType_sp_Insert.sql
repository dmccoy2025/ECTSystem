
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 2/26/2016
-- Description:	Inserts a new PH Field Type record into the PH_FieldType table.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	9/11/2017
-- Description:		Modified to pass in the new Length field.
-- ============================================================================
CREATE PROCEDURE [dbo].[PH_FieldType_sp_Insert]
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
	
	-- Make sure a PH Field Type with this name does not already exist...
	SELECT	@count = COUNT(*)
	FROM	PH_FieldType ft
	WHERE	ft.Name = @name
	
	IF (@count <> 0)
	BEGIN
		RETURN
	END
	
	BEGIN TRANSACTION
	
		INSERT	INTO	PH_FieldType ([Name], [DataTypeId], [Datasource], [Placeholder], [Color], [Length])
				VALUES (@name, @dataTypeId, @datasource, @placeholder, @color, @length)

		IF (@@ERROR <> 0)
		BEGIN
			ROLLBACK TRANSACTION
			RETURN
		END
		
	COMMIT TRANSACTION
END
GO

