
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 9/7/2016
-- Description:	Return all of the Member Status
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookUps_sp_MemberStatus]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT	ms.memberType AS [Type], ms.id, ms.memberDescr AS [Description], id AS [id], sort_order AS [sort_order]
	FROM	dbo.core_lkupMemberStatus ms
	ORDER By sort_order ASC
END
GO

