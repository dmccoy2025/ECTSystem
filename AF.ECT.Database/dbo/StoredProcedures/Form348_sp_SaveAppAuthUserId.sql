-- ============================================================================
-- Author:			Eric Kelley
-- Created Date:	10/26/2021
-- Description:		Saves the Appointing Authority userId on Form348 table
-- ============================================================================
CREATE Procedure [dbo].[Form348_sp_SaveAppAuthUserId]
@userId int,
@lodId int
As
update [ALOD].[dbo].[Form348]
Set appAuthUserId = @userId
where lodId = @lodId
GO

