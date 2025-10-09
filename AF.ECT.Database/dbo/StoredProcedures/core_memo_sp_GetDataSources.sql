
CREATE PROCEDURE [dbo].[core_memo_sp_GetDataSources]

AS

SELECT [name]
FROM sys.procedures
WHERE name LIKE 'memo_sp_%'
ORDER BY [name]
GO

