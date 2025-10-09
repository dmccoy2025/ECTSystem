

-- ============================================================================
-- Author:			Evan Morrison
-- Create date:		7/22/2016
-- Description:		The stored procedure used for getting the parameter data for
--					the Appeal Request memo. 
-- ============================================================================
-- Edited By:		Evan Morrison
-- Edit date:		5/1/2017
-- Description:		Print out the case id instead of the ref id of the original
--					LOD case
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	5/15/2017
-- Description:		Update memo input information
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	6/9/2017
-- Description:		- Modified to use the AP cases member unit info instead of
--					the LODs member unit info.
-- ============================================================================
-- Modified By:		Darel Johnson
-- Modified Date:	08/20/2020
-- Description:		- Modified to use the correct label for the FROM field
--					in the memo based on the member's compo.
-- ============================================================================
CREATE PROCEDURE [dbo].[memo_sp_AppealRequest]
	@refId int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @results TABLE
	(
		id varchar(100),
		value varchar(100)
	);

	DECLARE	@rank varchar(6),
			@name varchar(100),
			@LastName varchar(50),
			@beginDate datetime,
			@sigBlock varchar(1000),
			@helpExtension NVARCHAR(50) = '',
			@findings varchar(100),
			@case_id VARCHAR(50),
			@fullname VARCHAR(50),
			@from varchar(50)

	SELECT	@rank = mg.RANK,
			@name = md.FIRST_NAME + ' ' + (CASE md.MIDDLE_NAMES WHEN NULL THEN '' WHEN '' THEN '' ELSE LEFT(md.MIDDLE_NAMES, 1) + ' ' END) + md.LAST_NAME,
			@case_id = a.case_id,
			@LastName = md.LAST_NAME,
			@from = (CASE a.member_compo WHEN '6' THEN 'HQ AFRC/CV' ELSE 'ANGRC/CC' END)
	FROM	Form348_AP ap
			JOIN Form348 a ON a.lodId = ap.initial_lod_id
			LEFT JOIN core_lkUpFindings fndgs ON fndgs.Id = a.FinalFindings
			JOIN core_lkupGrade mg on mg.CODE = ap.member_grade
			LEFT JOIN VW_MEMBERDATA md ON md.ssn= ap.member_ssn 
	WHERE	appeal_id = @refId


	INSERT INTO @results (id, value) VALUES ('MEMBER_RANK', @rank);
	INSERT INTO @results (id, value) VALUES ('MEMBER_NAME', @name);
	INSERT INTO @results (id, value) VALUES ('CASE_ID', @case_id);
	INSERT INTO @results (id, value) VALUES ('MEMBER_LAST_NAME', @LastName);
	INSERT INTO @results (id, value) VALUES ('FROM', @from);

	SELECT id, value FROM @results
END
GO

