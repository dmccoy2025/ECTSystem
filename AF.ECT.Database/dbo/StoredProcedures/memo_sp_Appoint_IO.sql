

--EXEC  memo_sp_Appoint_IO 52

-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	10/22/2015
-- Description:		Due to a change in how the Wing Commander's (Appointing
--					Authority) signature is put on the IO memo the following
--					values were commented out:
--						APPOINTING_FNAME
--						APPOINTING_MI
--						APPOINTING_LNAME
--						APPOINTING_RANK
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	1/7/2016
-- Description:		Altered the vw_users join statement to match the join
--					against the IO user ID stored in the Form348 table instead
--					of the Form 261 table. 
-- ============================================================================
CREATE PROCEDURE [dbo].[memo_sp_Appoint_IO]
	@refId int
AS

--DECLARE @refId int
--SET @refId = 297



DECLARE @results TABLE
(
	id varchar(100),
	value varchar(MAX)
);

DECLARE @name varchar(100), @rank varchar(6), @address varchar(200)
, @ioName varchar(100), @ioRankName varchar(100), @ioRank varchar(6), @appPOC varchar(400)
, @appInstructionsToIo varchar(max)
, @aaUnitName varchar(100)
, @aaFname varchar(100), @aaMname varchar(100),@aaLname varchar(100), @aaRank nvarchar(6)
--, @appAuthName varchar(100)  @ioUnit varchar(100), , @incidentType varchar(100) @unit varchar(100), @ioCompletionDate varchar(100),

SELECT 
	@name = h.FIRST_NAME + ' ' +  h.LAST_NAME
	--, @unit = member_unit
	, @rank = b.RANK
	, @appPOC = isnull(a.io_poc_info,'No Poc Entered')
	, @ioName = dbo.InitCap(e.FirstName) + ' ' + dbo.InitCap(e.LastName)
	, @ioRankName = upper(e.[rank] + ' ' + e.FirstName + ' ' + e.LastName)
	, @ioRank = e.[rank]
	--, @ioUnit = e.unit_description
	--, @ioCompletionDate = datename(dw,a.io_completion_date) + ' - ' + datename(dd,a.io_completion_date) + ' ' + datename(mm,a.io_completion_date) + ' ' + datename(yyyy,a.io_completion_date)-- + ' ' + DATEPART(D,c.io_completionDate) + ' ' + DATENAME(MM, c.io_completionDate)
	, @appInstructionsToIo = isnull(a.io_instructions,'No Additional Instructions')
	--, @incidentType = LOWER(f.event_nature_type)
	--, @appAuthName = UPPER(g.RANK + ' ' + g.FirstName + ' ' + g.LastName)
	, @aaUnitName = g.unit_description
	, @aaFname = g.FirstName, @aaMname = g.MiddleName, @aaLname = g.LastName, @aaRank = g.RANK 
FROM 
	Form348 a
	LEFT JOIN core_lkupGrade b ON b.CODE = a.member_grade
	--LEFT JOIN Form261 c ON c.lodId=a.lodId
	--LEFT JOIN vw_users e ON e.userID=c.IoUserId 
	LEFT JOIN vw_users e ON e.userID=a.io_uid
	--LEFT JOIN Form348_Medical f ON f.lodid=a.lodId
	LEFT JOIN vw_users g ON g.userID=a.appAuthUserId 
	LEFT JOIN VW_MEMBERDATA h ON h.ssn= a.member_ssn
WHERE a.lodId = @refId

INSERT INTO @results (id, value) VALUES ('IO_RANK_NAME', @ioRankName);
--INSERT INTO @results (id, value) VALUES ('APPOINTING_AUTHORITY', @appAuthName);
INSERT INTO @results (id, value) VALUES ('IO_NAME', @ioName);
INSERT INTO @results (id, value) VALUES ('IO_RANK', @ioRank);
--INSERT INTO @results (id, value) VALUES ('IO_UNIT_NAME', @ioUnit);
--INSERT INTO @results (id, value) VALUES ('INCIDENT_TYPE', @incidentType);
INSERT INTO @results (id, value) VALUES ('MEMBER_NAME', @name);
INSERT INTO @results (id, value) VALUES ('MEMBER_RANK', @rank);
--INSERT INTO @results (id, value) VALUES ('MEMBER_UNIT_NAME', @unit);
--INSERT INTO @results (id, value) VALUES ('INV_SUSPENSE', @ioCompletionDate);
INSERT INTO @results (id, value) VALUES ('ADDITIONAL_INSTRUCTIONS', @appInstructionsToIo);
INSERT INTO @results (id, value) VALUES ('APPOINTING_POC', @appPOC);
INSERT INTO @results (id, value) VALUES ('APPOINTING_UNIT_NAME', @aaUnitName);
--INSERT INTO @results (id, value) VALUES ('APPOINTING_FNAME', @aaFname);
--INSERT INTO @results (id, value) VALUES ('APPOINTING_MI', @aaMname);
--INSERT INTO @results (id, value) VALUES ('APPOINTING_LNAME', @aaLname);
--INSERT INTO @results (id, value) VALUES ('APPOINTING_RANK', @aaRank);

SELECT id, value FROM @results

--SELECT datename(dw,c.io_completionDate) + ' ' + datename(dd,c.io_completionDate) + ' ' + datename(mm,c.io_completionDate) + ' ' + datename(yyyy,c.io_completionDate)
GO

