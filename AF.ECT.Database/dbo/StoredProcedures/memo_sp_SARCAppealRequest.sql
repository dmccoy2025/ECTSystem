
-- ============================================================================
-- Author:			Evan Morrison
-- Create date:		1/10/2017
-- Description:		The stored procedure used for getting the parameter data for
--					the SARC Appeal Request memo. 
-- ============================================================================
-- Edited By:			Evan Morrison
-- Edit date:		5/2/2017
-- Description:		Puts case id of the original case instead of ref id
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	5/15/2017
-- Description:		Update memo input information
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	6/9/2017
-- Description:		- Modified to use the APSA cases member info instead of
--					the LODs/SARCs member info.
--					- Changed @results.value declaration from VARCHAR(100) to 
--					VARCHAR(MAX)
-- ============================================================================
CREATE PROCEDURE [dbo].[memo_sp_SARCAppealRequest]
	@refId int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @results TABLE
	(
		id varchar(100),
		value varchar(MAX)
	);

	DECLARE	@rank varchar(6),
			@name varchar(1000),
			@LastName varchar(50),
			@beginDate datetime,
			@sigBlock varchar(1000),
			@helpExtension NVARCHAR(50) = '',
			@findings varchar(100),
			@workflow INT,
			@caseId VARCHAR(50),
			@fullname VARCHAR(1000)

	SET @workflow = (SELECT TOP 1 initial_workflow FROM Form348_AP_SARC WHERE appeal_sarc_id = @refId)

	IF(@workflow = 1)
	BEGIN
		SELECT	@rank = g.RANK,
				@name = md.FIRST_NAME + ' ' + (CASE md.MIDDLE_NAMES WHEN NULL THEN '' WHEN '' THEN '' ELSE LEFT(md.MIDDLE_NAMES, 1) + ' ' END) + md.LAST_NAME,
				@caseId = lod.case_id,
				@LastName = md.LAST_NAME
		FROM	Form348_AP_SARC apsa
				JOIN Form348 lod ON lod.lodId = apsa.initial_id
				JOIN core_lkupGrade g ON g.CODE = apsa.member_grade
				JOIN MemberData md ON md.SSAN = apsa.member_ssn
		WHERE	apsa.appeal_sarc_id = @refId
	END
	ELSE
	BEGIN
		SELECT	@rank = g.RANK,
				@name = md.FIRST_NAME + ' ' + (CASE md.MIDDLE_NAMES WHEN NULL THEN '' WHEN '' THEN '' ELSE LEFT(md.MIDDLE_NAMES, 1) + ' ' END) + md.LAST_NAME,
				@caseId = sarc.case_id,
				@LastName = md.LAST_NAME
		FROM	Form348_AP_SARC apsa
				JOIN Form348_SARC sarc ON sarc.sarc_id = apsa.initial_id
				JOIN core_lkupGrade g ON g.CODE = apsa.member_grade
				JOIN MemberData md ON md.SSAN = apsa.member_ssn
		WHERE	apsa.appeal_sarc_id = @refId
	END


	INSERT INTO @results (id, value) VALUES ('MEMBER_RANK', @rank);
	INSERT INTO @results (id, value) VALUES ('MEMBER_NAME', @name);
	INSERT INTO @results (id, value) VALUES ('CASE_ID', @caseId);
	INSERT INTO @results (id, value) VALUES ('MEMBER_LAST_NAME', @LastName);


	SELECT id, value FROM @results
END
GO

