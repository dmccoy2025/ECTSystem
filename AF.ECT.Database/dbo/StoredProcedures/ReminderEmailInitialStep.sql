
-- ============================================================================
-- Author:		Kamal Singh
-- Create date: 8/21/2014
-- Description:	When a case changes status, thise procedure will remove cases 
--				from ReminderEmails table, then add them with 
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	7/28/2016
-- Description:		Added Appeal workflow
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	12/2/2016
-- Description:		Added the SARC workflow section
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	1/16/2017
-- Description:		Added the Appeal SARC workflow section
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	6/9/2017
-- Description:		- Modified to use the RR/Appeal/SARC Appeal cases member
--					unit info instead of the LODs/SARCs member unit info.
-- ============================================================================
CREATE PROCEDURE [dbo].[ReminderEmailInitialStep]
	@pId bigint,
	@pworkStatusId bigint,
	@pcaseType varchar(10)
AS
BEGIN
	DECLARE @caseId VARCHAR(128)
	DECLARE @memberUnitId int
	
	IF(@pcaseType = 'LOD')
	BEGIN
		SELECT	@caseId = case_id, @memberUnitID = member_unit_id 
		FROM	[dbo].[Form348]
		WHERE	lodId = @pId
	END
	
	IF(@pcaseType = 'RR')
	BEGIN
	
		SELECT	@caseId = RR.case_id, @memberUnitID = RR.member_unit_id
		FROM	[dbo].[Form348_RR] AS RR 
		WHERE	request_id = @pId
	END

	IF (@pcaseType = 'SARC')
	BEGIN
		SELECT	@caseId = sr.case_id,
				@memberUnitID = sr.member_unit_id
		FROM	Form348_SARC sr
		WHERE	sr.sarc_id = @pId
	END
	
	IF(@pcaseType = 'AP')
	BEGIN
		SELECT	@caseId = AP.case_id, @memberUnitID = AP.member_unit_id
		FROM	[dbo].[Form348_AP] AS AP
		WHERE	appeal_id = @pId
	END
	
	IF(@pcaseType = 'SC')
	BEGIN
		SELECT	@caseId = case_id, @memberUnitID = Member_Unit_Id 
		FROM	[dbo].[Form348_SC]
		WHERE	SC_Id = @pId
	END

	IF (@pcaseType = 'APSA')
	BEGIN
			SELECT	@caseId = AP.case_id, @memberUnitID = AP.member_unit_id
			FROM	[dbo].[Form348_AP_SARC] AS AP
			WHERE	appeal_sarc_id = @pId
	END

	INSERT	INTO	[dbo].[ReminderEmails] (settingId, caseId, lastSentDate, lastModifiedDate, sentCount, member_unit_id)
			SELECT	setting.Id, @caseId, getDate(), getDate(), 0,@memberUnitID
			FROM	[dbo].[ReminderEmailSettings] AS setting
			WHERE	setting.wsId = @pworkStatusId
END
GO

