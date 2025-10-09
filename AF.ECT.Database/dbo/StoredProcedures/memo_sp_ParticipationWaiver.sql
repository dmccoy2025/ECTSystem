-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[memo_sp_ParticipationWaiver]
	@refId int

AS

--DECLARE @refId int
--SET @refId = 297



DECLARE @results TABLE
(
	id varchar(100),
	value varchar(MAX)
);

DECLARE @name varchar(100), @unit varchar(100),  @rank varchar(6), @address varchar(200), @member_ssn varchar(12), @diagnosis varchar(50), @paraInfo varchar(20)
--, @appAuthName varchar(100), @lodId int
--Maj Small, David W., XXX-XX-XXXX  197008606   197-00-8606
-- exec memo_sp_ParticipationWaiver 69

SELECT 
	@name = MD.LAST_NAME + ', ' + MD.FIRST_NAME + ' ' + ISNULL(MD.MIDDLE_NAMES,'') 
	--, @unit = member_unit
	, @rank = Grade.RANK
	, @member_ssn = LEFT (MD.SSN , 3) + '-' + SUBSTRING(MD.SSN, 4, 2) + '-' +  RIGHT(MD.SSN,4)
--	, @appAuthName = rr.sig_name_approval
FROM 
	Form348_SC SpecialCase
		LEFT JOIN core_lkupGrade Grade ON Grade.CODE = SpecialCase.member_grade
		LEFT JOIN VW_MEMBERDATA MD ON MD.ssn= SpecialCase.member_ssn
WHERE SpecialCase.SC_Id = @refId

Select @diagnosis = Name, @paraInfo = Para_Info from core_lkupPWCategories 
where Id = (SELECT PWaiver_Category FROM Form348_SC WHERE SC_Id  = @refId)


--SET @address = (SELECT LOCAL_ADDR_STREET + ' ' + LOCAL_ADDR_CITY + ', ' + LOCAL_ADDR_STATE + ' ' + ADRS_MAIL_ZIP
--					FROM MemberData WHERE SSAN = (SELECT member_ssn FROM Form348_SC WHERE SC_Id  = @refId)
--				)

--INSERT INTO @results (id, value) VALUES ('APPROVING_AUTHORITY', @appAuthName);
INSERT INTO @results (id, value) VALUES ('MEMBER_NAME', @name);
INSERT INTO @results (id, value) VALUES ('MEMBER_RANK', @rank);
--INSERT INTO @results (id, value) VALUES ('MEMBER_UNIT_NAME', @unit);
--INSERT INTO @results (id, value) VALUES ('MEMBER_ADDRESS', @address);
INSERT INTO @results (id, value) VALUES ('MEMBER_SSN', @member_ssn);
INSERT INTO @results (id, value) VALUES ('DIAGNOSIS', @diagnosis);
INSERT INTO @results (id, value) VALUES ('PARAGRAPH_INFO', @paraInfo);
--


SELECT id, value FROM @results

--SELECT datename(dw,c.io_completionDate) + ' ' + datename(dd,c.io_completionDate) + ' ' + datename(mm,c.io_completionDate) + ' ' + datename(yyyy,c.io_completionDate)
GO

