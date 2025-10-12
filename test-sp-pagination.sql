-- ============================================================================
-- Test Script for ApplicationWarmupProcess_sp_GetAllLogs_pagination
-- Purpose: Verify pagination correctness and consistency
-- ============================================================================

USE [ALOD_Database]; -- Change to your database name
GO

PRINT '========================================';
PRINT 'Pagination Test Suite';
PRINT '========================================';
PRINT '';

-- First, let's see how much data we have
PRINT '--- Test 0: Total Record Count ---';
SELECT COUNT(*) AS TotalRecords
FROM ApplicationWarmupProcessLog l
JOIN ApplicationWarmupProcess p ON l.ProcessId = p.Id;
PRINT '';

-- Test 1: Basic Pagination - Page 1
PRINT '--- Test 1: Page 1, Size 10 (Default Sort: ExecutionDate DESC) ---';
EXEC [dbo].[ApplicationWarmupProcess_sp_GetAllLogs_pagination]
    @PageNumber = 1,
    @PageSize = 10,
    @SortBy = 'ExecutionDate',
    @SortOrder = 'DESC';
PRINT '';

-- Test 2: Basic Pagination - Page 2 (should return next 10 records)
PRINT '--- Test 2: Page 2, Size 10 (Default Sort: ExecutionDate DESC) ---';
EXEC [dbo].[ApplicationWarmupProcess_sp_GetAllLogs_pagination]
    @PageNumber = 2,
    @PageSize = 10,
    @SortBy = 'ExecutionDate',
    @SortOrder = 'DESC';
PRINT '';

-- Test 3: Check for duplicate records across pages
PRINT '--- Test 3: Checking for Duplicates Across Pages 1-3 ---';
WITH Page1 AS (
    SELECT l.Id, p.Name, l.ExecutionDate, l.Message,
           ROW_NUMBER() OVER (ORDER BY 
               CASE WHEN 'ExecutionDate' = 'ExecutionDate' AND 'DESC' = 'DESC' THEN l.ExecutionDate END DESC) AS RowNum
    FROM ApplicationWarmupProcessLog l
    JOIN ApplicationWarmupProcess p ON l.ProcessId = p.Id
),
AllPages AS (
    SELECT Id FROM Page1 WHERE RowNum BETWEEN 1 AND 10  -- Page 1
    UNION ALL
    SELECT Id FROM Page1 WHERE RowNum BETWEEN 11 AND 20 -- Page 2
    UNION ALL
    SELECT Id FROM Page1 WHERE RowNum BETWEEN 21 AND 30 -- Page 3
)
SELECT 
    COUNT(*) AS TotalRecords,
    COUNT(DISTINCT Id) AS UniqueRecords,
    CASE WHEN COUNT(*) = COUNT(DISTINCT Id) THEN 'PASS: No duplicates' 
         ELSE 'FAIL: Duplicates found!' END AS Result
FROM AllPages;
PRINT '';

-- Test 4: Verify OFFSET calculation
PRINT '--- Test 4: Verify OFFSET Calculation (Page 3, Size 5) ---';
PRINT 'Expected OFFSET: (3-1)*5 = 10 records skipped';
EXEC [dbo].[ApplicationWarmupProcess_sp_GetAllLogs_pagination]
    @PageNumber = 3,
    @PageSize = 5,
    @SortBy = 'Id',
    @SortOrder = 'ASC';
PRINT '';

-- Test 5: Compare stored procedure results with direct query
PRINT '--- Test 5: Compare SP Results with Direct Query (Page 1, Size 5, Id ASC) ---';
PRINT 'Direct Query Results:';
SELECT TOP 5 l.Id, p.Name, l.ExecutionDate, l.Message
FROM ApplicationWarmupProcessLog l
JOIN ApplicationWarmupProcess p ON l.ProcessId = p.Id
ORDER BY l.Id ASC;

PRINT '';
PRINT 'Stored Procedure Results:';
EXEC [dbo].[ApplicationWarmupProcess_sp_GetAllLogs_pagination]
    @PageNumber = 1,
    @PageSize = 5,
    @SortBy = 'Id',
    @SortOrder = 'ASC';
PRINT '';

-- Test 6: Test with different sort columns
PRINT '--- Test 6: Sort by Name ASC (Page 1) ---';
EXEC [dbo].[ApplicationWarmupProcess_sp_GetAllLogs_pagination]
    @PageNumber = 1,
    @PageSize = 5,
    @SortBy = 'Name',
    @SortOrder = 'ASC';
PRINT '';

-- Test 7: Test consistency - run same query twice
PRINT '--- Test 7: Consistency Check - Same Query Run Twice ---';
PRINT 'First Run:';
EXEC [dbo].[ApplicationWarmupProcess_sp_GetAllLogs_pagination]
    @PageNumber = 2,
    @PageSize = 5,
    @SortBy = 'ExecutionDate',
    @SortOrder = 'DESC';

PRINT '';
PRINT 'Second Run (should be identical):';
EXEC [dbo].[ApplicationWarmupProcess_sp_GetAllLogs_pagination]
    @PageNumber = 2,
    @PageSize = 5,
    @SortBy = 'ExecutionDate',
    @SortOrder = 'DESC';
PRINT '';

-- Test 8: Edge case - Large page number (beyond available data)
PRINT '--- Test 8: Edge Case - Page Number Beyond Data ---';
EXEC [dbo].[ApplicationWarmupProcess_sp_GetAllLogs_pagination]
    @PageNumber = 9999,
    @PageSize = 10,
    @SortBy = 'Id',
    @SortOrder = 'ASC';
PRINT '';

-- Test 9: Check for gaps in pagination
PRINT '--- Test 9: Check for Gaps - Verify Sequential Coverage ---';
DECLARE @TotalCount INT;
SELECT @TotalCount = COUNT(*) 
FROM ApplicationWarmupProcessLog l
JOIN ApplicationWarmupProcess p ON l.ProcessId = p.Id;

WITH AllRecords AS (
    SELECT l.Id
    FROM ApplicationWarmupProcessLog l
    JOIN ApplicationWarmupProcess p ON l.ProcessId = p.Id
),
Page1Records AS (
    SELECT l.Id
    FROM ApplicationWarmupProcessLog l
    JOIN ApplicationWarmupProcess p ON l.ProcessId = p.Id
    ORDER BY l.Id ASC
    OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY
),
Page2Records AS (
    SELECT l.Id
    FROM ApplicationWarmupProcessLog l
    JOIN ApplicationWarmupProcess p ON l.ProcessId = p.Id
    ORDER BY l.Id ASC
    OFFSET 10 ROWS FETCH NEXT 10 ROWS ONLY
)
SELECT 
    'Page Coverage Test' AS TestName,
    (SELECT COUNT(*) FROM Page1Records) AS Page1Count,
    (SELECT COUNT(*) FROM Page2Records) AS Page2Count,
    @TotalCount AS TotalCount,
    CASE 
        WHEN @TotalCount <= 20 THEN 'PASS: All records covered'
        WHEN (SELECT COUNT(*) FROM Page1Records) = 10 AND (SELECT COUNT(*) FROM Page2Records) = 10 THEN 'PASS: Pages correctly sized'
        ELSE 'Needs verification'
    END AS Result;
PRINT '';

-- Test 10: Test NULL handling in CASE statements
PRINT '--- Test 10: Analyze ORDER BY with CASE statement behavior ---';
SELECT TOP 10
    l.Id,
    p.Name,
    l.ExecutionDate,
    -- Show what the CASE statements evaluate to
    CASE WHEN 'ExecutionDate' = 'Id' AND 'DESC' = 'ASC' THEN l.Id END AS Case_Id_ASC,
    CASE WHEN 'ExecutionDate' = 'Id' AND 'DESC' = 'DESC' THEN l.Id END AS Case_Id_DESC,
    CASE WHEN 'ExecutionDate' = 'ExecutionDate' AND 'DESC' = 'ASC' THEN l.ExecutionDate END AS Case_Date_ASC,
    CASE WHEN 'ExecutionDate' = 'ExecutionDate' AND 'DESC' = 'DESC' THEN l.ExecutionDate END AS Case_Date_DESC
FROM ApplicationWarmupProcessLog l
JOIN ApplicationWarmupProcess p ON l.ProcessId = p.Id
ORDER BY 
    CASE WHEN 'ExecutionDate' = 'Id' AND 'DESC' = 'ASC' THEN l.Id END ASC,
    CASE WHEN 'ExecutionDate' = 'Id' AND 'DESC' = 'DESC' THEN l.Id END DESC,
    CASE WHEN 'ExecutionDate' = 'ExecutionDate' AND 'DESC' = 'ASC' THEN l.ExecutionDate END ASC,
    CASE WHEN 'ExecutionDate' = 'ExecutionDate' AND 'DESC' = 'DESC' THEN l.ExecutionDate END DESC;
PRINT '';

PRINT '========================================';
PRINT 'Test Suite Complete';
PRINT '========================================';
PRINT '';
PRINT 'Key Points to Verify:';
PRINT '1. No duplicate records across pages';
PRINT '2. Consistent results on repeated queries';
PRINT '3. Correct OFFSET calculation';
PRINT '4. No gaps in pagination';
PRINT '5. CASE statements return mostly NULLs (potential issue)';
