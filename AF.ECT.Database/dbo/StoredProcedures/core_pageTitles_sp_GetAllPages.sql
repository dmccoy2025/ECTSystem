CREATE PROCEDURE [dbo].[core_pageTitles_sp_GetAllPages]

AS
	SET NOCOUNT ON;

SELECT     pageId, title
FROM         dbo.core_Pages
GO

