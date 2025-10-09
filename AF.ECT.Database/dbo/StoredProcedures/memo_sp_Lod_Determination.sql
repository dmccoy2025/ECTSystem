
-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	The stored procedure used for getting the parameter data for
--				the ILOD determination memo. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	12/23/2015
-- Work Item:		TFS Task 393
-- Description:		Added the selection of the EXTENSION_NUMBER placeholder.
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	5/15/2017
-- Description:		Update memo input information
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	6/27/2017
-- Description:		Combine contact information to be included based on flags
-- ============================================================================
CREATE PROCEDURE [dbo].[memo_sp_Lod_Determination]
	@refId int
AS

SET NOCOUNT ON;

DECLARE @results TABLE
(
	id varchar(100),
	value varchar(MAX)
);

DECLARE 
@rank VARCHAR(100)
,@name VARCHAR(100)
,@address VARCHAR(200)
,@addressCity VARCHAR(200)
,@unit VARCHAR(100)
,@caseId VARCHAR(50)
,@diagnosis VARCHAR(MAX)
,@helpExtension NVARCHAR(50)
,@email VARCHAR(100)
,@appealAddress VARCHAR(200)
,@chkEmail INT
,@chkPhone INT
,@chkAddress INT
,@more INT = 0
,@contact VARCHAR(MAX)

SELECT 
	@rank = g.RANK
	,@name = md.FIRST_NAME + ' ' + (CASE md.MIDDLE_NAMES WHEN NULL THEN '' WHEN '' THEN '' ELSE LEFT(md.MIDDLE_NAMES, 1) + ' ' END) +  md.LAST_NAME
	,@address = md.LOCAL_ADDR_STREET
	,@addressCity = ISNULL(md.LOCAL_ADDR_CITY,'') + ', ' + ISNULL(md.LOCAL_ADDR_STATE, '') + ' ' + ISNULL(md.ADRS_MAIL_ZIP, '')
	,@unit = a.member_unit 
	,@caseId = a.case_id
	,@diagnosis = med.diagnosis_text
	,@helpExtension = post.helpExtensionNumber
	,@email = post.email
	,@appealAddress = post.appealStreet + ', ' + post.appealCity + ', ' + post.appealState + ' ' + post.appealZip + ', ' + post.appealCountry
	,@ChkEmail = email_flag
	,@ChkPhone = phone_flag
	,@ChkAddress = address_flag
FROM Form348 a
JOIN core_lkupGrade g ON g.CODE = a.member_grade
JOIN MemberData md ON md.SSAN = a.member_ssn 
JOIN Form348_Medical med ON med.lodid = a.lodId
JOIN Form348_PostProcessing post ON post.lodId = a.lodId
WHERE a.lodId = @refId


if(@ChkAddress = 1)
BEGIN
	SET @more = 1

	SET @Contact = @appealAddress

END

if(@ChkEmail = 1)
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

if(@ChkPhone = 1)
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
INSERT INTO @results (id, value) VALUES ('DIAGNOSIS', lower(@diagnosis));
INSERT INTO @results (id, value) VALUES ('CASE_ID', @caseId);
INSERT INTO @results (id, value) VALUES ('CONTACT', @Contact);


SELECT id, value FROM @results
GO

