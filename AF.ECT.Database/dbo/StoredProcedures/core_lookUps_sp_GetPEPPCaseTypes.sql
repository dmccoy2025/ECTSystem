-- =============================================
-- Author:		Kenneth Barnett
-- Create date: 6/16/2014
-- Description:	Returns the records in the core_lkupPEPPCaseType table
-- =============================================
CREATE PROCEDURE [dbo].[core_lookUps_sp_GetPEPPCaseTypes]  

AS
BEGIN
	SET NOCOUNT ON;

    SELECT		casetypeID AS Value, caseTypeName AS Name, active
    FROM		core_lkupPEPPCaseType
    WHERE		active = 1
    ORDER BY	caseTypeName
END
GO

