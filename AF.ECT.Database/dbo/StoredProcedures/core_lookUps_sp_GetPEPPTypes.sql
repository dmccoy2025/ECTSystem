-- =============================================
-- Author:		Kenneth Barnett
-- Create date: 6/16/2014
-- Description:	Returns the records in the core_lkupPEPPType table
-- =============================================
CREATE PROCEDURE [dbo].[core_lookUps_sp_GetPEPPTypes]  
	@id INT = 0,
	@filter INT = 0
AS
BEGIN
	SET NOCOUNT ON;
    
	IF (@id = 0) AND (@filter = 0) -- Get all
		BEGIN
			SELECT		typeID AS Value, typeName AS Name, active 
			FROM		core_lkupPEPPType
			ORDER BY	typeName
		END
		
	ELSE IF @filter = 1 -- Get all Active 
		BEGIN
			SELECT		typeID AS Value, typeName AS Name, active
			FROM		core_lkupPEPPType
			WHERE		Active = 1
			ORDER BY	typeName
		END	
		
	ELSE IF @id > 0 -- Get one
		BEGIN 
			SELECT		typeID AS Value, typeName AS Name 
			FROM		core_lkupPEPPType
			WHERE		typeID = @id				
		END
END
GO

