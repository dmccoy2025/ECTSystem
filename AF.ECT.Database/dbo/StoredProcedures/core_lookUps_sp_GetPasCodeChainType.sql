

CREATE PROCEDURE [dbo].[core_lookUps_sp_GetPasCodeChainType]

AS

	SET NOCOUNT ON;


SELECT     id as Value, description as  Name
FROM         core_lkupChainType
WHERE     (active = 1)
GO

