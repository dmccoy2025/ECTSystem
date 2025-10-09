-- =============================================
-- Author:		Kenneth Barnett
-- Create date: 6/18/2014
-- Description:	Inserts a new record into or updates an existing record in core_lkupPEPPDisposition
-- =============================================
CREATE PROCEDURE [dbo].[core_lookups_sp_Insert_PEPPDisposition] 
	@id INT = NULL,
	@dispositionName VARCHAR(50) = '',
	@active bit = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	IF (@id IS NOT NULL) AND (@id <> 0)
		BEGIN
			UPDATE	core_lkupPEPPDisposition
			SET		dispositionName = @dispositionName, active = @active
			WHERE	dispositionId = @id
		
		END
	 	
	ELSE
		DECLARE @name VARCHAR(50)
			
		SELECT	@name = dispositionName 
		FROM	core_lkupPEPPDisposition
		WHERE	dispositionName = @dispositionName	
		
		IF @name is null
			BEGIN
				INSERT INTO core_lkupPEPPDisposition(dispositionName)
				VALUES(@dispositionName)
			END

END
GO

