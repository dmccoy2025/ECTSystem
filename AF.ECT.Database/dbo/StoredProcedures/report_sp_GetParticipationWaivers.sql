-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

-- Exec [dbo].[report_sp_GetParticipationWaivers] 0, '', '08/01/2012', '09/15/2012', 45, 9, 1
 
CREATE PROCEDURE [dbo].[report_sp_GetParticipationWaivers]
	
	@unitId INT = 0
	, @ssn varchar(12)
	, @beginDate datetime
	, @endDate datetime
	, @interval tinyint
	, @groupId  int
	, @includeSubordinate bit

AS
	SET NOCOUNT ON; 

Select @interval = Case @interval
	When 0 Then 0
	When 45 Then 45
	When 30 Then 30
	When 60 Then 60
	Else 90
End

if (@ssn = '')
	SET @ssn = null;

IF @interval <> 0
	BEGIN
		if(@endDate is null)
			SET @endDate = getDate()
		if(Convert(char(11), @endDate, 101) > GETDATE())
			SET @endDate = getDate()
		If @interval = 30
			Set @beginDate = DateAdd(d, -1 * @interval, @endDate)
		If @interval = 45
			Set @beginDate = DateAdd(d, -1 * @interval, @endDate)
		If @interval = 60
			Set @beginDate = DateAdd(d, -1 * @interval, @endDate)			
		If IsNull(@beginDate, '') = ''
			Set @beginDate = DateAdd(d, -1 * @interval, @endDate)
	END
	


Declare @moduleId int = 10  -- P-Waivers
	
Declare @userUnit int, @scope tinyint ,@canActOnsarcUnrestriced bit, @userId int

If ISNULL(@unitId, 0) = 0
	SELECT Top 1 @userUnit = unit_id, @scope = accessscope  from vw_Users where groupId = @groupId 
Else
	SELECT Top 1 @userUnit = @unitId, @scope = accessscope  from vw_Users where groupId = @groupId And unit_id = @unitId
	
	--PRINT @userUnit
	--PRINT @scope
	--PRINT @groupId
	--PRINT @unitId
	--PRINT @beginDate
	--PRINT @endDate

 SELECT
	
	 a.sc_Id as RefId
	 ,a.case_id  as CaseId  
	,a.member_name  as Name
	,RIGHT( a.member_ssn, 4) as SSN  -- Protected SSN
	, a.member_compo as Compo
	,cs.PAS_CODE as Pascode
	,cs.long_name as UnitName
	,Convert(char(11), a.Approval_Date, 101) ApprovalDate
	--,Case When DATEADD(d, 90, a.Approval_Date) > @endDate Then DateDiff(d, DATEADD(d, 90, a.Approval_Date), @endDate) Else 0 End As DaysRemaining
	--,Case When DateDiff(d, @endDate, a.Approval_Date) < 90 Then DateDiff(d, @endDate, DATEADD(d, 90, a.Approval_Date)) Else 90 End As DaysUsed
	--,Case When DateDiff(d, GETDATE(), a.Expiration_Date) < 0 Then 0 Else DateDiff(d, GETDATE(), a.Expiration_Date) End As DaysRemaining
	
	 ,Case 
		When DateDiff(d, GETDATE(), a.Expiration_Date) < 0 Then 0
		When DateDiff(d, GETDATE(), a.Expiration_Date) > 90 Then 90
		Else DateDiff(d, GETDATE(), a.Expiration_Date)
     End As DaysRemaining	
--	,Case When DateDiff(d, a.Approval_Date, GETDATE()) > 90 Then 90 Else DateDiff(d, a.Approval_Date, GETDATE()) End As DaysUsed
	 ,Case 
		When DateDiff(d, a.Approval_Date, GETDATE()) > 90 Then 90
		When DateDiff(d, a.Approval_Date, GETDATE()) < 0 Then 0
		Else DateDiff(d, a.Approval_Date, GETDATE())
     End As DaysUsed	 
	, MemberWaivers
	
FROM Form348_SC  a
--JOIN core_WorkStatus ws ON ws.ws_id = a.status
--JOIN core_StatusCodes s ON s.statusId = ws.statusId
--JOIN core_workflow w ON w.workflowId = a.workflow
JOIN COMMAND_STRUCT CS ON cs.cs_id = a.member_unit_id 
-- left join core_workflowLocks l on l.refId = a.SC_Id and l.module = a.Module_Id
LEFT JOIN (SELECT max(startDate) ReceiveDate, ws_id, refId FROM core_WorkStatus_Tracking GROUP BY ws_id, refId) t ON t.refId = a.sc_Id AND t.ws_id = a.status
-- update this for Phase II
Left Join (Select Member_ssn, Count(*) As MemberWaivers From Form348_sc Where Module_Id = @moduleId Group By Member_ssn) As b On a.Member_ssn = b.Member_ssn

WHERE 
a.Module_Id = @moduleId
AND
(
	(
	IsNull(@ssn, '') <> ''  -- SSN passed in
	AND
	a.member_ssn like '%' + @ssn + '%'
	)
	OR
	(   -- Other criteria
		(
			@unitId = 0 -- All Units
			OR
			CASE 
			 WHEN @includeSubordinate = 0 THEN a.member_unit_id 
			 ELSE @unitId  -- include subordinates
			 END
			 = @unitId
		 )
 		AND
		(  
			(@scope = 3)
			OR
			(@scope = 2)
			OR
			(	
				@scope = 1
				AND
				a.member_unit_id  IN 				 
					(SELECT child_id FROM Command_Struct_Tree WHERE parent_id=@userUnit 
					) 
			)
			/*OR
			(
				a.created_by = @userId
			)*/
		)
 		--AND  	 
		--( 
		--	a.status in  (SELECT ws_id FROM vw_WorkStatus WHERE moduleId=@moduleId AND groupId = @groupId)
		--   or
		--	a.status in  (SELECT status FROM core_StatusCodeSigners where groupId = @groupId)
		-- )  
		 AND
		 Convert(char(11), a.Approval_Date, 101) Between Convert(char(11), @beginDate, 101) And Convert(char(11), @endDate, 101)
		 And
		 Case 
			When @interval = 45 Then Convert(char(11), DATEADD(D, -45, @endDate) , 101)
			When @interval = 30 Then Convert(char(11), DATEADD(D, -30, @endDate) , 101)
			When @interval = 60 Then Convert(char(11), DATEADD(D, -60, @endDate) , 101)			
			Else Convert(char(11), a.Approval_Date, 101)
		End = Convert(char(11), a.Approval_Date, 101)
	 )
 )
ORDER BY ISNULL(t.ReceiveDate, a.created_date)
GO

