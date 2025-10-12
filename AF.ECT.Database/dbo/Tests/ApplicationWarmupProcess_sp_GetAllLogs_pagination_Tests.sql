-- ============================================================================
-- Redgate SQL Test (tSQLt) Framework Tests
-- Test Class: ApplicationWarmupProcess_sp_GetAllLogs_pagination_Tests
-- Purpose: Comprehensive testing of pagination stored procedure
-- ============================================================================
-- Prerequisites: 
--   1. Install tSQLt framework: https://tsqlt.org/
--   2. Run: EXEC tSQLt.NewTestClass 'ApplicationWarmupProcess_sp_GetAllLogs_pagination_Tests';
-- ============================================================================

GO

-- Create test class if it doesn't exist
IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'ApplicationWarmupProcess_sp_GetAllLogs_pagination_Tests')
BEGIN
    EXEC tSQLt.NewTestClass 'ApplicationWarmupProcess_sp_GetAllLogs_pagination_Tests';
END
GO

-- ============================================================================
-- Test 1: Verify default pagination parameters work correctly
-- ============================================================================
CREATE OR ALTER PROCEDURE ApplicationWarmupProcess_sp_GetAllLogs_pagination_Tests.[test that default pagination parameters return first 10 records]
AS
BEGIN
    -- Arrange
    EXEC tSQLt.FakeTable 'dbo.ApplicationWarmupProcess';
    EXEC tSQLt.FakeTable 'dbo.ApplicationWarmupProcessLog';
    
    -- Insert test process
    INSERT INTO dbo.ApplicationWarmupProcess (Id, Name, IsActive)
    VALUES (1, 'TestProcess', 1);
    
    -- Insert 25 test log records
    DECLARE @i INT = 1;
    WHILE @i <= 25
    BEGIN
        INSERT INTO dbo.ApplicationWarmupProcessLog (Id, ProcessId, ExecutionDate, Message)
        VALUES (@i, 1, DATEADD(MINUTE, -@i, GETDATE()), 'Test message ' + CAST(@i AS NVARCHAR(10)));
        SET @i = @i + 1;
    END
    
    -- Act
    DECLARE @TotalCount INT;
    DECLARE @ActualCount INT;
    
    CREATE TABLE #Results (Id INT, Name NVARCHAR(255), ExecutionDate DATETIME, Message NVARCHAR(MAX));
    
    INSERT INTO #Results
    EXEC dbo.ApplicationWarmupProcess_sp_GetAllLogs_pagination
        @PageNumber = 1,
        @PageSize = 10,
        @SortBy = 'ExecutionDate',
        @SortOrder = 'DESC';
    
    SELECT @ActualCount = COUNT(*) FROM #Results;
    
    -- Assert
    EXEC tSQLt.AssertEquals @Expected = 10, @Actual = @ActualCount;
    
    DROP TABLE #Results;
END
GO

-- ============================================================================
-- Test 2: Verify pagination calculates OFFSET correctly
-- ============================================================================
CREATE OR ALTER PROCEDURE ApplicationWarmupProcess_sp_GetAllLogs_pagination_Tests.[test that page 2 skips first page records]
AS
BEGIN
    -- Arrange
    EXEC tSQLt.FakeTable 'dbo.ApplicationWarmupProcess';
    EXEC tSQLt.FakeTable 'dbo.ApplicationWarmupProcessLog';
    
    INSERT INTO dbo.ApplicationWarmupProcess (Id, Name, IsActive)
    VALUES (1, 'TestProcess', 1);
    
    -- Insert 30 records with predictable IDs
    DECLARE @i INT = 1;
    WHILE @i <= 30
    BEGIN
        INSERT INTO dbo.ApplicationWarmupProcessLog (Id, ProcessId, ExecutionDate, Message)
        VALUES (@i, 1, DATEADD(MINUTE, -@i, GETDATE()), 'Message ' + CAST(@i AS NVARCHAR(10)));
        SET @i = @i + 1;
    END
    
    -- Act - Get Page 1
    CREATE TABLE #Page1 (Id INT, Name NVARCHAR(255), ExecutionDate DATETIME, Message NVARCHAR(MAX));
    INSERT INTO #Page1
    EXEC dbo.ApplicationWarmupProcess_sp_GetAllLogs_pagination
        @PageNumber = 1,
        @PageSize = 10,
        @SortBy = 'Id',
        @SortOrder = 'ASC';
    
    -- Act - Get Page 2
    CREATE TABLE #Page2 (Id INT, Name NVARCHAR(255), ExecutionDate DATETIME, Message NVARCHAR(MAX));
    INSERT INTO #Page2
    EXEC dbo.ApplicationWarmupProcess_sp_GetAllLogs_pagination
        @PageNumber = 2,
        @PageSize = 10,
        @SortBy = 'Id',
        @SortOrder = 'ASC';
    
    -- Assert - Page 2 should start at ID 11
    DECLARE @FirstIdPage2 INT;
    SELECT TOP 1 @FirstIdPage2 = Id FROM #Page2 ORDER BY Id ASC;
    
    EXEC tSQLt.AssertEquals @Expected = 11, @Actual = @FirstIdPage2;
    
    -- Assert - No overlap between pages
    DECLARE @OverlapCount INT;
    SELECT @OverlapCount = COUNT(*)
    FROM #Page1 p1
    INNER JOIN #Page2 p2 ON p1.Id = p2.Id;
    
    EXEC tSQLt.AssertEquals @Expected = 0, @Actual = @OverlapCount, @Message = 'Pages should not contain duplicate records';
    
    DROP TABLE #Page1;
    DROP TABLE #Page2;
END
GO

-- ============================================================================
-- Test 3: Verify process name filter works
-- ============================================================================
CREATE OR ALTER PROCEDURE ApplicationWarmupProcess_sp_GetAllLogs_pagination_Tests.[test that process name filter returns only matching records]
AS
BEGIN
    -- Arrange
    EXEC tSQLt.FakeTable 'dbo.ApplicationWarmupProcess';
    EXEC tSQLt.FakeTable 'dbo.ApplicationWarmupProcessLog';
    
    INSERT INTO dbo.ApplicationWarmupProcess (Id, Name, IsActive)
    VALUES 
        (1, 'ProcessA', 1),
        (2, 'ProcessB', 1),
        (3, 'ProcessC', 1);
    
    INSERT INTO dbo.ApplicationWarmupProcessLog (Id, ProcessId, ExecutionDate, Message)
    VALUES 
        (1, 1, GETDATE(), 'Message from A'),
        (2, 2, GETDATE(), 'Message from B'),
        (3, 3, GETDATE(), 'Message from C'),
        (4, 1, GETDATE(), 'Another from A'),
        (5, 2, GETDATE(), 'Another from B');
    
    -- Act
    CREATE TABLE #Filtered (Id INT, Name NVARCHAR(255), ExecutionDate DATETIME, Message NVARCHAR(MAX));
    INSERT INTO #Filtered
    EXEC dbo.ApplicationWarmupProcess_sp_GetAllLogs_pagination
        @PageNumber = 1,
        @PageSize = 10,
        @ProcessName = 'ProcessA',
        @SortBy = 'Id',
        @SortOrder = 'ASC';
    
    -- Assert
    DECLARE @ActualCount INT;
    SELECT @ActualCount = COUNT(*) FROM #Filtered;
    
    EXEC tSQLt.AssertEquals @Expected = 2, @Actual = @ActualCount, @Message = 'Should return only ProcessA records';
    
    -- Verify all records are from ProcessA
    DECLARE @NonMatchingCount INT;
    SELECT @NonMatchingCount = COUNT(*) FROM #Filtered WHERE Name <> 'ProcessA';
    
    EXEC tSQLt.AssertEquals @Expected = 0, @Actual = @NonMatchingCount, @Message = 'All records should be from ProcessA';
    
    DROP TABLE #Filtered;
END
GO

-- ============================================================================
-- Test 4: Verify date range filter works
-- ============================================================================
CREATE OR ALTER PROCEDURE ApplicationWarmupProcess_sp_GetAllLogs_pagination_Tests.[test that date range filter returns only records within range]
AS
BEGIN
    -- Arrange
    EXEC tSQLt.FakeTable 'dbo.ApplicationWarmupProcess';
    EXEC tSQLt.FakeTable 'dbo.ApplicationWarmupProcessLog';
    
    INSERT INTO dbo.ApplicationWarmupProcess (Id, Name, IsActive)
    VALUES (1, 'TestProcess', 1);
    
    DECLARE @StartDate DATETIME = '2025-10-01';
    DECLARE @EndDate DATETIME = '2025-10-10';
    
    INSERT INTO dbo.ApplicationWarmupProcessLog (Id, ProcessId, ExecutionDate, Message)
    VALUES 
        (1, 1, '2025-09-30', 'Before range'),
        (2, 1, '2025-10-05', 'In range'),
        (3, 1, '2025-10-08', 'In range'),
        (4, 1, '2025-10-11', 'After range');
    
    -- Act
    CREATE TABLE #Filtered (Id INT, Name NVARCHAR(255), ExecutionDate DATETIME, Message NVARCHAR(MAX));
    INSERT INTO #Filtered
    EXEC dbo.ApplicationWarmupProcess_sp_GetAllLogs_pagination
        @PageNumber = 1,
        @PageSize = 10,
        @StartDate = @StartDate,
        @EndDate = @EndDate,
        @SortBy = 'Id',
        @SortOrder = 'ASC';
    
    -- Assert
    DECLARE @ActualCount INT;
    SELECT @ActualCount = COUNT(*) FROM #Filtered;
    
    EXEC tSQLt.AssertEquals @Expected = 2, @Actual = @ActualCount, @Message = 'Should return only records within date range';
    
    DROP TABLE #Filtered;
END
GO

-- ============================================================================
-- Test 5: Verify message filter works with partial match
-- ============================================================================
CREATE OR ALTER PROCEDURE ApplicationWarmupProcess_sp_GetAllLogs_pagination_Tests.[test that message filter performs partial match]
AS
BEGIN
    -- Arrange
    EXEC tSQLt.FakeTable 'dbo.ApplicationWarmupProcess';
    EXEC tSQLt.FakeTable 'dbo.ApplicationWarmupProcessLog';
    
    INSERT INTO dbo.ApplicationWarmupProcess (Id, Name, IsActive)
    VALUES (1, 'TestProcess', 1);
    
    INSERT INTO dbo.ApplicationWarmupProcessLog (Id, ProcessId, ExecutionDate, Message)
    VALUES 
        (1, 1, GETDATE(), 'Error: Something failed'),
        (2, 1, GETDATE(), 'Success: Operation completed'),
        (3, 1, GETDATE(), 'Error: Another failure'),
        (4, 1, GETDATE(), 'Warning: Check this');
    
    -- Act
    CREATE TABLE #Filtered (Id INT, Name NVARCHAR(255), ExecutionDate DATETIME, Message NVARCHAR(MAX));
    INSERT INTO #Filtered
    EXEC dbo.ApplicationWarmupProcess_sp_GetAllLogs_pagination
        @PageNumber = 1,
        @PageSize = 10,
        @MessageFilter = 'Error',
        @SortBy = 'Id',
        @SortOrder = 'ASC';
    
    -- Assert
    DECLARE @ActualCount INT;
    SELECT @ActualCount = COUNT(*) FROM #Filtered;
    
    EXEC tSQLt.AssertEquals @Expected = 2, @Actual = @ActualCount, @Message = 'Should return only error messages';
    
    DROP TABLE #Filtered;
END
GO

-- ============================================================================
-- Test 6: Verify sorting by different columns works
-- ============================================================================
CREATE OR ALTER PROCEDURE ApplicationWarmupProcess_sp_GetAllLogs_pagination_Tests.[test that sorting by Id ASC works correctly]
AS
BEGIN
    -- Arrange
    EXEC tSQLt.FakeTable 'dbo.ApplicationWarmupProcess';
    EXEC tSQLt.FakeTable 'dbo.ApplicationWarmupProcessLog';
    
    INSERT INTO dbo.ApplicationWarmupProcess (Id, Name, IsActive)
    VALUES (1, 'TestProcess', 1);
    
    INSERT INTO dbo.ApplicationWarmupProcessLog (Id, ProcessId, ExecutionDate, Message)
    VALUES 
        (5, 1, GETDATE(), 'Message 5'),
        (2, 1, GETDATE(), 'Message 2'),
        (8, 1, GETDATE(), 'Message 8'),
        (1, 1, GETDATE(), 'Message 1');
    
    -- Act
    CREATE TABLE #Results (Id INT, Name NVARCHAR(255), ExecutionDate DATETIME, Message NVARCHAR(MAX));
    INSERT INTO #Results
    EXEC dbo.ApplicationWarmupProcess_sp_GetAllLogs_pagination
        @PageNumber = 1,
        @PageSize = 10,
        @SortBy = 'Id',
        @SortOrder = 'ASC';
    
    -- Assert
    DECLARE @FirstId INT, @LastId INT;
    SELECT TOP 1 @FirstId = Id FROM #Results ORDER BY Id ASC;
    SELECT TOP 1 @LastId = Id FROM #Results ORDER BY Id DESC;
    
    EXEC tSQLt.AssertEquals @Expected = 1, @Actual = @FirstId, @Message = 'First record should have Id = 1';
    EXEC tSQLt.AssertEquals @Expected = 8, @Actual = @LastId, @Message = 'Last record should have Id = 8';
    
    DROP TABLE #Results;
END
GO

-- ============================================================================
-- Test 7: Verify invalid sort parameter raises error
-- ============================================================================
CREATE OR ALTER PROCEDURE ApplicationWarmupProcess_sp_GetAllLogs_pagination_Tests.[test that invalid sort column raises error]
AS
BEGIN
    -- Arrange
    EXEC tSQLt.FakeTable 'dbo.ApplicationWarmupProcess';
    EXEC tSQLt.FakeTable 'dbo.ApplicationWarmupProcessLog';
    
    INSERT INTO dbo.ApplicationWarmupProcess (Id, Name, IsActive)
    VALUES (1, 'TestProcess', 1);
    
    INSERT INTO dbo.ApplicationWarmupProcessLog (Id, ProcessId, ExecutionDate, Message)
    VALUES (1, 1, GETDATE(), 'Test message');
    
    -- Act & Assert
    EXEC tSQLt.ExpectException 
        @ExpectedMessage = 'Invalid @SortBy parameter%',
        @ExpectedSeverity = 16,
        @ExpectedState = NULL;
    
    EXEC dbo.ApplicationWarmupProcess_sp_GetAllLogs_pagination
        @PageNumber = 1,
        @PageSize = 10,
        @SortBy = 'InvalidColumn',
        @SortOrder = 'ASC';
END
GO

-- ============================================================================
-- Test 8: Verify invalid sort order raises error
-- ============================================================================
CREATE OR ALTER PROCEDURE ApplicationWarmupProcess_sp_GetAllLogs_pagination_Tests.[test that invalid sort order raises error]
AS
BEGIN
    -- Arrange
    EXEC tSQLt.FakeTable 'dbo.ApplicationWarmupProcess';
    EXEC tSQLt.FakeTable 'dbo.ApplicationWarmupProcessLog';
    
    INSERT INTO dbo.ApplicationWarmupProcess (Id, Name, IsActive)
    VALUES (1, 'TestProcess', 1);
    
    INSERT INTO dbo.ApplicationWarmupProcessLog (Id, ProcessId, ExecutionDate, Message)
    VALUES (1, 1, GETDATE(), 'Test message');
    
    -- Act & Assert
    EXEC tSQLt.ExpectException 
        @ExpectedMessage = 'Invalid @SortOrder parameter%',
        @ExpectedSeverity = 16,
        @ExpectedState = NULL;
    
    EXEC dbo.ApplicationWarmupProcess_sp_GetAllLogs_pagination
        @PageNumber = 1,
        @PageSize = 10,
        @SortBy = 'Id',
        @SortOrder = 'INVALID';
END
GO

-- ============================================================================
-- Test 9: Verify empty result set when no data matches
-- ============================================================================
CREATE OR ALTER PROCEDURE ApplicationWarmupProcess_sp_GetAllLogs_pagination_Tests.[test that no matching data returns empty result]
AS
BEGIN
    -- Arrange
    EXEC tSQLt.FakeTable 'dbo.ApplicationWarmupProcess';
    EXEC tSQLt.FakeTable 'dbo.ApplicationWarmupProcessLog';
    
    INSERT INTO dbo.ApplicationWarmupProcess (Id, Name, IsActive)
    VALUES (1, 'TestProcess', 1);
    
    INSERT INTO dbo.ApplicationWarmupProcessLog (Id, ProcessId, ExecutionDate, Message)
    VALUES (1, 1, GETDATE(), 'Test message');
    
    -- Act
    CREATE TABLE #Results (Id INT, Name NVARCHAR(255), ExecutionDate DATETIME, Message NVARCHAR(MAX));
    INSERT INTO #Results
    EXEC dbo.ApplicationWarmupProcess_sp_GetAllLogs_pagination
        @PageNumber = 1,
        @PageSize = 10,
        @ProcessName = 'NonExistentProcess',
        @SortBy = 'Id',
        @SortOrder = 'ASC';
    
    -- Assert
    DECLARE @ActualCount INT;
    SELECT @ActualCount = COUNT(*) FROM #Results;
    
    EXEC tSQLt.AssertEquals @Expected = 0, @Actual = @ActualCount, @Message = 'Should return no records when filter does not match';
    
    DROP TABLE #Results;
END
GO

-- ============================================================================
-- Test 10: Verify consistency - same query returns same results
-- ============================================================================
CREATE OR ALTER PROCEDURE ApplicationWarmupProcess_sp_GetAllLogs_pagination_Tests.[test that pagination is consistent across multiple calls]
AS
BEGIN
    -- Arrange
    EXEC tSQLt.FakeTable 'dbo.ApplicationWarmupProcess';
    EXEC tSQLt.FakeTable 'dbo.ApplicationWarmupProcessLog';
    
    INSERT INTO dbo.ApplicationWarmupProcess (Id, Name, IsActive)
    VALUES (1, 'TestProcess', 1);
    
    -- Insert records with same execution date to test tie-breaking
    DECLARE @SameDate DATETIME = '2025-10-12 10:00:00';
    INSERT INTO dbo.ApplicationWarmupProcessLog (Id, ProcessId, ExecutionDate, Message)
    VALUES 
        (1, 1, @SameDate, 'Message 1'),
        (2, 1, @SameDate, 'Message 2'),
        (3, 1, @SameDate, 'Message 3'),
        (4, 1, @SameDate, 'Message 4'),
        (5, 1, @SameDate, 'Message 5');
    
    -- Act - Run same query twice
    CREATE TABLE #Run1 (Id INT, Name NVARCHAR(255), ExecutionDate DATETIME, Message NVARCHAR(MAX));
    CREATE TABLE #Run2 (Id INT, Name NVARCHAR(255), ExecutionDate DATETIME, Message NVARCHAR(MAX));
    
    INSERT INTO #Run1
    EXEC dbo.ApplicationWarmupProcess_sp_GetAllLogs_pagination
        @PageNumber = 1,
        @PageSize = 3,
        @SortBy = 'ExecutionDate',
        @SortOrder = 'DESC';
    
    INSERT INTO #Run2
    EXEC dbo.ApplicationWarmupProcess_sp_GetAllLogs_pagination
        @PageNumber = 1,
        @PageSize = 3,
        @SortBy = 'ExecutionDate',
        @SortOrder = 'DESC';
    
    -- Assert - Both runs should return same IDs in same order
    DECLARE @Run1Ids NVARCHAR(100), @Run2Ids NVARCHAR(100);
    
    SELECT @Run1Ids = STRING_AGG(CAST(Id AS NVARCHAR(10)), ',') WITHIN GROUP (ORDER BY Id)
    FROM #Run1;
    
    SELECT @Run2Ids = STRING_AGG(CAST(Id AS NVARCHAR(10)), ',') WITHIN GROUP (ORDER BY Id)
    FROM #Run2;
    
    EXEC tSQLt.AssertEqualsString @Expected = @Run1Ids, @Actual = @Run2Ids, @Message = 'Both runs should return identical results';
    
    DROP TABLE #Run1;
    DROP TABLE #Run2;
END
GO

-- ============================================================================
-- Run All Tests
-- ============================================================================
-- Uncomment the following line to run all tests in this class:
-- EXEC tSQLt.Run 'ApplicationWarmupProcess_sp_GetAllLogs_pagination_Tests';

-- Run a specific test:
-- EXEC tSQLt.Run 'ApplicationWarmupProcess_sp_GetAllLogs_pagination_Tests.[test that default pagination parameters return first 10 records]';
