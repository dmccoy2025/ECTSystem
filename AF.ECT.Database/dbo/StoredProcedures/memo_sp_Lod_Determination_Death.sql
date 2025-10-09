

-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	The stored procedure used for getting the parameter data for
--				the Death NILOD determination memo. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	11/25/2015
-- Work Item:		TFS Task 393
-- Description:		Updated to work with the new AFI 36-2910 determination 
--					memos.
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	5/15/2017
-- Description:		Update memo input information
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	7/27/2017
-- Description:		Combine contact information to be included based on flags
-- ============================================================================
CREATE PROCEDURE [dbo].[memo_sp_Lod_Determination_Death]

@refId int

AS

SET NOCOUNT ON;

DECLARE @results TABLE
(
	id varchar(100),
	value varchar(MAX)
);

DECLARE 
@nextOfKin NVARCHAR(200)
,@unit VARCHAR(100)
,@caseId VARCHAR(50)
,@apprAuthNameUserGroup VARCHAR(MAX)
,@appAuthNameUserGroup VARCHAR(MAX)
,@apprAuthName varchar(100)
,@appAuthName varchar(100)
,@rank NVARCHAR(50)
,@name varchar(100)
,@lodType varchar(11)
,@gender varchar(6)
,@helpExtension NVARCHAR(50)
,@email VARCHAR(100)
,@appealAddress VARCHAR(200)
,@chkEmail INT
,@chkPhone INT
,@chkAddress INT
,@more INT = 0
,@contact VARCHAR(MAX)
,@dateOfCompletion VARCHAR(50)


SELECT 
	@nextOfKin = fpp.nokFirstName + ' ' + (CASE fpp.nokMiddleName WHEN NULL THEN '' WHEN '' THEN '' ELSE LEFT(fpp.nokMiddleName, 1) + ' ' END) + fpp.nokLastName
	,@unit = a.member_unit
	,@caseId = a.case_id
	,@apprAuthNameUserGroup = (approver.RANK + ' ' + approver.name + ', AFRC/A1 Approving Authority')
	,@appAuthNameUserGroup = (appointer.RANK + ' ' + appointer.name + ', Appointing Authority')
	,@rank = g.RANK
	,@name = m.LAST_NAME
	,@lodType = (CASE WHEN a.formal_inv = 0 THEN 'an Informal' WHEN a.formal_inv = 1 THEN 'a Formal' END)
	,@apprAuthName = (approver.RANK + ' ' + approver.name)
	,@appAuthName = (appointer.RANK + ' ' + appointer.name)
	,@gender = (CASE WHEN m.SEX_SVC_MBR='F' THEN 'her' WHEN m.SEX_SVC_MBR='M' THEN 'his' ELSE 'their' END)
	,@helpExtension = fpp.helpExtensionNumber
	,@email = fpp.email
	,@appealAddress = fpp.appealStreet + ', ' + fpp.appealCity + ', ' + fpp.appealState + ' ' + fpp.appealZip + ', ' + fpp.appealCountry
	,@ChkEmail = email_flag
	,@ChkPhone = phone_flag
	,@ChkAddress = address_flag
	,@dateOfCompletion = DATENAME(MONTH, vw.[Date Completed]) + RIGHT(CONVERT(VARCHAR(12), GETDATE(), 107), 9)
FROM Form348 a
JOIN Form348_PostProcessing fpp ON fpp.lodId = a.lodId
JOIN MemberData m ON m.SSAN = a.member_ssn
JOIN core_lkupGrade g ON g.CODE = a.member_grade
LEFT JOIN FORM348_findings approver ON approver.lodid = a.lodid AND approver.ptype = (CASE WHEN a.formal_inv = 0 THEN 10 ELSE 18 END)
LEFT JOIN FORM348_findings appointer ON appointer.lodid = a.lodid AND appointer.ptype = (CASE WHEN a.formal_inv = 0 THEN 5 ELSE 13 END)
JOIN vw_lod vw ON vw.lodId = a.lodId
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



INSERT INTO @results (id, value) VALUES ('NEXT_OF_KIN_NAME', @nextOfKin);
INSERT INTO @results (id, value) VALUES ('TO_UNIT', @unit);
INSERT INTO @results (id, value) VALUES ('CASE_ID', @caseId);
INSERT INTO @results (id, value) VALUES ('FINAL_AUTHORITY_NAME_AND_USER_GROUP', ISNull(@apprAuthNameUserGroup,@appAuthNameUserGroup));
INSERT INTO @results (id, value) VALUES ('MEMBER_RANK', @rank);
INSERT INTO @results (id, value) VALUES ('MEMBER_NAME', @name);
INSERT INTO @results (id, value) VALUES ('LOD_TYPE', @lodType);
INSERT INTO @results (id, value) VALUES ('MEMBER_GENDER', @gender);
INSERT INTO @results (id, value) VALUES ('APPOINTING_POC', @appAuthName);
INSERT INTO @results (id, value) VALUES ('DATE_OF_COMPLETION', @dateOfCompletion);
INSERT INTO @results (id, value) VALUES ('CONTACT', @Contact);


SELECT id, value FROM @results
GO

