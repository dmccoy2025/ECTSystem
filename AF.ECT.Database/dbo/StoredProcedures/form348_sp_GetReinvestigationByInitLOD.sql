--exec form348_sp_GetReinvestigationByInitLOD 1

CREATE PROCEDURE [dbo].[form348_sp_GetReinvestigationByInitLOD]
		 @LodId int 

AS

If Exists(Select 1 From Form348_RR Where InitialLodId = @LodId)
BEGIN
	Select InitialLodId, ReinvestigationLodId, CreatedBy, CreatedDate
	From Form348_RR
	Where InitialLodId = @LodId
END
GO

