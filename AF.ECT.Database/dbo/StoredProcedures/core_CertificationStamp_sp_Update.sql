
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 1/22/2016
-- Description:	Updates an existing Certification Stamp record in the 
--				core_CertificationStamp table.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_CertificationStamp_sp_Update]
	@id INT,
	@name NVARCHAR(100),
	@body NVARCHAR(500),
	@isQualified BIT
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
	
	IF (ISNULL(@body, '') = '')
	BEGIN
		RETURN
	END
	
	IF (@isQualified IS NULL)
	BEGIN
		RETURN
	END
	
	
	DECLARE @count INT = 0
	
	-- Make sure the stamp being updated actually exists...
	SELECT	@count = COUNT(*)
	FROM	core_CertificationStamp cs
	WHERE	cs.Id = @id
	
	IF (@count <> 1)
	BEGIN
		RETURN
	END
	
	-- Check if a stamp with the new name already exists...
	SET @count = 0
	
	SELECT	@count = COUNT(*)
	FROM	core_CertificationStamp cs
	WHERE	cs.Name = @name
			AND cs.Id <> @id
	
	IF (@count <> 0)
	BEGIN
		RETURN
	END
	
	-- Update the record...
	UPDATE	core_CertificationStamp
	SET		Name = @name, 
			Body = @body, 
			IsQualified = @isQualified
	WHERE	Id = @id
END
GO

