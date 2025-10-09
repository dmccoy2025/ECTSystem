CREATE  PROCEDURE [dbo].[imp_updateCoreStatusTracking]

	 @lod_id 	int
AS
             
DECLARE @lodHistory TABLE
(
	lod_id int,
	pvs_id int, 
	ws_id int ,
	calling_pp_id int,
	start_date datetime,
	end_date datetime ,
	process_name varchar(400),
	userId int 
)
			
DECLARE  
	 @ws_id int
	,@prev_ws_id int
	,@new_lod_id int
	,@pvs_id int
	,@final_startDate datetime
	,@final_endDate datetime 
	,@userId int 
	,@procName varchAr(500) 
	,@remark varchAr(500) 
	,@startDate datetime
	,@final_userId int 
	,@endDate datetime   
	,@old_lod_id INT  
	,@currentStatus int  
        
         
SET @new_lod_id=(select alod_lod_id from imp_LODMAPPING where rcpha_lodid=@lod_id)
  
IF( @new_lod_id is not null)		
BEGIN 
      
	BEGIN TRY --BEGIN TRY BLOCK 
		
		insert into @lodHistory (lod_id  ,calling_pp_id,pvs_id  ,process_name ,ws_id   ,start_date   ,end_date,userId  )
		SELECT  
			cast(t1.lod_id as int) AS lod_id 
		 	,CASE --This is to make sure that the current status is latest  
				WHEN   t.CALLING_PP_ID IS  null or t.CALLING_PP_ID=' '   THEN null
				ELSE  cast(t.CALLING_PP_ID as int)
				END 
			AS CALLING_PP_ID  
		 	,cast(t.pvs_id as int) AS pvs_id 
		 	,t.PROCESS_NAME as  process_name
			,lod_status.ALOD_WORKSTATUS as ws_id 
			,cast(t.start_date as datetime)AS  start_date
			,CASE --This is to make sure that the current status is latest  
				WHEN t.end_date is null or t.end_date=' ' and cast(t.pvs_id as int) not in (900,901,902,903,912,913,914,915)  THEN getUTCDate()
				WHEN t.end_date is null or t.end_date=' ' and cast(t.pvs_id as int)   in (900,901,902,903,912,913,914,915)  THEN DATEADD (ms,5, cast(t.start_date as DATETIME))
	 			ELSE CASE 
					WHEN cast(t.pvs_id as int)=974 THEN DATEADD (ms,10, cast(t.end_date as DATETIME))
					ELSE cast(t.end_date as DATETIME)
					END 
				END  AS end_date , 
				--,t3.process_name  AS process_name	,t.remark AS remark 
			(SELECT  USERid from imp_USERMAPPING where CAST(PERSON_ID AS INT)=CAST(t4.pers_id AS INT))--t4.pers_id) --	 lod_users.UserId as userId 
				FROM 
				imp_lod_dispositions t1 
				left join imp_person_processes t  on cast(t.pi_id as int)=cast(t1.pi_id as int)
				inner join imp_process_valid_status t2 on cast(t2.pvs_id as int)=cast(t.pvs_id  as int)
				inner join imp_process   t3 on cast(t3.proc_id as int)=cast(t2.proc_id as int)
				inner join imp_PROCMAPPING lod_status on cast(t.pvs_id as int)=lod_status.PVS_ID
				inner join imp_process_instance t4 on cast(t4.pi_id as int)=cast(t1.pi_id as int)
				WHERE cast(t1.lod_id as int)=@lod_id      
				order By cast(t.Start_Date as datetime) ASC 

		SET @currentStatus=(SELECT STATUS FROM Form348 WHERE  lodId=@new_lod_id)

	    DECLARE history_cursor CURSOR FOR  
			SELECT  @old_lod_id  ,pvs_id   ,ws_id   ,start_date   ,end_date,userId   
			FROM @lodHistory  
			order By [Start_Date] ASC, END_DATE ASC 
		   
		OPEN history_cursor
				
		FETCH NEXT FROM history_cursor		
		INTO @old_lod_id,@pvs_id,@ws_id ,@startDate,@endDate,@userId 
				
		set @prev_ws_id=@ws_id
		set @final_startDate=@startDate
	 			
		WHILE @@FETCH_STATUS = 0
		BEGIN
			Set @final_endDate=@endDate
			Set @final_userId =@userId
					
	 		FETCH NEXT FROM history_cursor  INTO    @old_lod_id,@pvs_id,@ws_id,@startDate,@endDate,@userId 
	 					 
	 		if @prev_ws_id <> @ws_id	 
	 		BEGIN  
		    
				IF (  @final_startDate   >=   @final_endDate   )
				BEGIN
					SET @final_endDate=DATEADD (ms,10 ,@final_startDate)
				END  
									
    			IF  (  @final_endDate >=@startDate )
				BEGIN
					SET @startDate=DATEADD (ms,10,@final_endDate)
				END 	
									
    			INSERT INTO  core_WorkStatus_Tracking 
					(ws_id,refId,module,startDate,endDate,completedBy)
				Values
					(@prev_ws_id,@new_lod_id,2,@final_startDate,@final_endDate,@final_userId) 
							 	
				--Update time for next record
				SET @final_startDate=@startDate		
						 	    					
    					 
    		END  
    				
    		SET	@prev_ws_id=@ws_id		 
	   	END 
    			
		CLOSE history_cursor
		DEALLOCATE history_cursor					

		--This being the last status should be set as null 
		SET @endDate=NULL 
		SET @userId=NULL			
			
 		INSERT INTO  core_WorkStatus_Tracking 
			(ws_id,refId,module,startDate,endDate,completedBy)
		Values
			(@ws_id,@new_lod_id,2,@final_startDate,@endDate,@userId) 			 
		
	END TRY 
		 	
	BEGIN CATCH 

		DECLARE @msg varchar(2000)
		DECLARE @number int ,@errmsg varchar(2000)
		SELECT 
		  @number= ERROR_NUMBER()  
		 ,@errmsg= ERROR_MESSAGE()	 
		EXECUTE imp_InsertErrorRecord @number 
		,'core_status_tracking','imp_updateCoreStatusTracking ','error updating tracking info',@lod_id,@errmsg
			 				    
	END CATCH---END CATCH BLOCK      
			 
			 
END --new LOD_ID  not null 			 
ELSE
BEGIN
	EXECUTE imp_InsertErrorRecord 0 
		,'core_status_tracking','imp_updateCoreStatusTracking ','error updating tracking info',@lod_id,'MAPPINGERROR'
END
GO

