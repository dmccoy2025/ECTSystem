--exec form348_sp_GetReinvestigation 1

CREATE PROCEDURE [dbo].[form348_sp_GetReinvestigation]
		 @LodId int 

AS

If Exists(Select 1 From Form348_RR Where ReinvestigationLodId = @LodId)
BEGIN
	Select InitialLodId, ReinvestigationLodId, CreatedBy, CreatedDate
	From Form348_RR
	Where ReinvestigationLodId = @LodId
END
GO

