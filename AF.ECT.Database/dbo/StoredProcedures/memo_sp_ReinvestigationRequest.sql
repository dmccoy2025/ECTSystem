
-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	2/17/2015
-- Work Item:		TFS Bug 267
-- Description:		Added the variables and statement to construct the HQ
--					AFRC Address from the values in the core_KeyVal_* tables. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	11/27/2015
-- Work Item:		TFS Task 289
-- Description:		Altered the MAIN SELECT statement's WHERE clause to also
--					match the request_id of the Form348_RR records against the
--					@refId. This was done because multiple RR cases can now map
--					to the same LOD case. 
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	5/15/2017
-- Description:		Update memo input information
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	6/9/2017
-- Description:		- Modified to use the RR cases member info instead of
--					the LODs member info.
-- ============================================================================
CREATE PROCEDURE [dbo].[memo_sp_ReinvestigationRequest]
	@refId int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @results TABLE
	(
		id varchar(100),
		value varchar(MAX)
	);

	DECLARE	@unit varchar(100),
			@name varchar(100),
			@lastName varchar(100),
			@rank varchar(6),
			@address varchar(200),
			@appAuthName varchar(100),
			@lodId int,
			@keyDesc VARCHAR(20) = 'HQ AFRC Address',
			@keyId INT = 0,
			@hqAddrLine1 varchar(100),
			@hqAddrLine2 varchar(100),
			@hqAddrLine3 varchar(100),
			@caseId NVARCHAR(50)


	SELECT	@unit = r.member_unit,
			@rank = g.RANK,
			@name = md.FIRST_NAME + ' ' + LEFT(md.MIDDLE_NAMES, 1) + ' ' + md.LAST_NAME,
			@caseId = lod.case_id,
			@lastName = md.LAST_NAME
	FROM	Form348_RR r
			JOIN Form348 lod ON lod.lodId = r.InitialLodId
			JOIN core_lkupGrade g ON g.CODE = r.member_grade
			JOIN MemberData md ON md.SSAN = r.member_ssn
	WHERE	r.request_id = @refId


	SELECT	@keyId = Id
	FROM	core_KeyVal_Key
	WHERE	[Description] = @keyDesc

	IF (@keyId > 0)
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

	INSERT INTO @results (id, value) VALUES ('MEMBER_UNIT', @unit);
	INSERT INTO @results (id, value) VALUES ('HQ_ADDR_LINE_1', @hqAddrLine1);
	INSERT INTO @results (id, value) VALUES ('HQ_ADDR_LINE_2', @hqAddrLine2);
	INSERT INTO @results (id, value) VALUES ('HQ_ADDR_LINE_3', @hqAddrLine3);
	INSERT INTO @results (id, value) VALUES ('MEMBER_RANK', @rank);
	INSERT INTO @results (id, value) VALUES ('MEMBER_NAME', @name);
	INSERT INTO @results (id, value) VALUES ('CASE_ID', @caseId);
	INSERT INTO @results (id, value) VALUES ('MEMBER_LAST_NAME', @lastName);


	SELECT id, value FROM @results
END
GO

