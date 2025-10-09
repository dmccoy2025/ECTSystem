--Execute imp_CreateUserRoles
 CREATE PROCEDURE [dbo].[imp_ReplaceTilda]
AS

 BEGIN --BEGIN 
 
	declare @input nvarchar(4), @output as nvarchar(4)
	set @input = '\r\r';
	set @output = '\r\n' --CHAR(10);
	
 --   UPDATE CORE_USERS SET COMMENT=REPLACE(COMMENT, @input, @output)	
    
			
	
	--UPDATE form348 SET 
	--med_tech_comments =REPLACE(med_tech_comments, @input, @output)
	--,rwoa_explantion=REPLACE(rwoa_explantion, @input, @output)
	--,io_instructions=REPLACE(io_instructions, @input, @output)
	--,io_poc_info		=REPLACE(io_poc_info, @input, @output)
	
	UPDATE form348_MEDICAL SET 
	medical_facility =REPLACE(medical_facility, @input, @output)
	,physician_cancel_explanation =REPLACE(physician_cancel_explanation, @input, @output)
	where lodid = 9628;
	 
	--UPDATE form348_UNIT SET 
	-- cmdr_duty_others  =REPLACE(cmdr_duty_others, @input, @output)
	 
	--UPDATE Rwoa  SET 
	--explanation_for_sending_back=REPLACE(explanation_for_sending_back, @input, @output)
	--,comments_back_to_sender=REPLACE(comments_back_to_sender, @input, @output)
	
	--UPDATE FORM348_findings 
	--SET explanation =REPLACE(explanation, @input, @output)
 END--End Stored Proc
GO

