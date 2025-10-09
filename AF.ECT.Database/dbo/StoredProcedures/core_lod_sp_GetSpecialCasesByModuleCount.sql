-- Exec core_lod_sp_GetSpecialCasesByModuleCount 254, 11   -- Med Tech for MEB
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	5/6/2015
-- Work Item:		TFS User Story 120
-- Description:		Modified the store procedure to pass in two more null 
--					values for the call to form348_sc_sp_Search. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	11/27/2015
-- Work Item:		TFS Task 289
-- Description:		Changed the size of Case_Id from 20 to 50.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	4/29/2016
-- Description:		Added the PH reporting period as one of the fields selected
--					and stored in the result tables.
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	5/18/2016
-- Description:		Added the PH reporting period as one of the fields selected
--					and stored in the result tables.
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	4/3/2017
-- Description:		Add workflow_title to results table
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	8/8/2017
-- Description:		Increased the @results.status field size from 40 to 100.
-- ============================================================================
-- Modified By:		Austin Lorans
-- Modified Date:	11/01/2019
-- Description:		Added compo to results table
-- ============================================================================
-- Modified By:		Curt Lucas
-- Modified Date:	11/15/2019
-- Description:		Added column for 'pas_priority'
-- ============================================================================
-- Modified By:		Curt Lucas
-- Modified Date:	11/25/2019
-- Description:		Added column 'PriorityRank'
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lod_sp_GetSpecialCasesByModuleCount]
	@userId int,
	@moduleId int
AS
BEGIN
	DECLARE @groupId int, @roleId int

	SELECT @roleId = currentRole FROM core_Users WHERE userID = @userId
	SELECT @groupId = groupId FROM core_UserRoles WHERE userRoleID = @roleId

	DECLARE @results TABLE
	(
		requestId int,
		Compo int,
		Protected_SSN varchar(4),
		Member_Name varchar(100),
		Unit_Name varchar(50),
		Case_Id varchar(50),
		status varchar(100),
		module varchar(50),
		Receive_Date datetime,
		Days int,
		Sub_Type int,
		Reporting_Period NVARCHAR(75),
		lockId INT,
		workflow_title VARCHAR(100),
		pas_priority int,
		PriorityRank VARCHAR(3)
	)

	DECLARE @results2 TABLE
	(
		refId int,
		CaseId varchar(50),
		Name varchar(100),
		SSN varchar(4),
		Compo int,
		PasCode varchar(8),
		WorkStatusId int,
		WorkStatus varchar(100),
		IsFinal int,
		WorkflowId int,
		Workflow varchar(100),
		ModuleId int,
		DateCreated datetime,
		UnitName varchar(100),
		ReceiveDate datetime,
		Days int,
		lockId int,
		PrintId varchar(20),
		Sub_Type int,
		Reporting_Period NVARCHAR(75)
	)

	DECLARE @Compo int
	SET @Compo = dbo.GetCompoForUser(@userId)


	If (@moduleId = 11 And @groupId = 3)
	BEGIN
		Insert @Results2(refId, CaseId, Name, SSN, Compo, PasCode, WorkStatusId, WorkStatus, IsFinal, WorkflowId, Workflow, ModuleId
			, DateCreated, UnitName, ReceiveDate, Days, lockId, PrintId, Sub_Type, Reporting_Period)
		Exec form348_sc_sp_Search null,null,null,null,null,0,@userId,4,@Compo,0,@moduleId,0,false,null

		Select COUNT(*) From @results2
		Where IsFinal = 0
	END
	ELSE
	BEGIN
		Insert @Results(requestId, Protected_SSN, Member_name, Compo, Unit_Name, Case_Id, status, module, Receive_Date, Days, Sub_Type, Reporting_Period, lockId, workflow_title, pas_priority, PriorityRank)
		Exec core_lod_sp_GetSpecialCasesByModuleId @userId, @moduleId

		Select COUNT(*) From @results
	END
END
GO

