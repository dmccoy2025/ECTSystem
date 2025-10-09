
CREATE PROCEDURE [dbo].[core_workflow_sp_RemoveSignature]
	@refId int,
	@moduleType int,
	@groupId tinyint

AS

UPDATE core_Signatures
SET deleted = 1
WHERE refId = @refId AND moduleType=@moduleType AND groupId = @groupId
GO

