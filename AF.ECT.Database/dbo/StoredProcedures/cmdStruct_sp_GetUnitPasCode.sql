-- =============================================
-- Author:		Kenneth Barnett
-- Create date: 7/16/2014
-- Description:	Returns the PAS code of the Unit whose ID and name were passed into the stored procedure. 
-- =============================================
CREATE PROCEDURE [dbo].[cmdStruct_sp_GetUnitPasCode] 
(
	  @unitId int
	, @unitName as varchar(100)
)

AS
BEGIN

	SET NOCOUNT ON;

	-- Select the unit pas using the unit id and name
	SELECT	PAS_CODE
	FROM	Command_Struct 
	WHERE	CS_ID = @unitId
			AND LONG_NAME = @unitName

END
GO

