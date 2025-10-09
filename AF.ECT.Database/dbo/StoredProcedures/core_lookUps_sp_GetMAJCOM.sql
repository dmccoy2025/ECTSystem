-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[core_lookUps_sp_GetMAJCOM] 
	@id INT = 0,
	@filter INT = 0
AS
BEGIN
	SET NOCOUNT ON;
	
	IF (@id = 0) AND (@filter = 0) -- Get all
		BEGIN
			SELECT ID AS Value, MAJCOM_NAME AS Name, Active FROM core_lkupMAJCOM
			ORDER BY MAJCOM_NAME
		END
		
	ELSE IF @filter = 1 -- Get all Active 
		BEGIN
			SELECT ID AS Value, MAJCOM_NAME AS Name FROM core_lkupMAJCOM
			WHERE Active = 1
		END	
		
	ELSE IF @id > 0 -- Get one
		BEGIN 
			SELECT ID AS Value, MAJCOM_NAME AS Name FROM core_lkupMAJCOM
			WHERE ID = @id				
		END
	
END
GO

