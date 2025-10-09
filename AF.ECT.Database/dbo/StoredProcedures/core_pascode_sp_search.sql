
 -- =============================================
-- Author:		Nandita Srivastava
-- Create date: Feb20th 2009
-- Description:	 Procedure to search unit 
-- =============================================
-- exec core_pascode_sp_search 'F1B0',' '
	CREATE PROCEDURE [dbo].[core_pascode_sp_search]		 
		 
		 
		 @pascode nvarchar(4),
		 @longName nvarchar(100),
		 @activeOnly bit 
 
		 


	AS
		IF @pascode='' SET @pascode=null 
		IF @longName='' SET @longName=null 
		
	BEGIN
		SELECT 
			 CS_ID,     LONG_NAME +'('+PAS_CODE+ ')'  as LONG_NAME,PAS_CODE
		 FROM 
			COMMAND_STRUCT cs
		WHERE
			(PAS_CODE LIKE '%'+ ISNULL(@pascode , PAS_CODE) + '%') 
		 AND 
			(LONG_NAME LIKE '%'+ ISNULL(@longName , LONG_NAME) + '%') 
	 	AND 
		CASE 
		 WHEN @activeOnly =1 THEN 0
		 ELSE Inactive
		 END
		 =	Inactive	 
		 
	END
GO

