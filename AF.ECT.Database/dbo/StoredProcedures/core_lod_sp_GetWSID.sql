-- =============================================
-- Author:		Eric Kelley
-- Create date: 5/25/2021
-- Description:	Uses LODID to get WsId
-- =============================================
-- Modified By:		Eric Kelley
-- Modified Date:	07/15/2021
-- Description:		Uses LODID, SC_Id, to get WsId
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lod_sp_GetWSID]
@iD Int
As
DECLARE 
@wsId int = 0

If ((SELECT status FROM Form348_SC WHERE SC_Id = @iD ) is not NULL)
BEGIN
SET @wsId = 
	(SELECT status
	FROM Form348_SC
	WHERE SC_Id = @iD) 
END
ELSE if ((SELECT status FROM Form348 where lodid = @iD) is not Null)
BEGIN
SET @wsId = 
	(SELECT status
	FROM Form348
	where lodid = @iD)
END
Else if ((Select status from Form348_SARC where sarc_id = @iD) is not null)
BEGIN
SET @wsId = 
	(Select status 
	from Form348_SARC
	where sarc_id = @iD)
END
Else if ((Select status from Form348_RR where InitialLodId = @iD) is not null)
BEGIN
SET @wsId = 
	(Select status 
	from Form348_RR
	where InitialLodId = @iD)
END
Else if ((Select status from Form348_AP where initial_lod_id = @iD) is not null)
BEGIN
SET @wsId = 
	(Select status 
	from Form348_AP
	where initial_lod_id = @iD)
END
Else if ((Select status from Form348_AP_SARC where initial_id = @iD) is not null)
BEGIN
SET @wsId = 
	(Select status 
	from Form348_AP_SARC
	where initial_id = @iD)
END


Select @wsId

--[dbo].[core_lod_sp_GetWSID] 31396
GO

