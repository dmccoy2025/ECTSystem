-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 7/1/2015
-- Description:	Selects values from the core_KeyVal_Value table that are 
--				associated with as specific key value key description from the 
--				core_KeyVal_Key table. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_KeyVal_sp_GetKeyValuesByKeyDescription]
	@keyDesc VARCHAR(200)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT	v.Id, v.Value_Id AS ValueId, v.Value_Description AS ValueDescription, v.Value,
			k.ID AS KeyId, k.Description AS KeyDescription, 
			kt.Id AS KeyTypeId, kt.Type_Name AS KeyTypeName
	FROM	core_KeyVal_Value v
			INNER JOIN core_KeyVal_Key k ON v.Key_Id = k.ID
			INNER JOIN core_KeyVal_KeyType kt ON k.Key_Type_Id = kt.Id
	WHERE	k.Description = @keyDesc
END
GO

