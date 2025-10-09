
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 9/2/2016
-- Description:	Return all of the Member Category
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookUps_sp_MemberCategory]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT	mc.Member_Status_Desc AS [description], mc.Member_Status_ID AS [Id], mc.sort_order AS[sort_order]
	FROM	dbo.core_lkupMemberCategory mc
	ORDER	BY mc.sort_order ASC
END
GO

