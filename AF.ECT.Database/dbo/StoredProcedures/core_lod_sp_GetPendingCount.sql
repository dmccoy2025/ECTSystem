
-- exec core_lod_sp_GetPendingCount 12, 0
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	11/27/2015
-- Work Item:		TFS Task 289
-- Description:		Changed the size of caseId from 20 to 50.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	2/23/2017
-- Description:		Removed the ReceivedFrom field from being selected.
-- ============================================================================
-- Modified By:		Curt Lucas
-- Modified Date:	11/15/2019
-- Description:		Added pas_priority column
-- ============================================================================
-- Modified By:		Curt Lucas
-- Modified Date:	11/21/2019
-- Description:		Added PriorityRank column
-- ============================================================================
-- Modified By:		Michael van Diest
-- Modified Date:	8/13/2020
-- Description:		Added state column
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lod_sp_GetPendingCount]
	@userId int,
	@sarc bit 
AS
BEGIN
	DECLARE @viewType tinyint 
	SELECT @viewType = viewType FROM vw_users WHERE userID = @userId

	DECLARE @counts TABLE
	(
		actionName varchar(20),
		actionCount int
	)

	DECLARE @results TABLE
	(
		refId int,
		caseId varchar(50),
		name varchar(100),
		state varchar(10),
		ssn varchar(4),
		compo char(1),
		pascode char(4),
		workstatusid int,
		workstatus varchar(100),
		isfinal bit,
		workflowid int,
		workflow varchar(20),
		moduleid int,
		isformal bit,
		datecreated datetime,
		unitName varchar(100),
		receiveDate datetime,
		days int,
		lockId int,
		pas_priority int,
		PriorityRank varchar(3)
	)

	INSERT INTO @results (refId, caseId, name, state, ssn, compo, pascode,workstatusid,workstatus,isfinal,workflowid,workflow,moduleid,isformal,datecreated,unitName,receiveDate,days, lockId, pas_priority, PriorityRank)
	EXEC form348_sp_GroupSearch NULL,NULL, NULL, 0, @userId, @viewType, null, NULL, 2, NULL, NULL, @sarc 

	SELECT count(*) FROM @results
END

--[dbo].[core_lod_sp_GetPendingCount] 171, 1
GO

