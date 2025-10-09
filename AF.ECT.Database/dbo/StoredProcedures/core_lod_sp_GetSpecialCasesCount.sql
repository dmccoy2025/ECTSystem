
-- ============================================================================
-- Author:		?
-- Create date: ?
-- Description:	
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	11/27/2015
-- Work Item:		TFS Task 289
-- Description:		Changed the size of Case_Id from 20 to 50.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lod_sp_GetSpecialCasesCount]
	@userId int
 

AS

--DECLARE @userId int

--SET @userId = 295

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
	Days int
)

Insert @Results(requestId, Protected_SSN, Member_name, Unit_Name, Case_Id, status, module, Receive_Date, Days)
Exec core_lod_sp_GetSpecialCases @userId

Select COUNT(*) From @results
GO

