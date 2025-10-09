-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 8/11/2016
-- Description:	Selects all of the records from the core_KeyVal_Key table given
--				the keyTypeId. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_KeyVal_sp_GetKeysUsingKeyType]
	@keyTypeID as integer
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT	k.ID AS KeyId, k.Description AS KeyDescription, kt.Id AS KeyTypeId, kt.Type_Name AS KeyTypeName
	FROM	core_KeyVal_Key k
		INNER JOIN core_KeyVal_KeyType kt ON k.Key_Type_Id = kt.Id
	WHERE k.Key_Type_Id = @keyTypeID
END
GO

