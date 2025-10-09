-- =============================================
-- Author:		Kenneth Barnett
-- Create date: 6/18/2014
-- Description:	Inserts a new record into or updates an existing record in core_lkupPEPPType
-- =============================================
CREATE PROCEDURE [dbo].[core_lookups_sp_Insert_PEPPType] 
	@id INT = NULL,
	@typeName VARCHAR(50) = '',
	@active bit = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	IF (@id IS NOT NULL) AND (@id <> 0)
		BEGIN
			UPDATE	core_lkupPEPPType 
			SET		typeName = @typeName, active = @active
			WHERE	typeId = @id
		
		END
	 	
	ELSE
		DECLARE @name VARCHAR(50)
			
		SELECT	@name = typeName 
		FROM	core_lkupPEPPType
		WHERE	typeName = @typeName	
		
		IF @name is null
			BEGIN
				INSERT INTO core_lkupPEPPType(typeName)
				VALUES(@typeName)
			END

END
GO

