-- =============================================
-- Author:		Michael van Diest
-- Create date: 6/15/2020
-- Description:	get count of special cases that are not in holding queue
-- =============================================
CREATE PROCEDURE [dbo].[core_lod_sp_GetSpecialCaseNotHoldingCount] 
	@moduleId INT,
	@userId INT
AS
BEGIN
	DECLARE @results TABLE
	(
		requestId int,
		Protected_SSN varchar(4),
		Member_Name varchar(100),
		Unit_Name varchar(50),
		Case_Id varchar(50),
		status varchar(40),
		module varchar(50),
		Receive_Date datetime,
		Days int,
		Sub_Type varchar(10),
		lockId int,
		workflow_title varchar(50),
		pas_priority int,
		PriorityRank varchar(10),
		Reporting_Period date
	)

	Insert @Results(requestId, Protected_SSN, Member_name, Unit_Name, Case_Id, status, module, Receive_Date, Days, Sub_Type, lockId, workflow_title, pas_priority, PriorityRank, Reporting_Period)
	Exec core_lod_sp_GetSpecialCaseNotHolding @moduleId, @userId

	SELECT COUNT(*) From @results
END
GO

