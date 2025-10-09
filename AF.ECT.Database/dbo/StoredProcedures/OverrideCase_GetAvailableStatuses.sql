-- Exec OverrideCase_GetAvailableStatuses '20120524-001-PW'

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
CREATE Procedure [dbo].[OverrideCase_GetAvailableStatuses]
(
	@CaseNumber varchar(50)
)

As

Declare @SpecCaseId int,
@ModuleId int,
@WorkflowId int,
@CurrWorkStatusId int,
@CurrStatusId int,
@CurrStatusDesc varchar(50),
@MaxSortOrder int

select @SpecCaseId = SC_Id
	, @ModuleId = Module_Id
	, @CaseNumber = case_id 
	, @WorkflowId = workflow
	, @CurrWorkStatusId = spec.status
	, @CurrStatusId = cws.statusId
	, @CurrStatusDesc = description 
	, @MaxSortOrder = sortOrder 
from Form348_SC spec
inner join core_WorkStatus cws On cws.workflowId = spec.workflow
  And cws.ws_id = spec.status
inner join core_StatusCodes csc On cws.statusId = csc.statusId
where case_id = @CaseNumber

--Select 
--	@SpecCaseId As SpecCaseId
--	, @CaseNumber As CaseNumber
--	, @ModuleId As ModuleId
--	, @WorkflowId As WorkflowId
--	, @CurrWorkStatusId As WorkStatusId
--	, @CurrStatusId As StatusId
--	, @CurrStatusDesc As StatusDesc
--	, @MaxSortOrder As MaxSortOrder

Select @CaseNumber As CaseNumber, cws.ws_id As UpdateToId, description As NewStatusDesc
from core_WorkStatus cws
Inner Join core_StatusCodes csc On cws.statusId = csc.statusId
where workflowId = @WorkflowId
And cws.statusId <> @CurrStatusId
Order By sortOrder

select COUNT(*) As CaseFound from Form348_SC Where case_id = @CaseNumber

-- Second Query to do the actual override
--Update Form348_SC
--Set status = [UpdateToId]
--, modified_by = 1 -- Jenet's Sys Admin User Id
--Where case_id = @CaseNumber
GO

