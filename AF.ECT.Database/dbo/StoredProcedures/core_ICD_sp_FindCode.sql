-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 8/15/2015
-- Description:	Attemps to find the specified ICD code.  
-- ============================================================================
CREATE PROCEDURE [dbo].[core_ICD_sp_FindCode]
	 @value NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @valid INT = 1
		
	-- Check if the specified value is valid...
	IF @value IS NULL OR @value = ''
		BEGIN
		SET @valid = 0
		END
	
	-- If everything valid then select the record...
	IF @valid = 1
		BEGIN
		
		SELECT	ICD9_ID
		FROM	core_lkupICD9
		WHERE	value = @value
				
		END
END
GO

