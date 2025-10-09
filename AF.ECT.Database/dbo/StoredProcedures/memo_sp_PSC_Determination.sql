-- ============================================================================
-- Author:		Eric Kelley
-- Create date: 03-Sep-2021
-- Description:	The stored procedure used for getting the parameter data for
--				the PSC Determination memo. 
-- ============================================================================
-- Modified By:		Eric Kelley
-- Modified date:	14-Sep-2021
-- Description:	 Capitalized variables to make them work
-- ============================================================================
-- Modified By:		Eric Kelley
-- Modified date:	15-Sep-2021
-- Description:	 Add SMR, Board Legal, and Approving Authorty's name & rank. 
-- ============================================================================
-- Modified By:		Eric Kelley
-- Modified date:	24-Sep-2021
-- Description:	 Add SMR, Board Legal, and Approving Authorty's remarks 
-- ============================================================================
-- Modified By:		Eric Kelley
-- Modified date:	27-Sep-2021
-- Description:	 Made corrections to the @aAuthorityWSRemarks, @bLegalWSRemarks
-- @aAuthorityWSRemarks
-- ============================================================================
-- Modified By:		Eric Kelley
-- Modified date:	06-Oct-2021
-- Description:	 Added @bMedWSRemarks
-- ============================================================================
-- Modified By:		Gary Colbert
-- Modified date:	06-Oct-2021
-- Description:	 Added @bMedName and @bMedRank, removed @SMRName and @SMRRank
-- ============================================================================
-- Modified By:		Eric Kelley
-- Modified date:	07-Oct-2021
-- Description:	 Added @bTechMemoText, @bMedMemoText, @aAuthorityMemoText
-- ============================================================================
-- Modified By:		Gary Colbert
-- Modified date:	08-Oct-2021
-- Description:	 changed @smrMemoText to @bLegalMemoText
-- ============================================================================
-- Modified By:		Gary Colbert
-- Modified date:	13-Oct-2021
-- Description:	 added @rmu
-- ============================================================================
CREATE PROCEDURE [dbo].[memo_sp_PSC_Determination]
	@refId INT
AS 

SET NOCOUNT ON;

DECLARE @results TABLE
(
	id varchar(100),
	value varchar(MAX)
);

DECLARE 
@memberRank VARCHAR(100)
,@fullname VARCHAR(100)
,@lastname VARCHAR(100)
,@address VARCHAR(200)
,@addressCity VARCHAR(200)
,@unit VARCHAR(100)
,@rmu VARCHAR (100)
,@caseId VARCHAR(50)
,@diagnosis VARCHAR(MAX)
,@ftdiagnosis VARCHAR(MAX)
,@helpExtension NVARCHAR(50)
,@email VARCHAR(100)
,@appealAddress VARCHAR(200)
,@chkEmail INT
,@chkPhone INT
,@chkAddress INT
,@more INT = 0
,@contact VARCHAR(MAX)
,@lastFour SMALLINT
,@bTechFindings VARCHAR(300)
,@bTechFindingsDate DATETIME
,@bTechWSRemarks VARCHAR(300)
,@bTechMemoText VARCHAR(300)
,@bMedFindings VARCHAR(300)
,@bMedFindingsDate DATETIME
,@bMedWSRemarks VARCHAR(300)
,@bMedName VARCHAR(100)
,@bMedRank VARCHAR(100)
,@bMedMemoText VARCHAR(300)
,@smrFindings VARCHAR(300)
,@smrWSRemarks VARCHAR(300)
,@bLegalMemoText VARCHAR(300)
,@smrFindingsDate DATETIME
,@bLegalName VARCHAR(100)
,@bLegalRank VARCHAR(100)
,@bLegalFindings VARCHAR(300)
,@bLegalWSRemarks VARCHAR(300)
,@bLegalFindingsDate DATETIME
,@aAuthorityName VARCHAR(100)
,@aAuthorityRank VARCHAR(100)
,@aAuthorityFindings VARCHAR(300)
,@aAuthorityWSRemarks VARCHAR(300)
,@aAuthorityMemoText VARCHAR(300)
,@aAuthorityFindingsDate DATETIME

SELECT
	 @memberRank = g.RANK
	,@fullname = md.LAST_NAME + ', ' + md.FIRST_NAME + ' ' +(CASE md.MIDDLE_NAMES WHEN NULL THEN '' WHEN '' THEN '' ELSE LEFT(md.MIDDLE_NAMES, 1) + ' ' END)
	,@lastname = md.LAST_NAME
	,@lastFour = Right(sc.Member_ssn, 4)
	,@addressCity = ISNULL(md.LOCAL_ADDR_CITY,'') + ', ' + ISNULL(md.LOCAL_ADDR_STATE, '') + ' ' + ISNULL(md.ADRS_MAIL_ZIP, '')
	,@address = md.LOCAL_ADDR_STREET
	,@unit = sc.member_unit 
	,@rmu = (Select distinct rmu.RMU from core_lkupRMUs rmu
				left join Form348_SC sc on rmu.id=sc.RMU_Name
				left join Form348_PSID_Findings f on f.PSID_ID=sc.SC_Id
				where f.PSID_ID = @refId )
	,@caseId = sc.case_id
	,@diagnosis = (select icd9_description from Form348_SC where SC_ID = @refId)
	,@ftdiagnosis = (select FT_Diagnosis from Form348_SC where SC_Id = @refId)
	,@bTechFindings = (SELECT Description From Form348_PSID_Findings a join core_lkUpFindings b on a.Finding = b.Id where PSID_ID = @refId and PType = 1)
	,@bTechWSRemarks = (SELECT Remarks FROM Form348_PSID_Findings where PSID_ID = @refId and PType = 1)
	,@bTechMemoText = (SELECT FindingText FROM Form348_PSID_Findings where PSID_ID = @refId and PType = 1)
	,@bTechFindingsDate = (SELECT ModifiedDate From Form348_PSID_Findings where PSID_ID = @refId and PType = 1)
	,@bMedFindings = (SELECT Description From Form348_PSID_Findings a join core_lkUpFindings b on a.Finding = b.Id where PSID_ID = @refId and PType = 8)
	,@bMedFindingsDate =(SELECT ModifiedDate From Form348_PSID_Findings where PSID_ID = @refId and PType = 8)
	,@bMedWSRemarks = (SELECT Remarks FROM Form348_PSID_Findings where PSID_ID = @refId and PType = 8)
	,@bMedMemoText = (SELECT FindingText FROM Form348_PSID_Findings where PSID_ID = @refId and PType = 8)
	,@bMedName = (SELECT Name From Form348_PSID_Findings where PSID_ID = @refId and PType = 8)
	,@bMedRank = (SELECT Top (1) Rank FROM core_Users a 
				 join core_lkupGrade b on b.CODE = a.rank_code
				 join Form348_PSID_Findings c on c.ModifiedBy = a.userID
				 where PSID_ID = @refId and PType = 8 and FindingText <> ''
				 order by c.ModifiedDate desc)
	,@smrFindings = (SELECT Description From Form348_PSID_Findings a join core_lkUpFindings b on a.Finding = b.Id where PSID_ID = @refId and PType = 27)
	,@smrWSRemarks = (SELECT AdditionalRemarks FROM Form348_PSID_Findings where PSID_ID = @refId and PType = 27)
	,@bLegalMemoText = (SELECT FindingText FROM Form348_PSID_Findings where PSID_ID = @refId and PType = 7)
	,@smrFindingsDate = (SELECT ModifiedDate From Form348_PSID_Findings where PSID_ID = @refId and PType = 27)
	,@bLegalName = (SELECT Name From Form348_PSID_Findings where PSID_ID = @refId and PType = 7)
	,@bLegalRank = (SELECT Top (1) Rank FROM core_Users a 
				   join core_lkupGrade b on b.CODE = a.rank_code
				   join Form348_PSID_Findings c on c.ModifiedBy = a.userID
				   where PSID_ID = @refId and PType = 7 and FindingText <> ''
				   order by c.ModifiedDate desc)
	,@bLegalFindings = (SELECT Description From Form348_PSID_Findings a join core_lkUpFindings b on a.Finding = b.Id where PSID_ID = @refId and PType = 7)
	,@bLegalWSRemarks = (SELECT Remarks FROM Form348_PSID_Findings where PSID_ID = @refId and PType = 7)
	,@bLegalFindingsDate = (SELECT ModifiedDate From Form348_PSID_Findings where PSID_ID = @refId and PType = 7)
	,@aAuthorityName = (SELECT Name From Form348_PSID_Findings where PSID_ID = @refId and PType = 6)
	,@aAuthorityRank = (SELECT Top (1) Rank FROM core_Users a 
				       join core_lkupGrade b on b.CODE = a.rank_code
				       join Form348_PSID_Findings c on c.ModifiedBy = a.userID
				       where PSID_ID = @refId and PType = 6 and FindingText <> ''
				       order by c.ModifiedDate desc)
	,@aAuthorityFindings = (SELECT Description From Form348_PSID_Findings a join core_lkUpFindings b on a.Finding = b.Id where PSID_ID = @refId and PType = 6)
	,@aAuthorityWSRemarks = (SELECT Remarks FROM Form348_PSID_Findings where PSID_ID = @refId and PType = 6)
	,@aAuthorityMemoText = (SELECT FindingText FROM Form348_PSID_Findings where PSID_ID = @refId and PType = 6)
	,@aAuthorityFindingsDate  = (SELECT ModifiedDate From Form348_PSID_Findings where PSID_ID = @refId and PType = 6)
FROM [ALOD].[dbo].[Form348_SC] sc
JOIN MemberData md on md.SSAN = sc.Member_ssn
JOIN core_lkupGrade g on g.CODE = sc.Member_Grade
JOIN Form348_PSID_Findings pf on pf.PSID_ID = sc.SC_Id
--where sc.SC_Id = @refId

INSERT INTO @results (id, value) VALUES ('MEMBER_RANK', @memberRank);
INSERT INTO @results (id, value) VALUES ('MEMBER_NAME', @fullname); -- Last, First M
INSERT INTO @results (id, value) VALUES ('MEMBER_LASTNAME', @lastname);
INSERT INTO @results (id, value) VALUES ('MEMBER_ADDRESS', @address);
INSERT INTO @results (id, value) VALUES ('MEMBER_LASTFOUR', @lastFour);
INSERT INTO @results (id, value) VALUES ('MEMBER_CITY_STATE_ZIP', @addressCity);
INSERT INTO @results (id, value) VALUES ('MEMBER_UNIT', @unit);
INSERT INTO @results (id, value) VALUES ('MEMBER_RMU', @rmu);
INSERT INTO @results (id, value) VALUES ('DIAGNOSIS', lower(@diagnosis));
INSERT INTO @results (id, value) VALUES ('FT_DIAGNOSIS', @ftdiagnosis);
INSERT INTO @results (id, value) VALUES ('CASE_ID', @caseId);
INSERT INTO @results (id, value) VALUES ('BOARDTECH_FINDINGS', @bTechFindings);
INSERT INTO @results (id, value) VALUES ('BOARDTECH_FINDINGS_DATE', @bTechFindingsDate);
INSERT INTO @results (id, value) VALUES ('BOARDTECH_WSREMARKS', @bTechWSRemarks);
INSERT INTO @results (id, value) VALUES ('BOARDTECH_MEMOTEXT', @bTechMemoText);
INSERT INTO @results (id, value) VALUES ('BOARDMEDICAL_FINDINGS', @bMedFindings);
INSERT INTO @results (id, value) VALUES ('BOARDMEDICAL_FINDINGS_DATE', @bMedFindingsDate);
INSERT INTO @results (id, value) VALUES ('BOARDMEDICAL_WSREMARKS', @bMedWSRemarks);
INSERT INTO @results (id, value) VALUES ('BOARDMEDICAL_NAME', @bMedName);
INSERT INTO @results (id, value) VALUES ('BOARDMEDICAL_RANK', @bMedRank);
INSERT INTO @results (id, value) VALUES ('BOARDMEDICAL_MEMOTEXT', @bMedMemoText);
INSERT INTO @results (id, value) VALUES ('SENIORMEDREV_FINDINGS', @smrFindings);
INSERT INTO @results (id, value) VALUES ('SENIORMEDREV_WSREMARKS', @smrWSRemarks);
INSERT INTO @results (id, value) VALUES ('SENIORMEDREV_FINDINGS_DATE', @smrFindingsDate);
INSERT INTO @results (id, value) VALUES ('BOARDLEGAL_NAME', @bLegalName);
INSERT INTO @results (id, value) VALUES ('BOARDLEGAL_MEMOTEXT', @bLegalMemoText);
INSERT INTO @results (id, value) VALUES ('BOARDLEGAL_RANK', @bLegalRank);
INSERT INTO @results (id, value) VALUES ('BOARDLEGAL_FINDINGS', @bLegalFindings);
INSERT INTO @results (id, value) VALUES ('BOARDLEGAL_WSREMARKS', @bLegalWSRemarks);
INSERT INTO @results (id, value) VALUES ('BOARDLEGAL_FINDINGS_DATE', @bLegalFindingsDate);
INSERT INTO @results (id, value) VALUES ('APPROVINGAUTHORITY_NAME', @aAuthorityName);
INSERT INTO @results (id, value) VALUES ('APPROVINGAUTHORITY_RANK', @aAuthorityRank);
INSERT INTO @results (id, value) VALUES ('APPROVINGAUTHORITY_FINDINGS', @aAuthorityFindings);
INSERT INTO @results (id, value) VALUES ('APPROVINGAUTHORITY_WSREMARKS', @aAuthorityWSRemarks);
INSERT INTO @results (id, value) VALUES ('APPROVINGAUTHORITY_MEMOTEXT', @aAuthorityMemoText);
INSERT INTO @results (id, value) VALUES ('APPROVINGAUTHORITY_FINDINGS_DATE', @aAuthorityFindingsDate);


SELECT id, value FROM @results

--[dbo].[memo_sp_PSC_Determination] 26097
GO

