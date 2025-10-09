

--========================================================
-- Modified By:		Evan Morrison
-- Date:			1/10/2017
-- Description:		Add appeal sarc module 
--========================================================	
-- Modified By:		Evan Morrison
-- Date:			6/6/2017
-- Description:		When memos are coppied over the date 
--					is not overwritten
--========================================================	


CREATE PROCEDURE [dbo].[core_memo_sp_copy_memos_to_memos2]
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

If @newModuleId = 24
BEGIN
	SELECT @oldId = initial_lod_id
	From Form348_AP
	WHERE appeal_id = @requestId
END

If @newModuleId = 26
BEGIN
	SELECT @oldId = initial_id
	From Form348_AP_SARC
	WHERE appeal_sarc_id = @requestId
END


Declare @MemoId_MapTable TABLE
(
	OldMemoId int
	, moduleId int
	, templateId int
	, created_by int
	, created_date datetime
	, letterHead nvarchar(100)
	, deleted bit
	, NewMemoId int
)
	 
-- Copy Denied/Approved Memo from Request to Old LOD	
Insert Into core_Memos2 (refId, moduleId, templateId, created_by, created_date, letterHead, deleted)
Output inserted.memoId, inserted.moduleId, inserted.templateId, inserted.created_by, inserted.created_date, inserted.letterHead, inserted.deleted 
INTO @MemoId_MapTable (NewMemoId, moduleId, templateId, created_by, created_date, letterHead, deleted)
Select @requestId, @newModuleId, templateId, @userId, created_date, letterHead, deleted from core_Memos
Where refId = @oldId

Update @MemoId_MapTable
Set OldMemoId = c1.memoId
From @MemoId_MapTable m1 Inner Join core_Memos c1
	On c1.deleted = m1.deleted
	And c1.letterHead = m1.letterHead
	And c1.templateId = m1.templateId
Where c1.refId = @oldId


Insert Into core_MemoContents2 (memoId, attachments, body, created_by, created_date, memoDate, sigBlock, suspenseDate)
Select m1.NewMemoId, attachments, body, c2.created_by, c2.created_date, memoDate, sigBlock, suspenseDate 
From core_MemoContents c2 Inner Join @MemoId_MapTable m1 on c2.memoId = m1.OldMemoId


Delete @MemoId_MapTable
GO

