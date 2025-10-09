-- Exec OverrideCase_SetToNewStatus '20120525-008-IN', 41

-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	11/27/2015
-- Work Item:		TFS Task 289
-- Description:		Changed the size of @CaseNumber from 20 to 50.
-- ============================================================================
CREATE Procedure [dbo].[OverrideCase_SetToNewStatus]
(
	@CaseNumber varchar(50)
	, @newStatus int
	, @userId int = 1 -- Jenet's Sys Admin User Id
)

As

Declare @SpecCaseId int,
@ModuleId int,
@WorkflowId int,
@CurrWorkStatusId int,
@CurrStatusId int,
@CurrStatusDesc varchar(50),
@MaxSortOrder int,
@isCancelStatus int,
@isNextCancel int

select @SpecCaseId = SC_Id
	, @ModuleId = Module_Id
	, @CaseNumber = case_id 
	, @WorkflowId = workflow
	, @CurrWorkStatusId = spec.status
	, @CurrStatusId = cws.statusId
	, @CurrStatusDesc = description 
	, @MaxSortOrder = sortOrder 
	, @isCancelStatus = isCancel
from Form348_SC spec
inner join core_WorkStatus cws On cws.workflowId = spec.workflow
  And cws.ws_id = spec.status
inner join core_StatusCodes csc On cws.statusId = csc.statusId
where case_id = @CaseNumber

If Exists(Select 1 From core_lkupAccessScope Where @newStatus In (Select cws.ws_id From core_WorkStatus cws where cws.workflowId = @WorkflowId))
Begin
	Select @isNextCancel = isCancel 
	From core_WorkStatus cws Inner Join core_StatusCodes csc On cws.statusId = csc.statusId
	Where cws.ws_id = @newStatus

	If @isNextCancel = 1	
	Begin
		-- Second Query to do the actual override (to Cancel Status)
		Update Form348_SC
		Set status = @newStatus, Case_Cancel_Reason = 8, Case_Cancel_Explanation = 'Case Cancelled by System Admin', Case_Cancel_Date = GETDATE()
		, modified_by = @userId
		Where case_id = @CaseNumber

	End
	Else
	Begin
		If @isCancelStatus = 1
			Begin
				-- Second Query to do the actual override (from Cancel Status)
				Update Form348_SC
				Set status = @newStatus, Case_Cancel_Explanation = Null, Case_Cancel_Reason = Null, Case_Cancel_Date = Null
				, modified_by = @userId
				Where case_id = @CaseNumber
			End
		Else
			Begin
				--This is to take in account all of the cases that do not go to or come from a cancel status.
				Update Form348_SC
				Set status = @newStatus, modified_by = @userId
				Where case_id = @CaseNumber
			End
	End
End
Else
Begin
	Select 'Case: ' + @CaseNumber + ' can no be updated to status: ' + Convert(varchar, @newStatus)
End
GO

