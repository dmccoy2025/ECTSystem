
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 9/2/2016
-- Description:	Update a Influence in the influence table given the id
-- ============================================================================
CREATE PROCEDURE [dbo].[core_lookUps_sp_UpdateInfluence]
	@id INT,
	@description NVARCHAR(100),
	@type NVARCHAR(100),
	@sort_order INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE core_lkupMemberInfluence
	SET influence_description = @description, sort_order = @sort_order
	WHERE influence_id = @id

END
GO

