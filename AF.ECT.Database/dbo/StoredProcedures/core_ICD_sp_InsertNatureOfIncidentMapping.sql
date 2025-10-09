-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 9/8/2015
-- Description:	Inserts a new ICD to Nature of Incident mapping.  
-- ============================================================================
CREATE PROCEDURE [dbo].[core_ICD_sp_InsertNatureOfIncidentMapping]
	 @codeId INT,
	 @NOIValue NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @noiId INT = 0
	
	SELECT	@noiId = Id
	FROM	core_lkupNatureOfIncident
	WHERE	Value = @NOIValue
	
	IF @codeId <> 0 AND @noiId <> 0
		BEGIN
		
		INSERT INTO core_ICD_NatureOfIncident ([ICD_Id], [NOI_Id])
		VALUES (@codeId, @noiId)
		
		END
END
GO

