-- =============================================
-- Author:		Nandita Srivastav 
 -- Description:	Get Next letter  
-- =============================================
CREATE FUNCTION [dbo].[GetNextLetter] 
(
	-- Add the parameters for the function here
	@letter char(1)
)
RETURNS char(1)
AS

BEGIN
	DECLARE @next as char(1)

	SET @next=CASE 	@letter
		 
			WHEN  'A' THEN 'B'
		    WHEN  'B' THEN 'C'
			WHEN  'C' THEN 'D'
			WHEN  'D' THEN 'E'
			WHEN  'E' THEN 'F'
			WHEN  'F' THEN 'G'
			WHEN  'G' THEN 'H'
			WHEN  'H' THEN 'I'
			WHEN  'I' THEN 'J'
			WHEN  'J' THEN 'K'
			WHEN  'K' THEN 'L'
			WHEN  'L' THEN 'M'
			WHEN  'M' THEN 'N'
			WHEN  'N' THEN 'O'
			WHEN  'O' THEN 'P'
			WHEN  'P' THEN 'Q'
			WHEN  'Q' THEN 'R'
			WHEN  'R' THEN 'S'
			WHEN  'S' THEN 'T'
			WHEN  'T' THEN 'U'
			WHEN  'U' THEN 'V'
			WHEN  'V' THEN 'W'
			WHEN  'W' THEN 'X'
			WHEN  'X' THEN 'Y'
			WHEN  'Y' THEN 'Z'
			ELSE 'A'

		
		END 

		RETURN  @next

END
GO

