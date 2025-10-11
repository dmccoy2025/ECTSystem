CREATE PROCEDURE [dbo].[core_pageTitles_sp_GetAllPages_pagination]
	@PageNumber INT = 1,
	@PageSize INT = 10
AS
	SET NOCOUNT ON;

SELECT     pageId, title
FROM         dbo.core_Pages
ORDER BY pageId
OFFSET (@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
GO