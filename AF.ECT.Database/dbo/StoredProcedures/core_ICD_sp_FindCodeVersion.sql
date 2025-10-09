-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 8/18/2015
-- Description:	Attemps to find the ICD version for the ICD code specified by
--				the code ID.  
-- ============================================================================
CREATE PROCEDURE [dbo].[core_ICD_sp_FindCodeVersion]
	 @codeId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @valid INT = 1
		
	-- Check if the specified value is valid...
	IF @codeId IS NULL
		BEGIN
		SET @valid = 0
		END
	
	-- If everything valid then select the record...
	IF @valid = 1
		BEGIN
		
		SELECT	ICDVersion
		FROM	core_lkupICD9
		WHERE	ICD9_ID = @codeId
				
		END
END
GO

