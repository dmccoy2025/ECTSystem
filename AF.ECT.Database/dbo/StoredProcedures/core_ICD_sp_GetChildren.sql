-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 8/14/2015
-- Description:	Selects all of the immediate children for a specified ICD Code
--				ID. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_ICD_sp_GetChildren]
	@parentId INT,
	@version INT,
	@onlyActive BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Select layer 2+ codes...
	IF @parentId > 0
		BEGIN
		
		SELECT	 ICD9_ID
				,value
				,text
				,isDisease
				,Active
		FROM	core_lkupICD9
		WHERE	parentId = @parentId
				AND ICDVersion = @version
				AND (Active = 1 OR Active = @onlyActive)
				
		END
	-- Select layer 1 codes...
	ELSE
		BEGIN
		
		SELECT	 ICD9_ID
				,value
				,text
				,isDisease
				,Active
		FROM	core_lkupICD9
		WHERE	parentId IS NULL
				AND ICDVersion = @version
				AND (Active = 1 OR Active = @onlyActive)
		
		END
END
GO

