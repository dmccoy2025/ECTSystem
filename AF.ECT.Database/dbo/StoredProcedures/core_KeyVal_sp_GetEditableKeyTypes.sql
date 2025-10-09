-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 8/11/2016
-- Description:	Selects all of the records from the core_KeyVal_KeyType table. 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_KeyVal_sp_GetEditableKeyTypes]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT	kt.ID AS KeyTypeId, kt.Type_Name AS KeyTypeName
	FROM	core_KeyVal_KeyType kt
	WHERE kt.Type_Name <> 'MemoKey' AND kt.Type_Name <> 'HelpKey'
END
GO

