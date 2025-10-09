
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 1/22/2016
-- Description:	Inserts a new Certifcation Stamp record into the
--				core_CertificationStamp table.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_CertificationStamp_sp_Insert]
	@name NVARCHAR(100),
	@body NVARCHAR(500),
	@isQualified BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
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
	
	SELECT	@count = COUNT(*)
	FROM	core_CertificationStamp cs
	WHERE	cs.Name = @name
	
	IF (@count <> 0)
	BEGIN
		RETURN
	END
	
	INSERT	INTO core_CertificationStamp ([Name], [Body], [IsQualified])
			VALUES (@name, @body, @isQualified)
END
GO

