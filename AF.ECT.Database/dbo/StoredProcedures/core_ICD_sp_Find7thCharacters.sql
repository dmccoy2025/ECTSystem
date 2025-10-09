-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 8/18/2015
-- Description:	Attemps to find the set of 7th characters associated with the
--				specified ICD code ID.  
-- ============================================================================
CREATE PROCEDURE [dbo].[core_ICD_sp_Find7thCharacters]
	 @codeId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @valid INT = 1
		
	-- Check if the specified value is valid...
	IF @codeId IS NULL OR @codeId = 0
		BEGIN
		SET @valid = 0
		END
	
	-- If everything valid then select the record...
	IF @valid = 1
		BEGIN
		
		DECLARE @selectedCodeId INT = 0;

		DECLARE @hierarchy TABLE
		(
			ICD9_ID INT,
			parentId INT,
			value VARCHAR(7),
			text VARCHAR(250),
			SeventhChr BIT,
			recurseLevel INT
		);

		WITH codes( id, ptr, value, text, SeventhChr, level ) AS 
		(		SELECT 
					ICD9_ID, 
					parentId, 
					value,
					text,
					SeventhChr,
					0 AS level
				FROM 
					core_lkupICD9 t1 
				WHERE 
					ICD9_ID = @codeId
			UNION ALL 
				SELECT 
					t1.ICD9_ID, 
					t1.parentId, 
					t1.value,
					t1.text,
					t1.SeventhChr,
					level + 1
				FROM 
					core_lkupICD9 t1 JOIN codes ON codes.ptr = t1.ICD9_ID
		) 
		INSERT INTO @hierarchy
		SELECT * FROM codes

		SELECT	TOP 1 @selectedCodeId = h.ICD9_ID
		FROM	@hierarchy h
		WHERE	h.SeventhChr = 1

		SELECT	c.Char_Ext AS Character, c.Char_Ext + ' - ' + c.Char_Def AS Definition
		FROM	core_lkupICD9 i
				INNER JOIN core_lkupICD7thChar c ON i.value = c.Char_code
		WHERE	i.ICD9_ID = @selectedCodeId
				
		END
END
GO

