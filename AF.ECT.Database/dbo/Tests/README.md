# Redgate SQL Test (tSQLt) Suite

## Overview
This directory contains comprehensive unit tests for the `ApplicationWarmupProcess_sp_GetAllLogs_pagination` stored procedure using the Redgate SQL Test (tSQLt) framework.

## Prerequisites

### 1. Install tSQLt Framework
```sql
-- Download tSQLt from: https://tsqlt.org/downloads/

-- Enable CLR integration (required for tSQLt)
EXEC sp_configure 'clr enabled', 1;
RECONFIGURE;

-- Set database as trustworthy (required for tSQLt)
ALTER DATABASE [ALOD] SET TRUSTWORTHY ON;

-- Run the tSQLt.class.sql script to install the framework
-- (Download and execute from https://tsqlt.org/downloads/)
```

### 2. Verify Installation
```sql
-- Check if tSQLt is installed
SELECT * FROM sys.schemas WHERE name = 'tSQLt';

-- Check tSQLt version
EXEC tSQLt.Info;
```

## Test Files

### `ApplicationWarmupProcess_sp_GetAllLogs_pagination_Tests.sql`
Contains 10 comprehensive test cases:

1. **Default Pagination** - Verifies default parameters return first 10 records
2. **OFFSET Calculation** - Ensures page 2 correctly skips first page records
3. **Process Name Filter** - Tests filtering by process name with partial match
4. **Date Range Filter** - Validates date filtering within specified range
5. **Message Filter** - Tests message filtering with partial match (LIKE operator)
6. **Sorting** - Verifies sorting by Id ASC works correctly
7. **Invalid Sort Column** - Ensures error is raised for invalid @SortBy parameter
8. **Invalid Sort Order** - Ensures error is raised for invalid @SortOrder parameter
9. **Empty Result Set** - Tests behavior when no data matches filters
10. **Consistency** - Verifies same query returns same results (deterministic ordering)

### `RunPaginationTests.sql`
Setup and runner script that:
- Checks for tSQLt installation
- Creates test class if needed
- Runs all tests
- Displays formatted results summary

## Running the Tests

### Option 1: Run All Tests (Recommended)
```sql
-- In SQL Server Management Studio (SSMS):
USE [ALOD];
GO

-- Execute the test runner script
:r RunPaginationTests.sql
```

### Option 2: Run Tests Manually
```sql
USE [ALOD];
GO

-- Run all tests in the class
EXEC tSQLt.Run 'ApplicationWarmupProcess_sp_GetAllLogs_pagination_Tests';

-- Run a specific test
EXEC tSQLt.Run 'ApplicationWarmupProcess_sp_GetAllLogs_pagination_Tests.[test that default pagination parameters return first 10 records]';
```

### Option 3: Run from PowerShell
```powershell
# Using sqlcmd
sqlcmd -S localhost -d ALOD -E -i "RunPaginationTests.sql"
```

## Test Results

### Viewing Results
After running tests, view results with:

```sql
-- View all test results
SELECT Class, TestCase, Result, Msg
FROM tSQLt.TestResult
WHERE Class = 'ApplicationWarmupProcess_sp_GetAllLogs_pagination_Tests'
ORDER BY Result, TestCase;

-- View only failures
SELECT TestCase, Msg
FROM tSQLt.TestResult
WHERE Class = 'ApplicationWarmupProcess_sp_GetAllLogs_pagination_Tests'
    AND Result = 'Failure';

-- Get summary counts
SELECT 
    Result,
    COUNT(*) AS Count
FROM tSQLt.TestResult
WHERE Class = 'ApplicationWarmupProcess_sp_GetAllLogs_pagination_Tests'
GROUP BY Result;
```

## Test Coverage

The test suite covers:

✅ **Pagination Logic**
- Correct OFFSET calculation
- Page size enforcement
- No duplicate records across pages
- Sequential page coverage

✅ **Filtering**
- Process name (partial match)
- Date range (start and end dates)
- Message content (partial match)
- Multiple filters combined

✅ **Sorting**
- Multiple sort columns (Id, Name, ExecutionDate, Message)
- Both ASC and DESC ordering
- Invalid parameter validation

✅ **Edge Cases**
- Empty result sets
- Beyond available data
- Invalid parameters

✅ **Consistency**
- Deterministic ordering
- Repeatable results

## CI/CD Integration

### Azure DevOps Pipeline Example
```yaml
- task: SqlAzureDacpacDeployment@1
  displayName: 'Run tSQLt Tests'
  inputs:
    azureSubscription: '$(AzureSubscription)'
    ServerName: '$(SqlServerName)'
    DatabaseName: '$(DatabaseName)'
    SqlUsername: '$(SqlUsername)'
    SqlPassword: '$(SqlPassword)'
    deployType: 'SqlTask'
    SqlFile: 'AF.ECT.Database/dbo/Tests/RunPaginationTests.sql'
```

### GitHub Actions Example
```yaml
- name: Run tSQLt Tests
  run: |
    sqlcmd -S ${{ secrets.SQL_SERVER }} -d ALOD -U ${{ secrets.SQL_USER }} -P ${{ secrets.SQL_PASSWORD }} -i AF.ECT.Database/dbo/Tests/RunPaginationTests.sql
```

## Maintenance

### Adding New Tests
1. Create a new stored procedure in `ApplicationWarmupProcess_sp_GetAllLogs_pagination_Tests` schema
2. Follow naming convention: `[test that <description>]`
3. Use tSQLt.FakeTable for isolation
4. Use tSQLt.AssertEquals or tSQLt.AssertEqualsString for assertions
5. Run tests to verify

### Updating Tests
When the stored procedure changes:
1. Review affected test cases
2. Update fake table structures if schema changed
3. Update assertions if behavior changed
4. Run full test suite to verify

## Troubleshooting

### Common Issues

**Issue: "tSQLt is not installed"**
```sql
-- Solution: Install tSQLt framework
-- Download from https://tsqlt.org/downloads/
-- Run tSQLt.class.sql in your database
```

**Issue: "CLR integration is not enabled"**
```sql
-- Solution: Enable CLR
EXEC sp_configure 'clr enabled', 1;
RECONFIGURE;
```

**Issue: "Cannot trust assemblies"**
```sql
-- Solution: Set database as trustworthy
ALTER DATABASE [ALOD] SET TRUSTWORTHY ON;
```

**Issue: "Tests fail with 'Invalid object name'"**
```sql
-- Solution: Ensure stored procedure exists
-- Check that ApplicationWarmupProcess_sp_GetAllLogs_pagination is deployed
SELECT * FROM sys.procedures WHERE name = 'ApplicationWarmupProcess_sp_GetAllLogs_pagination';
```

## Best Practices

1. **Isolation**: Each test uses `tSQLt.FakeTable` to isolate from real data
2. **Cleanup**: Temp tables are dropped after each test
3. **Descriptive Names**: Test names clearly describe what they test
4. **Single Responsibility**: Each test validates one specific behavior
5. **Assertions**: Use appropriate tSQLt assertion methods
6. **Documentation**: Each test has header comments explaining purpose

## References

- [tSQLt Official Documentation](https://tsqlt.org/user-guide/)
- [Redgate SQL Test](https://www.red-gate.com/products/sql-development/sql-test/)
- [SQL Server Unit Testing Best Practices](https://www.red-gate.com/simple-talk/databases/sql-server/tools-sql-server/sql-server-unit-testing-best-practices/)

## Support

For issues with:
- **tSQLt Framework**: https://tsqlt.org/
- **These Tests**: Contact the ECTSystem development team
- **Stored Procedure**: See `ApplicationWarmupProcess_sp_GetAllLogs_pagination.sql`
