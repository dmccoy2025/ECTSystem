
-- =============================================
-- Author:		Kenneth Barnett
-- Create date: 3/25/2015
-- Description: Selects data members which have the same or similar
--				values for their last name, first name, and middle
--				name as the values passed into the stored procedure.
-- =============================================
CREATE PROCEDURE [dbo].[member_sp_GetMembersByName]  
	 @lastName VARCHAR(50)
	,@firstName VARCHAR(50)
	,@middleName VARCHAR(60)
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @middleIncluded BIT
	SET @middleIncluded = 1

	IF @lastName = ''
		BEGIN
		SET @lastName = NULL
		END
		
	IF @firstName = ''
		BEGIN
		SET @firstName = NULL
		END
		
	IF @middleName IS NULL OR @middleName = ''
		BEGIN
		SET @middleName = NULL
		SET @middleIncluded = 0
		END

	IF @middleIncluded = 1
		BEGIN
		
		SELECT		md.LAST_NAME AS LastName, md.FIRST_NAME AS FirstName, md.MIDDLE_NAMES AS MiddleName, md.SSN AS SSN, md.RANK AS Rank, cs.LONG_NAME AS Unit
		FROM		VW_MEMBERDATA md LEFT JOIN
					Command_Struct AS cs ON cs.PAS_CODE = md.PAS_NUMBER
		WHERE		md.LAST_NAME LIKE '%' + IsNull(@lastName, md.LAST_NAME) + '%'
					AND md.FIRST_NAME LIKE '%' + IsNull(@firstName, md.FIRST_NAME) + '%'
					AND md.MIDDLE_NAMES LIKE '%' + IsNull(@middleName, md.MIDDLE_NAMES) + '%'
		ORDER BY	md.LAST_NAME, md.FIRST_NAME
		
		END
	ELSE
		BEGIN
		
		SELECT		md.LAST_NAME AS LastName, md.FIRST_NAME AS FirstName, md.MIDDLE_NAMES AS MiddleName, md.SSN AS SSN, md.RANK AS Rank, cs.LONG_NAME AS Unit
		FROM		VW_MEMBERDATA md LEFT JOIN
					Command_Struct AS cs ON cs.PAS_CODE = md.PAS_NUMBER
		WHERE		md.LAST_NAME LIKE '%' + IsNull(@lastName, md.LAST_NAME) + '%'
					AND md.FIRST_NAME LIKE '%' + IsNull(@firstName, md.FIRST_NAME) + '%'
		ORDER BY	md.LAST_NAME, md.FIRST_NAME
		
		END
END
GO

