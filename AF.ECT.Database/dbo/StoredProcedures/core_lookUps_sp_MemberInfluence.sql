
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 9/7/2016
-- Description:	Return all of the Member Influence
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookUps_sp_MemberInfluence]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT	mi.influence_id AS [id], mi.influence_description AS [description], mi.sort_order AS [sort_order]
	FROM	dbo.core_lkupMemberInfluence mi
	ORDER	BY mi.sort_order ASC
END
GO

