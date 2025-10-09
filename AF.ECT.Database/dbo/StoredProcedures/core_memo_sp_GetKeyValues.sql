-- =============================================
-- Author:		Ken Barnett
-- Create date: 2/17/2015
-- Description:	Selects values from the core_KeyVal_Value
--				that are associated with as specific
--				key value id from the core_KeyVal_Key
--				table. 
-- =============================================
CREATE PROCEDURE [dbo].[core_memo_sp_GetKeyValues]
	@keyId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT	Value_Id, Value_Description, Value
	FROM	core_KeyVal_Value
	WHERE	[Key_Id] = @keyId
END
GO

