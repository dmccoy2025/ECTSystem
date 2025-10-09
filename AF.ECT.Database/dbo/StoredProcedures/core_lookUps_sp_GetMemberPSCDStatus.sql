-- ================================================================================
-- Author:		Eric Kelley
-- Create date: 21 Sep 2021
-- Description:	Returns all of the Member Status options for the PSCD Workflow
-- no longer using [dbo].[core_lookUps_sp_GetMemberPSCDCategory] 
-- ===============================================================================
CREATE PROCEDURE [dbo].[core_lookUps_sp_GetMemberPSCDStatus]
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT	serviceID, service
	FROM	dbo.core_lkupMilitaryServices 
	
END

--[dbo].[core_lookUps_sp_GetMemberPSCDStatus]
GO

