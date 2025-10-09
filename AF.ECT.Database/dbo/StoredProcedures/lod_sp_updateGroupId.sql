
CREATE PROCEDURE [dbo].[lod_sp_updateGroupId] 
	@refId int,
	@groupId int

AS

UPDATE Form348
SET DOC_GROUP_ID = @groupId
WHERE LODID = @refId
GO

