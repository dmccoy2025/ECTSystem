
-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	Copy RR memos from core_Memos2 and core_MemoContents2 to
--				core_Memos and core_MemoContents respectively.
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	3/1/2017
-- Description:		- Modified to only select the most recent RR determination
--					memo to copy over to the original LOD case.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_memo_sp_copy_memos2_to_memos]
(
	@requestId int,
	@userId int,
	@newModuleId int
)

AS 

Declare @oldId int, @newId int = 0

-- Copy this section for other modules
If @newModuleId = 5
BEGIN
	Select @oldId = InitialLodId, @newId = ReinvestigationLodId 
	From Form348_RR 
	Where request_id = @requestId
END


Declare @MemoId_MapTable TABLE
(
	OldMemoId int
	, templateId int
	, created_by int
	, created_date datetime
	, letterHead nvarchar(100)
	, deleted bit
	, NewMemoId int
)
	 
-- Copy Denied/Approved Memo from Request to Old LOD	
Insert Into core_Memos (refId, templateId, created_by, created_date, letterHead, deleted)
Output inserted.memoId, inserted.templateId, inserted.created_by, inserted.created_date, inserted.letterHead, inserted.deleted 
INTO @MemoId_MapTable (NewMemoId, templateId, created_by, created_date, letterHead, deleted)
Select Top 1 @oldId, templateId, @userId, GETDATE(), letterHead, deleted from core_Memos2
Where refId = @requestId
	And templateId In (12, 14)  -- Reinvestigation Approved/Denied Memos only
	And moduleId = @newModuleId  -- Reinvestigation Memos only
Order by memoId DESC

Update @MemoId_MapTable
Set OldMemoId = c1.memoId
From @MemoId_MapTable m1 Inner Join core_Memos2 c1
	On c1.deleted = m1.deleted
	And c1.letterHead = m1.letterHead
	And c1.templateId = m1.templateId
Where c1.refId = @requestId
	And moduleId = @newModuleId  -- Reinvestigation Memos only


Insert Into core_MemoContents (memoId, attachments, body, created_by, created_date, memoDate, sigBlock, suspenseDate)
Select m1.NewMemoId, attachments, body, c2.created_by, c2.created_date, memoDate, sigBlock, suspenseDate 
From core_MemoContents2 c2 Inner Join @MemoId_MapTable m1 on c2.memoId = m1.OldMemoId

If @newId > 0  -- LOD Reinvestigation Request was Approved
BEGIN
	-- Refresh the Temp Table
	Delete From @MemoId_MapTable

	-- Copy ALL Memos from Request to New LOD	
	Insert Into core_Memos (refId, templateId, created_by, created_date, letterHead, deleted)
	Output inserted.memoId, inserted.templateId, inserted.created_by, inserted.created_date, inserted.letterHead, inserted.deleted 
	INTO @MemoId_MapTable (NewMemoId, templateId, created_by, created_date, letterHead, deleted)
	Select @newId, templateId, @userId, GETDATE(), letterHead, deleted from core_Memos2
	Where refId = @requestId
			And moduleId = @newModuleId  -- Reinvestigation Memos only

	Update @MemoId_MapTable
	Set OldMemoId = c1.memoId
	From @MemoId_MapTable m1 Inner Join core_Memos2 c1
		On c1.deleted = m1.deleted
		And c1.letterHead = m1.letterHead
		And c1.templateId = m1.templateId
	Where c1.refId = @requestId
		And moduleId = @newModuleId  -- Reinvestigation Memos only


	Insert Into core_MemoContents (memoId, attachments, body, created_by, created_date, memoDate, sigBlock, suspenseDate)
	Select m1.NewMemoId, attachments, body, c2.created_by, c2.created_date, memoDate, sigBlock, suspenseDate 
	From core_MemoContents2 c2 Inner Join @MemoId_MapTable m1 on c2.memoId = m1.OldMemoId

END

Delete @MemoId_MapTable
GO

