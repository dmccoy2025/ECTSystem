-- =============================================
-- Author:		Ken Barnett
-- Create date: 2/17/2015
-- Description:	Updates the value field in the
--				core_KeyVal_Value table for a specific
--				key id and value id to a new value. 
-- =============================================
CREATE PROCEDURE [dbo].[core_memo_sp_UpdateKeyValue]
	 @keyId INT
	,@valueId INT
	,@newValueDescription VARCHAR(50)
	,@newValue VARCHAR(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE	core_KeyVal_Value
	SET		Value = @newValue, Value_Description = @newValueDescription
	WHERE	[Key_Id] = @keyId
			AND Value_Id = @valueId
END
GO

