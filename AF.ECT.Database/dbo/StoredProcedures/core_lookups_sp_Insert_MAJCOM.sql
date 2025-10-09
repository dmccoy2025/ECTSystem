-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[core_lookups_sp_Insert_MAJCOM] 
	@id INT = NULL,
	@majcomName VARCHAR(20) = '',
	@active bit = NULL
	
AS
BEGIN
	SET NOCOUNT ON;
	
	IF (@id IS NOT NULL) AND (@id <> 0)
		BEGIN
			UPDATE core_lkupMAJCOM SET MAJCOM_NAME = @majcomName, Active = @active
			WHERE ID = @id
		
		END
	 	
	ELSE
		DECLARE @name VARCHAR(20)	
		SELECT @name = MAJCOM_NAME FROM core_lkupMAJCOM WHERE MAJCOM_NAME = @majcomName	
		IF @name is null
			BEGIN
				INSERT INTO core_lkupMAJCOM(MAJCOM_NAME)
				VALUES(UPPER(@majcomName))
			END

END
GO

