--Execute imp_updateCaseIds 
--Excution time 2 minutes for 9000 lods
--select  * from imp_lodmapping where case_id is not  null order by createddate
CREATE PROCEDURE [dbo].[imp_updateCaseIds]
AS
 
DECLARE        @lod_id    int 
DECLARE        @createDateString   varchar (200)  
declare	@created as datetime
DECLARE       @caseId  varchar (200)  
DECLARE       @lod_num   int 
         
 
UPDATE imp_LODMAPPING set case_id=null   
	  
 DECLARE createDates CURSOR FOR   
	SELECT distinct CONVERT(varchar(10), created_date ,111) as created_date FROM imp_lod_dispositions  
 		 
OPEN createDates
FETCH NEXT FROM createDates  INTO @createDateString 
		 
WHILE @@FETCH_STATUS = 0
BEGIN --BEGIN DATE CURSOR 

	BEGIN  TRY 

		SET @lod_num=0
    	DECLARE lod_cursor CURSOR FOR      	 
    		SELECT lod_id FROM imp_lod_dispositions 
			where DATEDIFF(dd, 0, created_date) = cast(@createDateString as datetime) order by created_date asc
     			  
     	OPEN lod_cursor
    	FETCH NEXT FROM lod_cursor INTO @lod_id 
    	
    	WHILE @@FETCH_STATUS = 0	
    						  
    	BEGIN --BEGIN LOD CURSOR 
    	
    		SET @lod_num=@lod_num+1 
    		--SET @caseId= replace(@createDateString, '/','_')+ '_'+ cast( @lod_num as varchar(10))
    		
    		select @created = Cast(@createDateString as datetime);
    		
    		set @caseId = right('0000' + cast(datepart(year,@created) as varchar(4)), 4) +
			 right('00' + cast(datepart(month,@created) as varchar(2)), 2) +
			 right('00' + cast(datepart(day,@created) as varchar(2)), 2) +
			 '-' + right('000' + cast(@lod_num as varchar(5)), 3);
    		
    		UPDATE imp_LODMAPPING
			SET case_id=@caseId				 
			WHERE rcpha_lodid=@lod_id 
												
			FETCH NEXT FROM lod_cursor INTO @lod_id 
    										 
    	END --END LOD CURSOR
    							
    	CLOSE lod_cursor
		DEALLOCATE lod_cursor
			
		UPDATE imp_LODMAPPING 
		SET case_id= ( SELECT case_id FROM imp_LODMAPPING a WHERE a.rcpha_lodid=b.rc_pha_org_lodid )+'_A'
		from imp_LODMAPPING b
		WHERE rc_pha_org_lodid IS NOT NULL 
	END TRY 	
	BEGIN CATCH 
	
		DECLARE @number int ,@errmsg varchar(2000)
		SELECT 
			@number= ERROR_NUMBER()  
			,@errmsg= ERROR_MESSAGE()	
							
		EXECUTE imp_InsertErrorRecord @number 
			,'LOD CASE ID UPDATE','imp_updateCaseIds ',@createDateString,@lod_id,@errmsg
			 	  	  		
	END CATCH     		


FETCH NEXT FROM createDates  INTO @createDateString 
    		  
END   --END DATE CURSOR

CLOSE createDates
DEALLOCATE  createDates
GO

