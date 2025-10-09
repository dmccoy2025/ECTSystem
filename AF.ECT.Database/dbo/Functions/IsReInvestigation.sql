-- ============================================================================
-- Author:		Nandita Srivastav 
-- Description:	Get Next letter  
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	11/27/2015
-- Work Item:		TFS Task 289
-- Description:		Changed the size of @caseID from 20 to 50.
-- ============================================================================
CREATE FUNCTION [dbo].[IsReInvestigation] 
(
	-- Add the parameters for the function here
	@caseId varchar(50) 
)
RETURNS bit 
AS
BEGIN 
	DECLARE @lastLetter as   CHAR(1) 
	DECLARE	@isAppealID as  bit 

 	SET @lastLetter = SUBSTRING (@caseId,LEN(@caseId) , 1) 
 	 	
		IF (@lastLetter in('A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'))
			set @isAppealID= 1 
		else
			set @isAppealID= 0
	 
	RETURN @isAppealID
END
GO

