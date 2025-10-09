

CREATE PROCEDURE [dbo].[core_workstatus_sp_GetWorkStatusDescription]
	@workstatusId int

AS

Select Description 
From core_StatusCodes csc Inner Join core_WorkStatus cws
	On cws.statusId = csc.statusId
Where ws_id = @workstatusId
GO

