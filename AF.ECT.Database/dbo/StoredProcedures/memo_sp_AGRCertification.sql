

-- =============================================
-- Author:		Darel Johnson
-- Create date: 04/02/2020
-- Description:	Provides Key value pairs for interpolation merge with Memo Template
-- =============================================
-- Darel Johnson: Added POC address 2
-- =============================================
CREATE PROCEDURE [dbo].[memo_sp_AGRCertification]
	@refId int

AS

--DECLARE @refId int
--SET @refId = 297

--exec  [dbo].[memo_sp_AGRCertification] 25945

DECLARE @results TABLE
(
	id varchar(100),
	value varchar(MAX)
);

DECLARE @name varchar(100), 
		@unit varchar(100),  
		@rank varchar(6), 
		@afsc varchar(20),
		@signature varchar(100),
		@poc_address  varchar(200),
		@poc_unit varchar(200),
		@poc_ph varchar(200),
		@poc_email varchar(200),
		@poc_address2  varchar(200)

SELECT 
	@name = MD.LAST_NAME + ', ' + MD.FIRST_NAME + ' ' + ISNULL(MD.MIDDLE_NAMES,'') 
	, @unit = PC.LONG_NAME
	, @rank = Grade.RANK
    , @afsc = SpecialCase.DAFSC
	, @signature = MC.sigBlock
	, @poc_unit = ISNULL(SpecialCase.POC_Unit,'')
	, @poc_ph = ISNULL(SpecialCase.POC_Phone_DSN,'')
	, @poc_email = ISNULL(SpecialCase.POC_Email,'')
	, @poc_address = ISNULL(SpecialCase.Military_Treatment_Facility_Initial,'')
	, @poc_address2 = ISNULL(SpecialCase.Military_Treatment_Facility_City_State_Zip,'')
FROM 
	Form348_SC SpecialCase
		LEFT JOIN core_lkupGrade Grade ON Grade.CODE = SpecialCase.member_grade
		LEFT JOIN VW_MEMBERDATA MD ON MD.ssn= SpecialCase.member_ssn
		LEFT JOIN vw_pasCode PC On MD.PAS_NUMBER = PC.PAS_CODE
		LEFT JOIN core_MemoContents MC On MC.memoid = SpecialCase.memo_template_id
WHERE SpecialCase.SC_Id = @refId


INSERT INTO @results (id, value) VALUES ('MEMBER_NAME', @name);
INSERT INTO @results (id, value) VALUES ('MEMBER_RANK', @rank);
INSERT INTO @results (id, value) VALUES ('MEMBER_UNIT_NAME', @unit);
INSERT INTO @results (id, value) VALUES ('AFSC', @afsc);
INSERT INTO @results (id, value) VALUES ('SIGNATURE', @signature);
INSERT INTO @results (id, value) VALUES ('POC_Unit', @poc_unit);
INSERT INTO @results (id, value) VALUES ('POC_Phone_DSN', @poc_ph);
INSERT INTO @results (id, value) VALUES ('POC_Email', @poc_email);
INSERT INTO @results (id, value) VALUES ('POC_Address', @poc_address);
INSERT INTO @results (id, value) VALUES ('POC_Address2', @poc_address2);

SELECT id, value FROM @results
GO

