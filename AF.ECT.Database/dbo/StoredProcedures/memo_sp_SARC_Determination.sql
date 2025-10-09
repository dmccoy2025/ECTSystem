
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 11/29/2016
-- Description:	The stored procedure used for getting the parameter data for
--				the SARC determination memos. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	4/25/2017
-- Description:		Fixed a bug that was assigning an incorrect date to the
--					@dateOfCompletion variable.
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 5/4/2017
-- Description:	Put different format of name after NILOD members Acknowledgement
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	5/15/2017
-- Description:		Update memo input information
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	6/6/2017
-- Description:		Update how contact information is put on the memo
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	8/10/2017
-- Description:		Fixed a bug where the City of the appeals address was being
--					appended twice instead of the Street being appended.
-- ============================================================================
CREATE PROCEDURE [dbo].[memo_sp_SARC_Determination]
	@refId int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @results TABLE
	(
		id NVARCHAR(100),
		value NVARCHAR(MAX)
	);

	DECLARE	@rank NVARCHAR(6),
			@name varchar(100),
			@address varchar(200),
			@addressCity VARCHAR(200),
			@unit varchar(100),
			@caseId VARCHAR(50),
			@icdE968 NVARCHAR(100) = '',
			@icdE969 NVARCHAR(100) = '',
			@icdOther NVARCHAR(MAX) = '',
			@hasOtherICDs BIT = 0,
			@helpExtension NVARCHAR(50) = '',
			@diagnosis VARCHAR(MAX),
			@apprAuthName varchar(100),
			@appealAddress NVARCHAR(500),
			@appealAddressStreet NVARCHAR(500),
			@appealAddressCity NVARCHAR(500),
			@appealAddressState NVARCHAR(500),
			@appealAddressZip NVARCHAR(500),
			@appealAddressCountry NVARCHAR(500),
			@dateOfCompletion VARCHAR(50),
			@email VARCHAR(200),
			@contact VARCHAR(MAX),
			@more INT = 0


	SELECT
			@rank = g.Rank,
			@name = md.FIRST_NAME + ' ' + (CASE md.MIDDLE_NAMES WHEN NULL THEN '' WHEN '' THEN '' ELSE LEFT(md.MIDDLE_NAMES, 1) + ' ' END) + md.LAST_NAME,
			@address = md.LOCAL_ADDR_STREET,
			@addressCity = ISNULL(md.LOCAL_ADDR_CITY,'') + ', ' + ISNULL(md.LOCAL_ADDR_STATE, '') + ' ' + ISNULL(md.ADRS_MAIL_ZIP, ''),
			@unit = s.member_unit ,
			@caseId = s.case_id,
			@icdE968 = (CASE WHEN s.ICD_E968_8 = 1 THEN 'Assault by Other Specified Means; ' ELSE '' END ),
			@icdE969 = ( CASE WHEN s.ICD_E969_9 = 1 THEN 'Poisoning by Unspecified Psychotropic Agent; ' ELSE '' END ),
			@hasOtherICDs = s.ICD_other,
			@helpExtension = spp.helpExtensionNumber,
			@apprAuthName = (approver.RANK + ' ' + approver.name),
			@appealAddressStreet = spp.appealStreet ,
			@appealAddressCity = spp.appealCity,
			@appealAddressState = spp.appealState,
			@appealAddressZip = spp.appealZip,
			@appealAddressCountry = spp.appealCountry,
			@email = spp.email,
			@dateOfCompletion = DATENAME(MONTH, comp.CompletedDate) + RIGHT(CONVERT(VARCHAR(12), comp.CompletedDate, 107), 9)
	FROM 
			Form348_SARC s
			JOIN Form348_SARC_PostProcessing spp ON spp.sarc_id = s.sarc_id
			JOIN core_lkupGrade g ON g.CODE = s.member_grade
			JOIN MemberData md ON md.SSAN = s.member_ssn
			LEFT JOIN FORM348_SARC_findings approver ON approver.SARC_ID = s.sarc_id AND approver.ptype = 10
			JOIN vw_sarc vw ON vw.sarc_id = s.sarc_id
			LEFT OUTER JOIN (
							SELECT	refId, MAX(endDate) AS CompletedDate
							FROM	dbo.core_WorkStatus_Tracking AS core_WorkStatus_Tracking_1 
									INNER JOIN dbo.vw_WorkStatus ws on core_WorkStatus_Tracking_1.ws_id = ws.ws_id
							WHERE	module = 25 AND ws.isFinal = 1 AND ws.isCancel = 0
							GROUP BY refId
					) AS comp ON comp.refId = s.sarc_id
	WHERE 
			s.sarc_id = @refId


	IF (@hasOtherICDs = 1)
	BEGIN
		SELECT	@icdOther += (c.text + '; ')
		FROM	Form348_SARC_ICD s
				JOIN core_lkupICD9 c ON s.ICDCodeId = c.ICD9_ID
		WHERE	s.SARCId = @refId
	END

	SET @diagnosis = @icdE968 + @icdE969 + ISNULL(@icdOther, '')

	IF (NOT @appealAddressStreet IS NULL
		AND NOT @appealAddressCity IS NULL
		AND NOT @appealAddressState IS NULL
		AND NOT @appealAddressZip IS NULL
		AND NOT @appealAddressCountry IS NULL)
	BEGIN
		SET @more = 1
		SET @Contact = @appealAddressStreet + ', ' + @appealAddressCity + ', ' + @appealAddressState + ' ' + @appealAddressZip + ', ' + @appealAddressCountry
	END

	IF (NOT @email IS NULL)
	BEGIN
		IF(@more = 1)
		BEGIN
			SET @Contact = @Contact + ' or ' + @Email
		END
		ELSE
		BEGIN
			SET @more = 1

			SET @Contact = @Email
		END
	END

	IF (NOT @helpExtension IS NULL)
	BEGIN
		IF(@more = 1)
		BEGIN
			SET @Contact = @Contact + ' or ' + @helpExtension
		END
		ELSE
		BEGIN
			SET @more = 1

			SET @Contact = @helpExtension
		END
	END


	INSERT INTO @results (id, value) VALUES ('MEMBER_RANK', @rank);
	INSERT INTO @results (id, value) VALUES ('MEMBER_NAME', @name);
	INSERT INTO @results (id, value) VALUES ('MEMBER_ADDRESS', @address);
	INSERT INTO @results (id, value) VALUES ('MEMBER_CITY_STATE_ZIP', @addressCity);
	INSERT INTO @results (id, value) VALUES ('MEMBER_UNIT', @unit);
	INSERT INTO @results (id, value) VALUES ('CASE_ID', @caseId);
	INSERT INTO @results (id, value) VALUES ('DIAGNOSIS', @diagnosis);
	INSERT INTO @results (id, value) VALUES ('EXTENSION_NUMBER', @helpExtension);
	INSERT INTO @results (id, value) VALUES ('FINAL_AUTHORITY_NAME', @apprAuthName);
	INSERT INTO @results (id, value) VALUES ('WING_SARC_RSL', @appealAddress);
	INSERT INTO @results (id, value) VALUES ('DATE_OF_COMPLETION', @dateOfCompletion);
	INSERT INTO @results (id, value) VALUES ('CONTACT', @contact);

	SELECT id, value FROM @results
END
GO

