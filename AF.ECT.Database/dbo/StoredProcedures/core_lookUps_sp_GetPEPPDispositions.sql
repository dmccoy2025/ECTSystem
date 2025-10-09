-- =============================================
-- Author:		Kenneth Barnett
-- Create date: 6/16/2014
-- Description:	Returns the records in the core_lkupPEPPDisposition table
-- =============================================
CREATE PROCEDURE [dbo].[core_lookUps_sp_GetPEPPDispositions]  
	@id INT = 0,
	@filter INT = 0
AS
BEGIN
	SET NOCOUNT ON;

    --SELECT		dispositionId AS Value, dispositionName AS Name, active
    --FROM		core_lkupPEPPDisposition
    --WHERE		active = 1
    --ORDER BY	dispositionName
    
	IF (@id = 0) AND (@filter = 0) -- Get all
		BEGIN
			SELECT		dispositionId AS Value, dispositionName AS Name, active 
			FROM		core_lkupPEPPDisposition
			ORDER BY	dispositionName
		END
		
	ELSE IF @filter = 1 -- Get all Active 
		BEGIN
			SELECT		dispositionId AS Value, dispositionName AS Name, active
			FROM		core_lkupPEPPDisposition
			WHERE		Active = 1
			ORDER BY	dispositionName
		END	
		
	ELSE IF @id > 0 -- Get one
		BEGIN 
			SELECT		dispositionId AS Value, dispositionName AS Name 
			FROM		core_lkupPEPPDisposition
			WHERE		dispositionId = @id				
		END
END
GO

