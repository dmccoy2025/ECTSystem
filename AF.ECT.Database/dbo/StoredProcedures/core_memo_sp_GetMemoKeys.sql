-- ============================================================================
-- Author:		Ken Barnett
-- Create date: 2/17/2015
-- Description:	Selects all of the memo related records from the 
--				core_KeyVal_Key table. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	6/26/2015
-- Work Item:		TFS Task 319
-- Description:		Added WHERE clause so that only memo related key values are
--					selected. Updated stored procedure description. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_memo_sp_GetMemoKeys]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT	k.ID AS KeyId, k.Description AS KeyDescription, kt.Id AS KeyTypeId, kt.Type_Name AS KeyTypeName
	FROM	core_KeyVal_Key k
			INNER JOIN core_KeyVal_KeyType kt ON k.Key_Type_Id = kt.Id
	WHERE	kt.Type_Name = 'MemoKey'
END
GO

