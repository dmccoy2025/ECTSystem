-- =============================================
-- Author:		Nandita Srivastava
-- Create date: June 9  2008
-- Description:	Selects the Lod Investigation Other Personnel Information 
-- =============================================

--Execute Form261_sp_Investigation_IOAASelect 1
CREATE PROCEDURE [dbo].[Form261_sp_Investigation_IOAASelect]
		@lodid int
AS
/*
BEGIN

	Declare @aaperson  xml,@aaUserId int
	Declare @aaPOC varchar(400),@ioInstructions varchar(1000),@ioCompDate dateTime 
	SELECT  @aaUserId=appAuthUserId, @aaperson =appAuth_Personnel FROM  Form348 WHERE   lodId=@lodid  
 	
	Declare @ioperson  xml, @ioUserId int
	SELECT  @ioUserId=ioUserId, @ioperson =ioPersonnel 
			,@aaPOC=app_auth_poc
			,@ioCompDate=io_completionDate
			,@ioInstructions=io_instructions
			  FROM   Form261 WHERE   lodId=@lodid  
 	
	 	
	 --Prepare input values as an XML documnet
	DECLARE @aaDoc INT ,  @ioDoc INT  
	EXEC sp_xml_preparedocument @aaDoc OUTPUT, @aaperson
	EXEC sp_xml_preparedocument @ioDoc OUTPUT, @ioperson
	
	SELECT @aaUserId as aaUserId,aa.ssn,aa.name,aa.grade,aa.branch ,aa.pascode,aa.pascodeDescription 
		 ,@ioUserId as ioUserId,io.ssn,io.name,io.grade,io.branch,io.pascode,io.pascodeDescription  
		 ,@aaPOC,@ioInstructions,@ioCompDate
		FROM OPENXML (@aaDoc, '/Personnel',1)  
			WITH   (ssn varchar(9),name varchar(100),grade varchar(6)
			,pascode varchar(8),pascodeDescription varchar(100),branch varchar(20)) as aa,
		OPENXML (@ioDoc, '/Personnel',1)  
				WITH   (ssn varchar(9),name varchar(100),grade varchar(6)
			,pascode varchar(8),pascodeDescription varchar(100),branch varchar(20)) as io
	
-- Rollback the transaction if there were any errors
 	EXEC sp_xml_removedocument @aaDoc
	EXEC sp_xml_removedocument @ioDoc


END

*/
GO

