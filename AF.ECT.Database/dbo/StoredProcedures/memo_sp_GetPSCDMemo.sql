/****** Script for SelectTopNRows command from SSMS  ******/
-- ============================================================================
-- Author:		Eric Kelley
-- Create date: Sep 10 2021
-- Description:	Get memo templateId using refid	
-- ============================================================================
-- Modified By:		Eric Kelley
-- Modified Date:	Sep 21 2021
-- Description:		Returns non- deleted memo
-- ============================================================================
CREATE PROCEDURE [dbo].[memo_sp_GetPSCDMemo]
@refId Int
AS
SELECT  [memoId]
      ,[refId]
      ,[moduleId]
      ,[templateId]
      ,[deleted]
      ,[letterHead]
      ,[created_by]
      ,[created_date]
  FROM [ALOD].[dbo].[core_Memos2]
  where refId = @refId and deleted = 0

  --dbo.memo_sp_GetPSCDMemo 26070
GO

