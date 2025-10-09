CREATE PROCEDURE [dbo].[imp_ImportSigDates ]
( @lod_id int) 
 
AS

BEGIN 
	--DECLARE-----------------------
	 	DECLARE   
			
		     @new_lod_id int
			,@pvs_id int
			,@userId int 			 
			,@startDate datetime
			,@endDate datetime 
			--FORM261  
			,@sig_date_io datetime
			,@sig_date_aa datetime
			--FORM348
			,@sig_date_unit_commander	 datetime 	
			,@sig_date_med_officer datetime
			,@sig_date_legal	   datetime
			,@sig_date_appointing datetime
			,@sig_date_board_legal datetime
			,@sig_date_board_medical datetime
			,@sig_date_board_admin datetime
			,@sig_date_approval datetime
			,@sig_date_formal_approval datetime
  
 -------------------------------------------
  set @new_lod_id=(select alod_lod_id from imp_LODMAPPING  where rcpha_lodid=@lod_id)
DECLARE @TransactionName varchar(20) 
SET @TransactionName= 'lodSignDataInsert' 
 
 IF(  @new_lod_id is not null)
 BEGIN 
		
		BEGIN TRY --BEGIN TRY BLOCK 
	    BEGIN TRANSACTION @TransactionName  --BEGIN TRANSACTION BLOCK 
 
	      DECLARE @lodHistory TABLE
			(
			  lod_id int,
			  calling_pp_id int,
			  pvs_id int, 			 
			  start_date datetime,
			  end_date datetime ,
			  userId int 
		   )
			
	
		 insert into @lodHistory (lod_id  ,calling_pp_id,pvs_id      ,start_date   ,end_date,userId  )
		 		SELECT  cast(t1.lod_id as int) AS lod_id
		 		,CASE
				 --This is to make sure that the current status is latest  
						WHEN   t.CALLING_PP_ID IS  null or t.CALLING_PP_ID=' '   THEN null
						   	ELSE  cast(t.CALLING_PP_ID as int)
						 END 
				  AS CALLING_PP_ID  
		 		,cast(t.pvs_id as int) AS pvs_id 
				,cast(t.start_date as datetime)AS  start_date
				,CASE
				 --This is to make sure that the current status is latest  
						WHEN   t.end_date is null or t.end_date=' '  then null
						 ELSE cast(t.end_date as DATETIME)
						 END 
					     AS end_date
				, (SELECT  userid from imp_USERMAPPING where CAST(PERSON_ID AS INT)=CAST(t4.pers_id AS INT))--t4.pers_id) --	 lod_users.UserId as userId 
	             FROM 
			 	 imp_lod_dispositions t1 
                 left join imp_person_processes t  on cast(t.pi_id as int)=cast(t1.pi_id as int)
                 inner join imp_process_valid_status t2 on cast(t2.pvs_id as int)=cast(t.pvs_id  as int)
                 inner join imp_process   t3 on cast(t3.proc_id as int)=cast(t2.proc_id as int)
                 inner join imp_process_instance t4 on cast(t4.pi_id as int)=cast(t1.pi_id as int)
			     WHERE cast(t1.lod_id as int)=@lod_id                
                 order By cast(t.Start_Date as datetime) ASC 
                            
			SET @sig_date_io=(SELECT TOP 1 end_date FROM   @lodHistory WHERE pvs_id=938 ORDER BY end_date DESC) ---FORMAL INVESTIGATION COMPLETE 
			SET @sig_date_aa=(SELECT TOP 1 end_date FROM   @lodHistory WHERE pvs_id=946 ORDER BY end_date DESC) ---FORMAL APP AUTH ACTION COMPLETE  
			SET @sig_date_med_officer =(SELECT TOP 1 end_date FROM   @lodHistory WHERE pvs_id=  918 ORDER BY end_date DESC) ---MEDICAL OFFICER  COMPLETE 
			SET @sig_date_unit_commander	 =(SELECT TOP 1 end_date FROM   @lodHistory WHERE pvs_id=  922 ORDER BY end_date DESC ) ---UNIT COMMANDER COMPLETE 	
			SET @sig_date_legal	   =(SELECT TOP 1 end_date FROM   @lodHistory WHERE pvs_id=  926 ORDER BY end_date DESC ) ---WING JA COMPLETE 
			SET @sig_date_appointing =(SELECT TOP 1 end_date FROM   @lodHistory WHERE pvs_id=  930 ORDER BY end_date DESC ) ---APPOINIOTNG AUTHORITY  COMPLETE 
			SET @sig_date_board_legal =(SELECT TOP 1 end_date FROM   @lodHistory WHERE pvs_id=  954 ORDER BY end_date DESC ) ---BOARD LEGAL COMPLETE 
			SET @sig_date_board_medical =(SELECT TOP 1 end_date FROM   @lodHistory WHERE pvs_id=  958 ORDER BY end_date DESC ) ---FORMAL INVESTIGATION COMPLETE 
			SET @sig_date_board_admin =(SELECT TOP 1 end_date FROM   @lodHistory WHERE pvs_id=  950 ORDER BY end_date DESC) ---BOARD   COMPLETE 
			SET @sig_date_approval =(SELECT TOP 1 end_date FROM   @lodHistory WHERE pvs_id=  962 ORDER BY end_date DESC) ---APPROVAL AUTHORITY  COMPLETE 
			SET @sig_date_formal_approval =(SELECT TOP 1 end_date FROM   @lodHistory WHERE pvs_id=  990 ORDER BY end_date DESC) ---FORMAL APPROVAL  COMPLETE 
				     
				     
				UPDATE Form348
				SET 
				sig_date_unit_commander=@sig_date_unit_commander	 	
			 	,sig_date_med_officer=@sig_date_med_officer
				,sig_date_legal	=@sig_date_legal  
				,sig_date_appointing=@sig_date_appointing
				,sig_date_board_legal=@sig_date_board_legal
				,sig_date_board_medical=@sig_date_board_medical
				,sig_date_board_admin=@sig_date_board_admin
				,sig_date_approval=@sig_date_approval
				,sig_date_formal_approval=@sig_date_formal_approval
  			 	WHERE lodid=@new_lod_id 
  			 	
  			 	                               
				UPDATE Form261
				SET 
				 sig_date_io =@sig_date_io 
			 	,sig_date_appointing=@sig_date_aa
			 	WHERE lodid=@new_lod_id  
			 	
		COMMIT TRAN @TransactionName ---END TRANSACTION  
		
		END TRY 
		
			--END TRY BLOCK  	
			BEGIN CATCH --BEGIN CATCH BLOCK 
		 			ROLLBACK TRAN @TransactionName
							DECLARE  @msg varchar(2000)
						  DECLARE @number int ,@errmsg varchar(2000)
							SELECT 
							  @number= ERROR_NUMBER()  
							 ,@errmsg= ERROR_MESSAGE()
				 			EXECUTE imp_InsertErrorRecord @number 
							,'FORM_SIGNDATES ,MEDICAL UNIT','imp_ImportSigDates ','record not inserted',@lod_id,@errmsg
			 				    
			 END CATCH---END CATCH BLOCK                        
				 
		END --END CASE  IF NEW LOD-ID FOUND 
		ELSE 
		BEGIN 
  						
  							EXECUTE imp_InsertErrorRecord 0 
							,'FORM_SIGNDATES ,MEDICAL UNIT','imp_ImportSigDates ','record not inserted',@lod_id,'MAPPINGERROR'
			 		 
				 		 
 
		END 
END --END STORED PRCO
GO

