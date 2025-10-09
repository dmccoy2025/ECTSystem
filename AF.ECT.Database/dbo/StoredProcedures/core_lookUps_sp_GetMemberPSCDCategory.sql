-- ================================================================================
-- Author:		Eric Kelley
-- Create date: 17 Sep 2021
-- Description:	Returns all of the Member Category options for the PSCD Workflow
-- ===============================================================================
CREATE PROCEDURE [dbo].[core_lookUps_sp_GetMemberPSCDCategory]
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT	serviceID, service
	FROM	dbo.core_lkupMilitaryServices 
	
END

--[dbo].[core_lookUps_sp_GetMemberPSCDCategory]
GO

