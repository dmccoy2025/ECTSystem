-- ======================================================
-- Author:		Eric Kelley
-- Create date: 7/1/2021
-- Description:	Set post_processing_complete using lodId
-- ======================================================
Create Procedure [dbo].[core_lod_sp_SetLODPostCompletion]
@lodId int
AS
UPDATE [dbo].[Form348]
   SET [is_post_processing_complete] = 1
 WHERE lodId = @lodId

-- [dbo].[core_lod_sp_SetLODPostCompletion] 31381
GO

