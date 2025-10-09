-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 7/2/2015
-- Description:	Deletes the record with the specified Id from the 
--				core_KeyVal_Value table. The Id is differnet then the ValueId
--				of a key value. Id is an auto increment identity value whereas
--				ValueId is a programatically controled value. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_KeyVal_sp_DeleteKeyValueById]
	 @id INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @valid INT = 1

	IF @id IS NULL
		BEGIN
		SET @valid = 0
		END
	
	IF @valid = 1
		BEGIN
		
		-- Delete the record
		DELETE	FROM core_KeyVal_Value
		WHERE	Id = @id
				
		END
END
GO

