-- ============================================================================
-- Author:		Kamal Singh
-- Create date: 8/21/2014
-- Description:	When a case changes status, thise procedure will
--				remove cases from ReminderEmails table, then add them with 
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	8/9/2016
-- Description:		Detect if module is for an appeal case
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	12/2/2016
-- Description:		Added the SARC workflow section
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	1/18/2017
-- Description:		Added the Appeal SARC workflow section
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	6/9/2017
-- Description:		- Modified to use the RR/Appeal/SARC Appeal cases member
--					unit info instead of the LODs/SARCs member unit info.
-- ============================================================================
CREATE PROCEDURE [dbo].[ReminderEmailUpdateStatusChange] 
	@poldWorkStatusId bigint,
	@pnewworkStatusId bigint,
	@pcaseId varchar(128),
	@pcaseType varchar (5)
AS
BEGIN
	DECLARE @memberUnitID int
	
	IF(@poldWorkStatusId <> @pnewWorkStatusId)
	BEGIN
	
		IF(@pcaseType = 'LOD')
		BEGIN
			SELECT	@memberUnitID = member_unit_id 
			FROM	[dbo].[Form348]
			WHERE	case_Id = @pcaseId
		END
	
		IF(@pcaseType = 'RR')
		BEGIN
			SELECT	@memberUnitID = RR.member_unit_id
			FROM	[dbo].[Form348_RR] AS RR
			WHERE	RR.Case_Id = @pcaseId
		END
		
		IF(@pcaseType = 'AP')
		BEGIN
			SELECT	@memberUnitID = AP.member_unit_id
			FROM	[dbo].[Form348_AP] AS AP
			WHERE	AP.Case_Id = @pcaseId
		END
	
		IF(@pcaseType = 'SC')
		BEGIN
			SELECT	@memberUnitID = Member_Unit_Id
			FROM	[dbo].[Form348_SC]
			WHERE	case_Id = @pcaseId
		END

		IF (@pcaseType = 'SARC')
		BEGIN
			SELECT	@memberUnitID = sr.member_unit_id 
			FROM	Form348_SARC sr
			WHERE	sr.case_Id = @pcaseId
		END

		IF(@pcaseType = 'APSA')
		BEGIN
			SELECT	@memberUnitID = ap.member_unit_id
			FROM	[dbo].[Form348_AP_SARC] AS ap
			WHERE	ap.Case_Id = @pcaseId
		END
	
		DELETE	FROM	dbo.ReminderEmails
				WHERE	caseId = @pcaseId
	

		INSERT	INTO	[dbo].[ReminderEmails] (settingId, caseId, lastSentDate, lastModifiedDate, sentCount, member_unit_id)
				SELECT	setting.Id, @pcaseID, getDate(), getDate(), 0, @memberUnitID
				FROM	[dbo].[ReminderEmailSettings] AS setting
				WHERE	setting.wsId = @pnewWorkStatusId
	END
END
GO

