
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
CREATE PROCEDURE [dbo].[core_lod_sp_GetReinvestigationsCount]
	@userId int,
	@sarc bit 
 

AS

--DECLARE @userId int
--Declare @sarc bit

--SET @userId = 295
--SET @sarc = 0

DECLARE @results TABLE
(
	requestId int,
	Protected_SSN varchar(9),
	Member_Name varchar(100),
	Unit_Name varchar(100),
	Case_Id varchar(50),
	status varchar(50),
	Receive_Date datetime,
	Days int
)

Insert @Results(requestId, Protected_SSN, Member_name, Unit_Name, Case_Id, status, Receive_Date, Days)
Exec core_lod_sp_GetReinvestigationRequests @userId, @sarc

Select COUNT(*) From @results
GO

