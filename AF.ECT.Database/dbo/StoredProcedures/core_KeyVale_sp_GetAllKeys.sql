-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 6/26/2015
-- Description:	Selects all of the records from the core_KeyVal_Key table. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_KeyVale_sp_GetAllKeys]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT	k.ID AS KeyId, k.Description AS KeyDescription, kt.Id AS KeyTypeId, kt.Type_Name AS KeyTypeName
	FROM	core_KeyVal_Key k
			INNER JOIN core_KeyVal_KeyType kt ON k.Key_Type_Id = kt.Id
END
GO

