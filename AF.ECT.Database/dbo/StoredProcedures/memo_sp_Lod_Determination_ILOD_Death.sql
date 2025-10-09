
-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 1/14/2016
-- Description:	The data source for the In Line of Duty Death LOD determination
--				memo.
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	5/15/2017
-- Description:		Update memo input information
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	6/27/2017
-- Description:		Combine contact information to be included based on flags
-- ============================================================================
CREATE PROCEDURE [dbo].[memo_sp_Lod_Determination_ILOD_Death]
	@refId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @results TABLE
	(
		id VARCHAR(100),
		value VARCHAR(MAX)
	);

DECLARE	
@nextOfKin NVARCHAR(200)
,@unit VARCHAR(100)
,@caseId NVARCHAR(50)
,@apprAuthName VARCHAR(100)
,@appAuthName VARCHAR(100)
,@rank NVARCHAR(50)
,@name VARCHAR(100)
,@helpExtension NVARCHAR(50)
,@email VARCHAR(100)
,@appealAddress VARCHAR(200)
,@chkEmail INT
,@chkPhone INT
,@chkAddress INT
,@more INT = 0
,@contact VARCHAR(MAX)

	
SELECT 
	@nextOfKin = fpp.nokFirstName + ' ' + (CASE fpp.nokMiddleName WHEN NULL THEN '' WHEN '' THEN '' ELSE LEFT(fpp.nokMiddleName, 1) + ' ' END) + fpp.nokLastName
	,@unit = a.member_unit
	,@caseId = a.case_id
	,@apprAuthName = (approver.RANK + ' ' + approver.name + ', AFRC/A1 Approving Authority')
	,@appAuthName = (appointer.RANK + ' ' + appointer.name + ', Appointing Authority')
	,@rank = g.RANK
	,@name = m.LAST_NAME
	,@helpExtension = fpp.helpExtensionNumber
	,@email = fpp.email
	,@appealAddress = fpp.appealStreet + ', ' + fpp.appealCity + ', ' + fpp.appealState + ' ' + fpp.appealZip + ', ' + fpp.appealCountry
	,@ChkEmail = email_flag
	,@ChkPhone = phone_flag
	,@ChkAddress = address_flag
FROM Form348 a
JOIN Form348_PostProcessing fpp ON fpp.lodId = a.lodId
JOIN MemberData m ON m.SSAN = a.member_ssn
JOIN core_lkupGrade g ON g.CODE = a.member_grade
LEFT JOIN FORM348_findings approver ON approver.lodid = a.lodid AND approver.ptype = (CASE WHEN a.formal_inv = 0 THEN 10 ELSE 18 END)
LEFT JOIN FORM348_findings appointer ON appointer.lodid = a.lodid AND appointer.ptype = (CASE WHEN a.formal_inv = 0 THEN 5 ELSE 13 END)
WHERE A.lodId = @refId


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

	
	
-- Store data into the results table...
INSERT INTO @results (id, value) VALUES ('NEXT_OF_KIN_NAME', @nextOfKin);
INSERT INTO @results (id, value) VALUES ('TO_UNIT', @unit);
INSERT INTO @results (id, value) VALUES ('CASE_ID', @caseId);
INSERT INTO @results (id, value) VALUES ('FINAL_AUTHORITY_NAME', ISNull(@apprAuthName,@appAuthName));
INSERT INTO @results (id, value) VALUES ('MEMBER_RANK', @rank);
INSERT INTO @results (id, value) VALUES ('MEMBER_NAME', @name);
INSERT INTO @results (id, value) VALUES ('CONTACT', @Contact);
	
	
	-- Select all of the data...
	SELECT	r.id, r.value 
	FROM	@results r
END
GO

