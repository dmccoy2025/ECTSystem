
-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	11/27/2015
-- Work Item:		TFS Task 289
-- Description:		Changed the size of case_id & @case_id from 30 to 50.
-- ============================================================================
CREATE PROCEDURE [dbo].[form348_sp_emailOverDue]
		  
		

AS

   	 DECLARE  @USERCASES TABLE
		( 	 
			 case_id varchar(50) 
			,userId int 		 
			,noOfDays int 
		)
  
  DECLARE overdue_cursor CURSOR FOR   SELECT a.lodid as lodid
	    ,a.case_id   as case_id
	    ,a.member_unit_id   as member_unit_id
	    ,a.member_compo 
	    ,f.IoUserId
	    ,a.appAuthUserId 
  		,a.status as WorkStatusId		 
		, datediff(d, ISNULL(t.ReceiveDate, a.created_date), getdate()) AS DAYS 
	FROM Form348  a
	LEFT JOIN form261 f ON f.lodid =a.lodid 
	JOIN core_WorkStatus ws ON ws.ws_id = a.status
	JOIN core_StatusCodes s ON s.statusId = ws.statusId 
	LEFT JOIN (SELECT max(startDate) ReceiveDate, ws_id, refId FROM core_WorkStatus_Tracking GROUP BY ws_id, refId) t ON t.refId = a.lodId AND t.ws_id = a.status
	WHERE  
			 datediff(d, ISNULL(t.ReceiveDate, a.created_date), getdate()) > 10 and s.isFinal<>1 
 
 
	DECLARE @noOfDays int, @wsId int,@lod_id int ,@case_id varchar(50) ,@member_unit_id int ,@ioUserId int ,@aaUserId int,@compo varchar(5)  
	 
 	 
	OPEN overdue_cursor
		
	FETCH NEXT FROM overdue_cursor  INTO @lod_id,@case_id,@member_unit_id,@compo,@ioUserId,@aaUserId,@wsId ,@noOfDays
	
	
	
    WHILE @@FETCH_STATUS = 0
    BEGIN
     INSERT INTO @USERCASES (case_id,userId,noOfDays)
		 SELECT @case_id,userID,@noOfDays
			 FROM vw_users a 
			 JOIN vw_WorkStatus ws ON ws.groupId = a.groupId
			WHERE
				ws.ws_id = @wsId
			AND
				a.accessStatus =  3
			AND
				a.receiveEmail = 1
			AND  
				a.compo = @compo
			--Scope filtering
			AND 
			(
	 		 
	 		@member_unit_id= a.unit_id
			 
			)
			--User ID Filtering
			AND
			(
				a.userId = CASE 
							WHEN ws.filter = 'io' THEN @ioUserId
							WHEN ws.filter = 'aa' THEN @aaUserId
							ELSE a.userId
						END 
			)
			 
			  
	 	FETCH NEXT FROM overdue_cursor  INTO @lod_id,@case_id,@member_unit_id,@compo,@ioUserId,@aaUserId,@wsId ,@noOfDays
	

	END ---End of overdue cursor     
     
     CLOSE overdue_cursor
	 DEALLOCATE  overdue_cursor
	 
	 
	 
	 DECLARE @ProfileName   varchar(200)
	 DECLARE @LODCases   nvarchar(MAX)
	 DECLARE @userId int ,@email varchar(100),@msg  nvarchar(MAX),@sub  varchar(100),@errorSub varchar(100)
	 
	

	 /*DECLARE  @EMAILSENT TABLE
		( 	 
			 EMAILS varchar(200) 
			, MSG    nvarchar(MAX)
		) */
	 SET @sub='Overdue LODs'
	 SET @msg='Following case(s) have been awaiting action for more then 10 days.'
	 SET @msg=@msg + CHAR(13) + CHAR(13) + 'CASE ID' +CHAR(9) + 'Days OverDue' + CHAR(13)	 
	
	  	 SET @ProfileName='DBMailProfile'
 
	
	 DECLARE user_cursor CURSOR FOR   SELECT distinct userId from  @USERCASES
	 OPEN user_cursor
		
	FETCH NEXT FROM user_cursor  INTO @userId  
	
    WHILE @@FETCH_STATUS = 0
    BEGIN
    
   	
	SET @errorSub ='Error in sending email to the userId ' + cast(@userId  as varchar(20)) 

     SET     @LODCases=''
	  SELECT   @LODCases=coalesce(@LODCases +  CHAR(13), '') + case_id  + CHAR(9)+ cast(noOfDays as varchar(20))   FROM  @USERCASES
	  WHERE userId = @userId    Order By noOfDays DESC   
	 	SET  @msg	=@msg +@LODCases
	 	
	 	
	 	SET @msg =@msg +  CHAR(13) + CHAR(13)+ 'Please click http://aflod  to login into the application.'  
	 	SET @email=(SELECT isnull(a.Email,'')+';'+isnull(a.Email2,'')+';'+ isnull(a.Email3,'')   
					FROM vw_users a		 
					WHERE a.userID = @userId )
	 		
	 IF @email is not null 
	 BEGIN
	 --INSERT INTO @EMAILSENT (EMAILS,MSG) VALUES (@email,@msg)
	   Exec msdb.dbo.sp_send_dbmail @recipients=@email, @body =@msg, @subject=@sub,@profile_name =@ProfileName
	   
	   
	   IF @@ERROR != 0
		BEGIN
					
					Exec msdb.dbo.sp_send_dbmail @recipients='rcteam-nonpro@asmr.com',@profile_name =@ProfileName,@body='Error Sending Email', @subject=@errorSub
		END
     END
 		 
	 	FETCH NEXT FROM user_cursor  INTO @userId 
	END ---End of overdue cursor     
     
     CLOSE user_cursor
	 DEALLOCATE  user_cursor
GO

