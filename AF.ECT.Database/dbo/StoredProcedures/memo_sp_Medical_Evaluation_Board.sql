
-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	2/17/2015
-- Work Item:		TFS Bug 267
-- Description:		Added the variables and statement to construct the HQ
--					AFRC Address from the values in the core_KeyVal_* tables. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	11/9/2015
-- Work Item:		TFS Bug 388
-- Description:		Added the variables and statement to grab the ALC DAV value
--					from the KeyVal tables based on the ALC value of the case.
-- ============================================================================
CREATE PROCEDURE [dbo].[memo_sp_Medical_Evaluation_Board]
	@refId int
AS


DECLARE @results TABLE
(
	id varchar(100),
	value varchar(MAX)
);

DECLARE @name varchar(100), 
@unit varchar(100),  
@rank varchar(6), 
@address varchar(200), 
@member_ssn varchar(12), 
@diagnosis varchar(50), 
@paraInfo varchar(20) = '2.11',
@gender1 varchar(6),
@gender2 varchar(6), 
@gender1a varchar(1), 
@expirationDate datetime,			-- Data Location: Expiration/Renewal data field in MB HQ AFRC Tech tab. 
@returnToDuty datetime,

-- NEW VARIABLES --
@rmu varchar(100),				-- Data Location: RMU name dropdown in MB HQ AFRC Tech tab. 
@medGroup varchar(100),			-- Data Location: Med Group Name dropdown in MB HQ AFRC Tech tab.  
@alc varchar(6), 			    -- Data Location: ALC dropdown field in WD Board Med tab. 
@alcParagraph varchar(Max), 	-- Data Location: unknown
@keyDesc VARCHAR(20) = 'HQ AFRC Address',
@keyId INT = 0,
@hqAddrLine1 varchar(100),
@hqAddrLine2 varchar(100),
@hqAddrLine3 varchar(100),
@davValue varchar(5),
@alcDAVLine varchar(250) = ''

SELECT 
	@name = MD.LAST_NAME + ', ' + MD.FIRST_NAME + ' ' + ISNULL(MD.MIDDLE_NAMES,'') 
	, @unit = Member_Unit
	, @rank = Grade.RANK
	, @member_ssn = LEFT (MD.SSN , 3) + '-' + SUBSTRING(MD.SSN, 4, 2) + '-' +  RIGHT(MD.SSN,4)
--	, @appAuthName = rr.sig_name_approval
	, @gender1a = md2.SEX_SVC_MBR
	, @diagnosis = icd9_description
	, @expirationDate = Expiration_Date
	, @returnToDuty = Return_To_Duty_Date
	, @rmu = RMU.RMU
	, @medGroup = MedGroup.MTF
	, @alc = ALC_Letter_Type
FROM 
	Form348_SC SpecialCase
		LEFT JOIN core_lkupGrade Grade ON Grade.CODE = SpecialCase.member_grade
		LEFT JOIN VW_MEMBERDATA MD ON MD.ssn= SpecialCase.member_ssn
		Left Join MemberData md2 on md2.SSAN = md.SSN
		Left Join core_lkupRMUs RMU on RMU.Id = SpecialCase.RMU_Name
		Left Join core_lkupMedGroups MedGroup ON MedGroup.Id = SpecialCase.Med_Group_Name
WHERE SpecialCase.SC_Id = @refId

IF @alc > 0 
BEGIN
	-- Select ALC paragraph value...
	SELECT @alcParagraph = [Value] FROM core_KeyVal_Value
	WHERE [Key_Id] = 1 AND [Value_Id] = @alc


	-- Select DAV value...
	SELECT	@keyId = Id
	FROM	core_KeyVal_Key
	WHERE	[Description] = 'ALC DAV Values'
	
	IF @keyId > 0
		BEGIN
		
		SELECT	@davValue = v.Value
		FROM	core_KeyVal_Value v
		WHERE	v.Key_Id = @keyId
				AND v.Value_Id = @alc
		
		END
		
		
	-- Select ALC_DAV_Line value...
	SET @keyId = 0
	
	SELECT	@keyId = Id
	FROM	core_KeyVal_Key
	WHERE	[Description] = 'MPF ALC/DAV Line'
	
	IF @keyId > 0
	BEGIN
		SELECT	@alcDAVLine = v.Value
		FROM	core_KeyVal_Value v
		WHERE	v.Key_Id = @keyId
				AND v.Value_Id = 1
	END
END

SET @keyId = 0

SELECT	@keyId = Id
FROM	core_KeyVal_Key
WHERE	[Description] = @keyDesc

IF @keyId > 0
BEGIN

	-- Select unit name value
	SELECT	@hqAddrLine1 = [Value]
	FROM	core_KeyVal_Value
	WHERE	[Key_Id] = @keyId AND
			[Value_Id] = 1
			
	-- Select street address value
	SELECT	@hqAddrLine2 = [Value]
	FROM	core_KeyVal_Value
	WHERE	[Key_Id] = @keyId AND
			[Value_Id] = 2

	-- Select city/base value
	SELECT	@hqAddrLine3 = [Value]
	FROM	core_KeyVal_Value
	WHERE	[Key_Id] = @keyId AND
			[Value_Id] = 3
			
	-- Select the state value
	SELECT	@hqAddrLine3 = @hqAddrLine3 + ' ' + [Value]
	FROM	core_KeyVal_Value
	WHERE	[Key_Id] = @keyId AND
			[Value_Id] = 4 AND
			[Value] IS NOT NULL
			
	-- Select the zip code value
	SELECT	@hqAddrLine3 = @hqAddrLine3 + ' ' + [Value]
	FROM	core_KeyVal_Value
	WHERE	[Key_Id] = @keyId AND
			[Value_Id] = 5 AND
			[Value] IS NOT NULL
END

-- default
Set @gender1 = 'Member'
Set @gender2 = 'their'

If @gender1a = 'M'
Begin
	Set @gender1 = 'He'
	Set @gender2 = 'his'
End
If @gender1a = 'F'
Begin
	Set @gender1 = 'She'
	Set @gender2 = 'her'
End
--SET @address = (SELECT LOCAL_ADDR_STREET + ' ' + LOCAL_ADDR_CITY + ', ' + LOCAL_ADDR_STATE + ' ' + ADRS_MAIL_ZIP
--					FROM MemberData WHERE SSAN = (SELECT member_ssn FROM Form348_SC WHERE SC_Id  = @refId)
--				)

--INSERT INTO @results (id, value) VALUES ('APPROVING_AUTHORITY', @appAuthName);
INSERT INTO @results (id, value) VALUES ('MEMBER_NAME', @name);
INSERT INTO @results (id, value) VALUES ('MEMBER_RANK', @rank);
INSERT INTO @results (id, value) VALUES ('MEMBER_UNIT_NAME', @unit);
--INSERT INTO @results (id, value) VALUES ('MEMBER_ADDRESS', @address);
INSERT INTO @results (id, value) VALUES ('MEMBER_SSN', @member_ssn);
INSERT INTO @results (id, value) VALUES ('DIAGNOSIS', @diagnosis);
INSERT INTO @results (id, value) VALUES ('Gender1', @gender1);
INSERT INTO @results (id, value) VALUES ('Gender2', @gender2);
INSERT INTO @results (id, value) VALUES ('Expiration_Date', REPLACE(STUFF(CONVERT(VARCHAR(17), @expirationDate, 106), 4, 3, DATENAME(month, @expirationDate)), ' ', '-'));
INSERT INTO @results (id, value) VALUES ('Return_To_Duty_Date', REPLACE(STUFF(CONVERT(VARCHAR(17), @returnToDuty, 106), 4, 3, UPPER(DATENAME(month, @returnToDuty))), ' ', '-'));
INSERT INTO @results (id, value) VALUES ('PARAGRAPH_INFO', @paraInfo);

-- NEW PARAMETERS --
INSERT INTO @results (id, value) VALUES ('ALC_DAV_LINE', @alcDAVLine);
INSERT INTO @results (id, value) VALUES ('RMU', @rmu);
INSERT INTO @results (id, value) VALUES ('MED_GROUP', @medGroup);
INSERT INTO @results (id, value) VALUES ('ALC', 'C' + @alc);
INSERT INTO @results (id, value) VALUES ('ALC_PARAGRAPH', @alcParagraph);
INSERT INTO @results (id, value) VALUES ('HQ_ADDR_LINE_1', @hqAddrLine1);
INSERT INTO @results (id, value) VALUES ('HQ_ADDR_LINE_2', @hqAddrLine2);
INSERT INTO @results (id, value) VALUES ('HQ_ADDR_LINE_3', @hqAddrLine3);
INSERT INTO @results (id, value) VALUES ('DAV', @davValue);


SELECT id, value FROM @results

--SELECT datename(dw,c.io_completionDate) + ' ' + datename(dd,c.io_completionDate) + ' ' + datename(mm,c.io_completionDate) + ' ' + datename(yyyy,c.io_completionDate)
GO

