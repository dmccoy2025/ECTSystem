-- ============================================================================
-- Author:		Nandita Srivastav 
-- Description:	Get Next CaseId  
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	11/27/2015
-- Work Item:		TFS Task 289
-- Description:		Changed the size of @caseID, @orgCaseId, @lastAppealCaseId, 
--					@newCaseId, and the return value from 20 to 50.
-- ============================================================================
CREATE FUNCTION [dbo].[GetNextCaseId] 
(
	-- Add the parameters for the function here
	@lodid  int
)
RETURNS varchar(50) 
AS

BEGIN

		DECLARE @ssn varchar(9) ,@count int , @caseId varchar(50)  
		,@orgCaseId varchar(50),@lastAppealCaseId  varchar(50),  @lastLetter char(1) 
		,@newLetter char(1),@newCaseId varchar(50)
		
		SELECT   @orgCaseId=case_id,@ssn=member_ssn from  form348 where lodid=@lodid 
		if (dbo.IsReInvestigation(@orgCaseId)=0)
		/*The case id is not an appeal Id*/
			BEGIN
				SET @count=(SELECT  COUNT(*) FROM  form348 WHERE  case_id like '%' +@orgCaseId +'%' AND LEN(case_id)<= LEN(@orgCaseId)+2
						AND member_ssn=@ssn
						 ) 
			 
			END 
		ELSE
			BEGIN 
				SET @count=(SELECT  COUNT(*) FROM  form348 WHERE  case_id like '%' +@orgCaseId +'%' AND LEN(case_id)<= LEN(@orgCaseId)+1
						AND member_ssn=@ssn
						 ) 
				 
			END 
		 
	
		IF(@count=1)
		/* There has never been an appeal*/
			BEGIN 
		
				IF (dbo.IsReInvestigation(@orgCaseId)=0)
				BEGIN
					SET @newCaseId=	@orgCaseId+ '-A'  
				END 
				Else
				BEGIN
					SET @newCaseId=	@orgCaseId+ 'A' 
				END 
			END 
		ELSE
		/* There has been an appeal.Get the last appeal Id */
			BEGIN
				IF(dbo.IsReInvestigation(@orgCaseId)=0)
					BEGIN 
			 				SET @lastAppealCaseId =(SELECT   TOP 1 case_id from  form348 WHERE   case_id like '%' +@orgCaseId +'%' AND LEN(case_id)<= LEN(@orgCaseId)+2
										ORDER BY CREATED_DATE DESC ) 
					END 
				ELSE
					BEGIN 
						SET @lastAppealCaseId =(SELECT   TOP 1 case_id from  form348 WHERE   case_id like '%' +@orgCaseId +'%'  AND LEN(case_id)<= LEN(@orgCaseId)+1
													ORDER BY CREATED_DATE DESC )
				
		 			END 
				SET @lastLetter = SUBSTRING (@lastAppealCaseId,LEN(@lastAppealCaseId) , 1) 
				SET @newLetter=dbo.GetNextLetter(@lastLetter)
				SET @newCaseId = SUBSTRING (@lastAppealCaseId,1 , LEN(@lastAppealCaseId)-1) + @newLetter
 
			END 
			 
	RETURN  @newCaseId

END
GO

