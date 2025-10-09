
-- =======================================================
-- Created By:		Evan Morrison
-- Date Created:	10/3/2016
-- Description:		Given the LODid this function will return the workflow id
-- =======================================================


CREATE PROCEDURE [dbo].[core_lod_sp_GetWorkflow]
(
	@LODid int
)

As
	DECLARE @workflow As INT
	SELECT TOP 1 @workflow = f.workflow
	FROM dbo.Form348 as f
	WHERE f.lodId = @LODid
	select @workflow
GO

