--EXECUTE imp_update_LODStatus
--select * from imp_lodmapping
--This takes around 12 minutes for 9000 lods 

CREATE PROCEDURE [dbo].[imp_update_LODStatus]
  @lod_id    INT 
AS

DECLARE    @PVS_ID    INT;
DECLARE    @PROCESS_NAME  VARCHAR(50);
DECLARE    @PVS_meaning     VARCHAR(50);

--DECLARE @lod_id int
--set @lod_id = 41928;
 
 
--Update Lod Status -----
  		 
--Clean data  
BEGIN TRY 
	SELECT 
		@PVS_ID =PO.pvs_id
		,@PROCESS_NAME=PO.process_name
		,@PVS_meaning=PO.pvs_meaning
	FROM 
		(SELECT top 1 
		t1.lod_id AS lod_id
		,CASE --This is to make sure that the current status is latest  
			WHEN t.CALLING_PP_ID IS null or t.CALLING_PP_ID=' ' THEN null
			ELSE  cast(t.CALLING_PP_ID as int)
			END AS CALLING_PP_ID					
		,cast(t.pvs_id as int) AS pvs_id
		,t3.process_name AS process_name
		,t2.STATUS_MEANING as pvs_meaning 
		,t.remark AS remark 
		,cast(t.start_date as datetime)AS  start_date
		,CASE
			WHEN t.end_date is null or t.end_date=' ' and cast(t.pvs_id as int) not in (900,901,902,903,912,913,914,915)  THEN getUTCDate()
			WHEN t.end_date is null or t.end_date=' ' and cast(t.pvs_id as int)   in (900,901,902,903,912,913,914,915)  THEN DATEADD (ms,5, cast(t.start_date as DATETIME))
    		ELSE 
    			CASE 
					WHEN cast(t.pvs_id as int)=974 THEN DATEADD (ms,200, cast(t.end_date as DATETIME))
					ELSE cast(t.end_date as DATETIME)
				END 
			END AS end_date   
		FROM 
	 imp_lod_dispositions t1 
	 left join imp_person_processes t on cast(t.pi_id as int)=cast(t1.pi_id as int)
	 inner join imp_process_valid_status t2 on cast(t2.pvs_id as int)=cast(t.pvs_id  as int)
	 inner join imp_process   t3 on cast(t3.proc_id as int)=cast(t2.proc_id as int)
	 WHERE cast(t1.lod_id as int)=@lod_id order By cast(Start_Date as datetime) DESC,  end_date desc
   )PO    


UPDATE 
	imp_LODMAPPING
SET  
	 rcpha_valid_status=@PVS_ID
	,rcpha_proc_name=@PROCESS_NAME 
	,rcpha_valid_status_meaning=@PVS_meaning 
	,alod_status=(SELECT ALOD_WORKSTATUS FROM imp_PROCMAPPING WHERE PVS_ID=@PVS_ID)
WHERE 
	rcpha_lodid=@lod_id

END TRY 	
BEGIN CATCH 
	DECLARE @number int ,@errmsg varchar(2000)
	SELECT 
		@number= ERROR_NUMBER()  
		,@errmsg= ERROR_MESSAGE()	
 
	EXECUTE imp_InsertErrorRecord @number 
	,'LOD STATUS UPDATE','imp_update_LODStatus ','error updating status',@lod_id,@errmsg
				
END CATCH
GO

