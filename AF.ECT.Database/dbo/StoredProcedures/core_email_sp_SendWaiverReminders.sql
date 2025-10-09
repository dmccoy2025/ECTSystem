
-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	11/27/2015
-- Work Item:		TFS Task 289
-- Description:		Changed the size of CaseId from 15 to 50.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_email_sp_SendWaiverReminders]

AS

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	Declare @emailAddress varchar(1000)
		--, @body1 varchar(max)
		--, @body2 varchar(max)
		--, @body varchar(max)
		, @body1 varchar(4000)
		, @body2 varchar(4000)
		, @bodyMain varchar(8000)		
		, @MessageSubject varchar(200) 
		, @subjectStart varchar(100) = 'P-Waivers (30 days and 45 days Notification Report) For Unit: '
		, @unit int
		, @groupId int
		, @unitName varchar(100)
		, @newLine varchar(20) = Char(13) + Char(10)
		, @dt DATETIME;
		
	SET @dt=GETDATE()

	Declare unitCursor Cursor For 
		Select Distinct cs_id, groupId 
		From core_UserRoles cur Inner Join core_Users cu 
			On cur.userID = cu.userID
		Where groupId In (3) -- (3, 4)    3 = Med Tech, 4 = Med Officer [no longer used]
		Order By groupId, cs_id

	Open unitCursor
	Fetch Next From unitCursor
	Into @unit, @groupId
		
	While @@FETCH_STATUS = 0
	BEGIN
		Set @body1 = ''
		Set @body2 = ''
		Set @bodyMain = ''
		--Exec @body1 = report_sp_GetParticipationWaivers @unit, Null, Null, getDate, 30, @groupId, 1
		--Exec @body2 = report_sp_GetParticipationWaivers @unit, Null, Null, getDate, 45, @groupId, 1
		
		-- Logic to properly add the results the @body1 and @body2 variables
		--Delaring a Temporary Table
		DECLARE @tmpPWaivers TABLE 
		(
			 RefId INT
			,CaseId varchar(50)
			,Name varchar(100)
			,SSN varchar(4)
			,Compo varchar(1)
			,PassCode varchar(8)
			,UnitName varchar(200)
			,ApprovalDate datetime
			,DaysRemaining int
			,DaysUsed int
			,MemberWaivers int
		)
		
		--Insert the First Group
		--Exec @body1 = report_sp_GetParticipationWaivers @unit, Null, Null, @dt, 30, @groupId, 1
		INSERT INTO @tmpPWaivers EXEC report_sp_GetParticipationWaivers @unit, Null, Null, @dt, 60, @groupId, 1		
		
 		DECLARE PWaiverCursor CURSOR FOR
 			--SELECT * FROM @tmpPWaivers 
 			SELECT CaseId FROM @tmpPWaivers 
			
			declare
			 @RefId int
			,@CaseId varchar(50)
			,@Name varchar(100)
			,@SSN varchar(4)
			,@Compo varchar(1)
			,@PassCode varchar(8)
			,@UnitNametmp varchar(200)
			,@ApprovalDate datetime
			,@DaysRemaining int
			,@DaysUsed int
			,@MemberWaivers int	
	
			OPEN PWaiverCursor
			FETCH NEXT FROM PWaiverCursor
			--INTO @RefId, @CaseId, @Name, @SSN, @Compo, @PassCode, @UnitNametmp, @ApprovalDate, @DaysRemaining, @DaysUsed, @MemberWaivers
			INTO @CaseId
		
			WHILE @@FETCH_STATUS = 0
				BEGIN			
				
					--Set @body1 =  @body1 + ' ' + Convert(varchar(10),@RefId) + '|' + @CaseId + '|' + @Name  + '|' + @SSN + '|' + @Compo + '|' + @PassCode + '|' + @UnitNametmp + '|' + Convert(varchar(10),@ApprovalDate,101) 
					--	+ '|' + Convert(varchar(10),@DaysRemaining) + '|' + Convert(varchar(10),@DaysUsed) + '|' + Convert(varchar(10),@MemberWaivers) + @newLine
					Set @body1 =  @body1 + ' ' + @CaseId + @newLine
					
					FETCH NEXT FROM PWaiverCursor
					--INTO @RefId, @CaseId, @Name, @SSN, @Compo, @PassCode, @UnitNametmp, @ApprovalDate, @DaysRemaining, @DaysUsed, @MemberWaivers
					INTO @CaseId
				END 
			CLOSE PWaiverCursor
			DEALLOCATE PWaiverCursor
			
			--Cleaning table for the Second Group
		Delete from @tmpPWaivers
		--Insert the Second Group
		--Exec @body2 = report_sp_GetParticipationWaivers @unit, Null, Null, @dt, 45, @groupId, 1
					
		INSERT INTO @tmpPWaivers EXEC report_sp_GetParticipationWaivers @unit, Null, Null, @dt, 45, @groupId, 1		
		
 		DECLARE PWaiverCursor CURSOR FOR
 			--SELECT * FROM @tmpPWaivers 
 			SELECT CaseId FROM @tmpPWaivers 
 			
			OPEN PWaiverCursor
			FETCH NEXT FROM PWaiverCursor
			--INTO @RefId, @CaseId, @Name, @SSN, @Compo, @PassCode, @UnitNametmp, @ApprovalDate, @DaysRemaining, @DaysUsed, @MemberWaivers
			INTO @CaseId
		
			WHILE @@FETCH_STATUS = 0
				BEGIN			
				
					--Set @body2 =  @body2 + ' ' + Convert(varchar(10),@RefId) + '|' + @CaseId + '|' + @Name  + '|' + @SSN + '|' + @Compo + '|' + @PassCode + '|' + @UnitNametmp + '|' + Convert(varchar(10),@ApprovalDate,101) 
						--+ '|' + Convert(varchar(10),@DaysRemaining) + '|' + Convert(varchar(10),@DaysUsed) + '|' + Convert(varchar(10),@MemberWaivers) + @newLine
					Set @body2 =  @body2 + ' ' + @CaseId + @newLine
					
					FETCH NEXT FROM PWaiverCursor
					--INTO @RefId, @CaseId, @Name, @SSN, @Compo, @PassCode, @UnitNametmp, @ApprovalDate, @DaysRemaining, @DaysUsed, @MemberWaivers
					INTO @CaseId
				END 
			CLOSE PWaiverCursor
			DEALLOCATE PWaiverCursor
			
			--Cleaning table for the Third? Group
		Delete from @tmpPWaivers


		-- Setting The Email Addresses - need to look for all RMU on the unit with Access Status Active
		SET @emailAddress = ''
		DECLARE @Emails varchar(60)
 		DECLARE EmailCursor CURSOR FOR
			Select Distinct Email 
			From core_Users 
			Where currentRole In 
			(
				Select userRoleId From core_UserRoles Where groupId = @groupId
			)
			And cs_id = @unit				
			And accessStatus = 3
			
			OPEN EmailCursor
			FETCH NEXT FROM EmailCursor			
			INTO @Emails
			
			WHILE @@FETCH_STATUS = 0
				BEGIN			
				
					IF LEN(@emailAddress) = 0 
						BEGIN
							SET @emailAddress = @Emails;
						END
					ELSE
						BEGIN
							SET @emailAddress = @emailAddress + ';' + @Emails;
						END

					FETCH NEXT FROM EmailCursor
					INTO @Emails				
				END
			CLOSE EmailCursor
			DEALLOCATE EmailCursor				
			
			
		--Select Top 1 @emailAddress = Email 
		--From core_Users 
		--Where currentRole In 
		--(
		--	Select userRoleId From core_UserRoles Where groupId = @groupId
		--)
		--	And cs_id = @unit

		Select @unitName = LONG_Name From Command_Struct Where CS_ID = @unit

		Set @MessageSubject = @subjectStart + @unitName

		Set @bodyMain = 'P-Waivers that will expire within 30 days: ' + @newLine + @newLine
		Set @bodyMain = @bodyMain + @body1 + @newLine + @newLine + @newLine
		Set @bodyMain = @bodyMain + 'P-Waivers that will expire within 45 days: ' + @newLine + @newLine
		Set @bodyMain = @bodyMain + @body2

		If(LEN(@body1) > 0 Or LEN(@body2) > 0)  -- might need to increase length check to account for header row
		--PRINT '@body1' + @body1
		Begin
			--Exec xp_sendmail @recipients = @emailAddress, @subject = @MessageSubject, @message = @body
			EXEC msdb.dbo.sp_send_dbmail
			--@recipients = 'juan.leon-cruz@asmr.com',
			--@body = @bodyMain,
			--@subject = 'Automated Success Message' ;
			--@recipients = @emailAddress,
			@profile_name = 'ALOD_PWaiver',
			@blind_copy_recipients = @emailAddress,
			@body = @bodyMain,
			@subject =  @MessageSubject;			


		End
	
		Fetch Next From unitCursor
		Into @unit, @groupId
	END

Close unitCursor
DeAllocate unitCursor

END
GO

