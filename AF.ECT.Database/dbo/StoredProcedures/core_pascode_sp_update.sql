
 -- =============================================
-- Author:		Nandita Srivastava
-- Create date: Feb20th 2009
-- Description:	 Procedure to update information reagarding the   pascode
-- =============================================
	CREATE PROCEDURE [dbo].[core_pascode_sp_update]
		 
		 
		 @CS_ID int  =null 
		,@LONG_NAME nvarchar(100)  =null 
		,@UNIT_NBR  nvarchar(4) =null 
		,@UNIT_KIND  nvarchar(5) =null 
		,@UNIT_TYPE  nvarchar(2) =null 
		,@UNIT_DET  nvarchar(4) =null 
		,@UIC  nvarchar(6) =null 
		,@CS_LEVEL  nvarchar(10) =null 
		,@CS_ID_PARENT  int =null 
		,@GAINING_COMMAND_CS_ID int =null 
		,@PAS_CODE  nvarchar(4) =null 
		,@BASE_CODE  nvarchar(2) =null 
		,@CS_OPER_TYPE  nvarchar(2) =null 
		,@TIME_ZONE  nvarchar(3) =null 
		,@COMPONENT nvarchar(10)  =null 
		,@GEO_LOC  nvarchar(4) =null 
		,@PHYS_EXAM_YN  nvarchar(1) =null 
		,@SCHEDULING_YN  nvarchar(1) =null 
		,@ADDRESS1  nvarchar(100) =null 
		,@ADDRESS2  nvarchar(100) =null 
		,@CITY  nvarchar(30) =null 
		,@COUNTRY  nvarchar(40) =null 
		,@STATE  nvarchar(2) =null 
		,@POSTAL_CODE  nvarchar(10) =null 
		,@E_MAIL  nvarchar(40) =null 


	AS


 

	BEGIN 
--Fetch the parent info
	UPDATE COMMAND_STRUCT
		SET 				
			
			LONG_NAME =@LONG_NAME  
			,UNIT_NBR=@UNIT_NBR  
			,UNIT_KIND=@UNIT_KIND  
			,UNIT_TYPE=@UNIT_TYPE  
			,UNIT_DET=@UNIT_DET  
			,UIC=@UIC  
			,CS_LEVEL=@CS_LEVEL  
			,CS_ID_PARENT=@CS_ID_PARENT  
			,GAINING_COMMAND_CS_ID=@GAINING_COMMAND_CS_ID 
			,PAS_CODE=@PAS_CODE  
			,BASE_CODE=@BASE_CODE  
			,CS_OPER_TYPE=@CS_OPER_TYPE  
			,TIME_ZONE=@TIME_ZONE  
			,COMPONENT=@COMPONENT   
			,GEO_LOC=@GEO_LOC  
			,PHYS_EXAM_YN=@PHYS_EXAM_YN  
			,SCHEDULING_YN=@SCHEDULING_YN  
			,ADDRESS1=@ADDRESS1  
			,ADDRESS2=@ADDRESS2  
			,CITY=@CITY  
			,COUNTRY=@COUNTRY  
			,STATE=@STATE  
			,POSTAL_CODE=@POSTAL_CODE  
			,E_MAIL=@E_MAIL  

 		WHERE 
				CS_ID =@CS_ID   
	END
GO

