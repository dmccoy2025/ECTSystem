-- ============================================================================
-- tSQLt Framework Setup and Test Runner
-- ============================================================================
-- This script sets up the tSQLt framework and runs all pagination tests
-- ============================================================================

USE [ALOD];
GO

PRINT '========================================';
PRINT 'tSQLt Test Framework Setup';
PRINT '========================================';
PRINT '';

-- Check if tSQLt is installed
IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'tSQLt')
BEGIN
    PRINT '✗ tSQLt framework is not installed!';
    PRINT '';
    PRINT 'To install tSQLt:';
    PRINT '1. Download from: https://tsqlt.org/downloads/';
    PRINT '2. Run the tSQLt.class.sql script in this database';
    PRINT '3. Execute: EXEC sp_configure ''clr enabled'', 1; RECONFIGURE;';
    PRINT '4. Execute: ALTER DATABASE [ALOD] SET TRUSTWORTHY ON;';
    PRINT '';
    RAISERROR('tSQLt is not installed. Please install it first.', 16, 1);
END
ELSE
BEGIN
    PRINT '✓ tSQLt framework is installed';
    
    -- Display tSQLt version
    DECLARE @Version NVARCHAR(100);
    SELECT @Version = CAST(SERVERPROPERTY('ProductVersion') AS NVARCHAR(100));
    PRINT '  SQL Server Version: ' + @Version;
    PRINT '';
END
GO

-- ============================================================================
-- Create Test Class
-- ============================================================================
PRINT 'Creating test class...';

IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'ApplicationWarmupProcess_sp_GetAllLogs_pagination_Tests')
BEGIN
    EXEC tSQLt.NewTestClass 'ApplicationWarmupProcess_sp_GetAllLogs_pagination_Tests';
    PRINT '✓ Test class created: ApplicationWarmupProcess_sp_GetAllLogs_pagination_Tests';
END
ELSE
BEGIN
    PRINT '✓ Test class already exists: ApplicationWarmupProcess_sp_GetAllLogs_pagination_Tests';
END
PRINT '';
GO

-- ============================================================================
-- Run Tests
-- ============================================================================
PRINT '========================================';
PRINT 'Running Pagination Tests';
PRINT '========================================';
PRINT '';

-- Run all tests in the class
EXEC tSQLt.Run 'ApplicationWarmupProcess_sp_GetAllLogs_pagination_Tests';
GO

-- ============================================================================
-- Display Results Summary
-- ============================================================================
PRINT '';
PRINT '========================================';
PRINT 'Test Results Summary';
PRINT '========================================';
PRINT '';

SELECT 
    Class,
    TestCase,
    Result,
    Msg
FROM tSQLt.TestResult
WHERE Class = 'ApplicationWarmupProcess_sp_GetAllLogs_pagination_Tests'
ORDER BY Result, TestCase;
GO

-- Display pass/fail counts
DECLARE @TotalTests INT, @PassedTests INT, @FailedTests INT;

SELECT 
    @TotalTests = COUNT(*),
    @PassedTests = SUM(CASE WHEN Result = 'Success' THEN 1 ELSE 0 END),
    @FailedTests = SUM(CASE WHEN Result = 'Failure' THEN 1 ELSE 0 END)
FROM tSQLt.TestResult
WHERE Class = 'ApplicationWarmupProcess_sp_GetAllLogs_pagination_Tests';

PRINT '';
PRINT 'Total Tests:  ' + CAST(@TotalTests AS NVARCHAR(10));
PRINT 'Passed:       ' + CAST(@PassedTests AS NVARCHAR(10)) + ' ✓';
PRINT 'Failed:       ' + CAST(@FailedTests AS NVARCHAR(10)) + CASE WHEN @FailedTests > 0 THEN ' ✗' ELSE '' END;
PRINT '';

IF @FailedTests = 0
BEGIN
    PRINT '========================================';
    PRINT '✓ ALL TESTS PASSED!';
    PRINT '========================================';
END
ELSE
BEGIN
    PRINT '========================================';
    PRINT '✗ SOME TESTS FAILED';
    PRINT '========================================';
    PRINT '';
    PRINT 'Failed tests:';
    SELECT TestCase, Msg
    FROM tSQLt.TestResult
    WHERE Class = 'ApplicationWarmupProcess_sp_GetAllLogs_pagination_Tests'
        AND Result = 'Failure';
END
PRINT '';
GO
