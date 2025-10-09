
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 1/27/2016
-- Description:	This stored procedure acts as the datasource for Recruiting
--				Services workflow case memos. It retrieves all of the 
--				necessary data to populate the paramatized fields in the 
--				certification stamp bodies such as {MEMBER_NAME}. 
-- ============================================================================
CREATE PROCEDURE [dbo].[memo_sp_RecruitingServices]
	@refId int
AS

DECLARE @results TABLE
(
	id VARCHAR(100),
	value VARCHAR(MAX)
)

DECLARE @name VARCHAR(100), 
		@unit VARCHAR(100),  
		@rank VARCHAR(6), 
		@address VARCHAR(200), 
		@member_ssn VARCHAR(12), 
		@gender1 varchar(6),
		@gender2 varchar(6), 
		@gender1a varchar(1), 
		@expirationDate datetime,
		@freeText NVARCHAR(500) = '',
		@keyDesc VARCHAR(20) = 'HQ AFRC Address',
		@keyId INT = 0,
		@alc varchar(6),
		@alcParagraph varchar(MAX),
		@hqAddrLine1 varchar(100),
		@hqAddrLine2 varchar(100),
		@hqAddrLine3 varchar(100),
		@davValue varchar(5),
		@alcDAVLine varchar(250) = '',
		@decision VARCHAR(1)


-- GET PARAMETER DATA --
SELECT	@name = MD.LAST_NAME + ', ' + MD.FIRST_NAME + ' ' + ISNULL(MD.MIDDLE_NAMES, ''),
		@unit = member_unit,
		@rank = Grade.RANK,
		@member_ssn = LEFT (MD.SSN , 3) + '-' + SUBSTRING(MD.SSN, 4, 2) + '-' +  RIGHT(MD.SSN,4),
		@freeText = SpecialCase.free_text,
		@alc = ISNULL(SpecialCase.alt_ALC_letter_type, SpecialCase.ALC_letter_type),
		@expirationDate = Expiration_Date,
		@gender1a = md2.SEX_SVC_MBR,
		@decision = SpecialCase.senior_medical_reviewer_concur
FROM	Form348_SC SpecialCase
		LEFT JOIN core_lkupGrade Grade ON Grade.CODE = SpecialCase.member_grade
		LEFT JOIN VW_MEMBERDATA MD ON MD.ssn = SpecialCase.member_ssn
		LEFT JOIN MemberData md2 ON md2.SSAN = md.SSN
WHERE	SpecialCase.SC_Id = @refId

	--ZACH UPDATE 9/18/2019
	--senior_medical_reviewer_concur
	if (@decision = 'N')
	BEGIN

		SELECT	@name = MD.LAST_NAME + ', ' + MD.FIRST_NAME + ' ' + ISNULL(MD.MIDDLE_NAMES, ''),
				@unit = member_unit,
				@rank = Grade.RANK,
				@member_ssn = LEFT (MD.SSN , 3) + '-' + SUBSTRING(MD.SSN, 4, 2) + '-' +  RIGHT(MD.SSN,4),
				@freeText = SpecialCase.free_text,
				@alc = SpecialCase.Alternate_ALC_Letter_Type, 
				@expirationDate = Alternate_Expiration_Date,
				@gender1a = md2.SEX_SVC_MBR,
				@decision = SpecialCase.senior_medical_reviewer_concur
		FROM	Form348_SC SpecialCase
				LEFT JOIN core_lkupGrade Grade ON Grade.CODE = SpecialCase.member_grade
				LEFT JOIN VW_MEMBERDATA MD ON MD.ssn = SpecialCase.member_ssn
				LEFT JOIN MemberData md2 ON md2.SSAN = md.SSN
		WHERE	SpecialCase.SC_Id = @refId

	END

IF (ISNULL(@freeText, '') = '')
BEGIN
	SET @freeText = '{FREE_TEXT}'
END


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

SET @gender1 = 'Member'
SET @gender2 = 'their'

IF (@gender1a = 'M')
BEGIN
	SET @gender1 = 'He'
	SET @gender2 = 'his'
END

IF (@gender1a = 'F')
BEGIN
	SET @gender1 = 'She'
	SET @gender2 = 'her'
END


-- ASSIGN DATA TO PARAMETERS --
INSERT INTO @results (id, value) VALUES ('MEMBER_NAME', @name)
INSERT INTO @results (id, value) VALUES ('MEMBER_RANK', @rank)
INSERT INTO @results (id, value) VALUES ('MEMBER_UNIT_NAME', @unit)
INSERT INTO @results (id, value) VALUES ('MEMBER_SSN', @member_ssn)
INSERT INTO @results (id, value) VALUES ('FREE_TEXT', @freeText)
INSERT INTO @results (id, value) VALUES ('Gender1', @gender1);
INSERT INTO @results (id, value) VALUES ('Gender2', @gender2);
INSERT INTO @results (id, value) VALUES ('Expiration_Date', REPLACE(STUFF(CONVERT(VARCHAR(17), @expirationDate, 106), 4, 3, UPPER(DATENAME(month, @expirationDate))), ' ', '-'));
INSERT INTO @results (id, value) VALUES ('ALC_DAV_LINE', @alcDAVLine);
INSERT INTO @results (id, value) VALUES ('ALC', 'C' + @alc);
INSERT INTO @results (id, value) VALUES ('ALC_PARAGRAPH', @alcParagraph);
INSERT INTO @results (id, value) VALUES ('HQ_ADDR_LINE_1', @hqAddrLine1);
INSERT INTO @results (id, value) VALUES ('HQ_ADDR_LINE_2', @hqAddrLine2);
INSERT INTO @results (id, value) VALUES ('HQ_ADDR_LINE_3', @hqAddrLine3);
INSERT INTO @results (id, value) VALUES ('DAV', @davValue);

-- SELECT DATA --
SELECT	id, value 
FROM	@results
GO

