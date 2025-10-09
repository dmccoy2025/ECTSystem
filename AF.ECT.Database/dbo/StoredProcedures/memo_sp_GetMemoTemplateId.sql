/****** Script for SelectTopNRows command from SSMS  ******/
-- ============================================================================
-- Author:		Eric Kelley
-- Create date: Sep 10 2021
-- Description:	Get memo templateId using memo title
-- ============================================================================
CREATE PROCEDURE [dbo].[memo_sp_GetMemoTemplateId]
@title varchar(100)
AS
SELECT TOP (1000) [templateId]
  FROM [ALOD].[dbo].[core_MemoTemplates]
  where title = @title

  --[dbo].[memo_sp_GetMemoTemplateId] 'PSC Determination'
GO

