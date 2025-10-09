

CREATE PROCEDURE [dbo].[core_workflow_sp_AddSignature] 
	@refId int,
	@moduleType int,
	@userId int,
	@actionId int,
	@groupId tinyint,
	@statusIn tinyint,
	@statusOut tinyint

AS

DECLARE @personId int
EXEC core_personnel_sp_CreatePersonnelFromUser @userId, @personId OUT

INSERT INTO core_Signatures (refId,moduleType, personId, actionId, groupId, statusIn, statusOut) 
VALUES (@refId,@moduleType, @personId, @actionId, @groupId, @statusIn, @statusOut)
GO

