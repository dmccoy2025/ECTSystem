CREATE PROCEDURE [dbo].[core_pageTitles_sp_UpdateByPageId]
(
	@pageId as int,
	@title as varchar(50)
)

AS
	SET NOCOUNT ON;

UPDATE    dbo.core_Pages
SET              title = @title
WHERE     (pageId = @pageId)
GO

