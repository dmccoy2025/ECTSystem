-- ============================================================================
-- Author:			?
-- Create date:		?
-- Description:
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	8/9/2016
-- Description:		Detect if module is for an appeal case
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	12/2/2016
-- Description:		Added the SARC workflow section
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	6/9/2017
-- Description:		- Modified to use the RR/Appeal/SARC Appeal cases member
--					unit info instead of the LODs/SARCs member unit info.
--					- Added the SARC Appeal workflow section
-- ============================================================================
CREATE PROCEDURE [dbo].[ReminderEmailsAdd] 
	@psettingId bigint,
	@pworkStatusId bigint,
	@pworkflowId bigint
AS
BEGIN
	DECLARE @moduleId int
	DECLARE @modType int = 0

	SELECT @moduleId = moduleId FROM [dbo].[core_Workflow] WHERE workflowId = @pworkflowId
	SELECT @modType = isSpecialCase FROM [dbo].[core_lkupModule] WHERE moduleId = @moduleId

	IF (@modType = 1)
	BEGIN
		INSERT	INTO	dbo.ReminderEmails (settingId, caseId,lastSentDate, lastModifiedDate, sentCount, member_unit_id)
				SELECT	@psettingId, SC.case_Id, SC.modified_date, SC.modified_date, 0, SC.Member_Unit_Id			
				FROM	dbo.Form348_SC AS SC 
				WHERE	SC.status = @pworkStatusId;		
	END
				 
	IF(@moduleId = 2) -- LOD
	BEGIN
		INSERT	INTO	dbo.ReminderEmails (settingId, caseId,lastSentDate, lastModifiedDate, sentCount, member_unit_id)
				SELECT	@psettingId, LOD.case_Id, LOD.modified_date, LOD.modified_date, 0, LOD.member_unit_id		
				FROM	dbo.Form348 AS LOD 
				WHERE	LOD.status = @pworkStatusId;
	END
		
	IF(@moduleId = 5)-- Reinvestigation
	BEGIN
		INSERT	INTO	dbo.ReminderEmails (settingId, caseId,lastSentDate, lastModifiedDate, sentCount, member_unit_id)
				SELECT	@psettingId, RR.case_Id, ISNULL(RR.Modified_Date, RR.CreatedDate), ISNULL(RR.Modified_Date, RR.CreatedDate), 0, RR.member_unit_id		
				FROM	dbo.Form348_RR AS RR 
				WHERE	RR.status = @pworkStatusId;
	END
		
	IF(@moduleId = 24) -- Appeal
	BEGIN
		INSERT	INTO	dbo.ReminderEmails (settingId, caseId,lastSentDate, lastModifiedDate, sentCount, member_unit_id)
				SELECT	@psettingId, ap.case_Id, ISNULL(ap.Modified_Date, ap.created_date), ISNULL(ap.Modified_Date, ap.created_date), 0, ap.member_unit_id		
				FROM	dbo.Form348_AP AS ap 
				WHERE	ap.status = @pworkStatusId;
	END

	IF (@moduleId = 25) -- SARC
	BEGIN
		INSERT	INTO	dbo.ReminderEmails (settingId, caseId,lastSentDate, lastModifiedDate, sentCount, member_unit_id)
				SELECT	@psettingId, sr.case_Id, sr.modified_date, sr.modified_date, 0, sr.member_unit_id		
				FROM	dbo.Form348_SARC AS sr
				WHERE	sr.status = @pworkStatusId;
	END

	IF (@moduleId = 26) -- SARC Appeal
	BEGIN
		INSERT	INTO	dbo.ReminderEmails (settingId, caseId,lastSentDate, lastModifiedDate, sentCount, member_unit_id)
				SELECT	@psettingId, aps.case_Id, aps.modified_date, aps.modified_date, 0, aps.member_unit_id		
				FROM	Form348_AP_SARC aps
				WHERE	aps.status = @pworkStatusId;
	END
END
GO

