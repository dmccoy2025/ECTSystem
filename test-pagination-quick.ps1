# ============================================================================
# Quick Pagination Consistency Test
# Tests if the same records appear across multiple runs and pages
# ============================================================================

param(
    [string]$ServerName = "localhost",
    [string]$DatabaseName = "ALOD"
)

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "Quick Pagination Consistency Test" -ForegroundColor Cyan
Write-Host "========================================`n" -ForegroundColor Cyan

# Test parameters
$pageSize = 10
$testRuns = 3

Write-Host "This test will:" -ForegroundColor Yellow
Write-Host "  1. Fetch the same page $testRuns times" -ForegroundColor Gray
Write-Host "  2. Check if results are identical each time" -ForegroundColor Gray
Write-Host "  3. Check for duplicate IDs across first 3 pages" -ForegroundColor Gray
Write-Host ""

# SQL queries
$countQuery = @"
SELECT COUNT(*) AS TotalCount
FROM ApplicationWarmupProcessLog l
JOIN ApplicationWarmupProcess p ON l.ProcessId = p.Id;
"@

$page1Query = @"
EXEC [dbo].[ApplicationWarmupProcess_sp_GetAllLogs_pagination]
    @PageNumber = 1,
    @PageSize = $pageSize,
    @SortBy = 'ExecutionDate',
    @SortOrder = 'DESC';
"@

$page2Query = @"
EXEC [dbo].[ApplicationWarmupProcess_sp_GetAllLogs_pagination]
    @PageNumber = 2,
    @PageSize = $pageSize,
    @SortBy = 'ExecutionDate',
    @SortOrder = 'DESC';
"@

$page3Query = @"
EXEC [dbo].[ApplicationWarmupProcess_sp_GetAllLogs_pagination]
    @PageNumber = 3,
    @PageSize = $pageSize,
    @SortBy = 'ExecutionDate',
    @SortOrder = 'DESC';
"@

function Invoke-SqlQuery {
    param(
        [string]$Query,
        [string]$Server,
        [string]$Database
    )
    
    try {
        $connectionString = "Server=$Server;Database=$Database;Integrated Security=True;TrustServerCertificate=True;"
        $connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
        $connection.Open()
        
        $command = $connection.CreateCommand()
        $command.CommandText = $Query
        $command.CommandTimeout = 30
        
        $adapter = New-Object System.Data.SqlClient.SqlDataAdapter($command)
        $dataset = New-Object System.Data.DataSet
        [void]$adapter.Fill($dataset)
        
        $connection.Close()
        
        return $dataset.Tables
    } catch {
        Write-Host "Error executing query: $_" -ForegroundColor Red
        return $null
    }
}

# Test 1: Get total count
Write-Host "Test 1: Getting total record count..." -ForegroundColor Yellow
$countResult = Invoke-SqlQuery -Query $countQuery -Server $ServerName -Database $DatabaseName
if ($countResult) {
    $totalCount = $countResult[0].Rows[0].TotalCount
    Write-Host "✓ Total records: $totalCount" -ForegroundColor Green
    Write-Host ""
} else {
    Write-Host "✗ Failed to get count" -ForegroundColor Red
    exit 1
}

# Test 2: Consistency check - run same query multiple times
Write-Host "Test 2: Consistency Check (Page 1, $testRuns runs)..." -ForegroundColor Yellow
$runs = @()

for ($i = 1; $i -le $testRuns; $i++) {
    Write-Host "  Run $i..." -ForegroundColor Gray
    $result = Invoke-SqlQuery -Query $page1Query -Server $ServerName -Database $DatabaseName
    if ($result -and $result.Count -ge 2) {
        $dataTable = $result[1] # Second result set is the data
        $ids = $dataTable.Rows | ForEach-Object { $_.Id }
        $runs += ,@($ids)
    }
}

# Compare runs
$allSame = $true
$run1Ids = $runs[0] -join ","
for ($i = 1; $i -lt $runs.Count; $i++) {
    $runIds = $runs[$i] -join ","
    if ($run1Ids -ne $runIds) {
        $allSame = $false
        Write-Host "✗ Run 1 and Run $($i+1) produced different results!" -ForegroundColor Red
        Write-Host "  Run 1 IDs: $run1Ids" -ForegroundColor Gray
        Write-Host "  Run $($i+1) IDs: $runIds" -ForegroundColor Gray
    }
}

if ($allSame) {
    Write-Host "✓ All $testRuns runs produced identical results" -ForegroundColor Green
    Write-Host "  IDs: $run1Ids" -ForegroundColor Gray
} else {
    Write-Host "✗ INCONSISTENCY DETECTED: This indicates a pagination problem!" -ForegroundColor Red
}
Write-Host ""

# Test 3: Check for duplicates across pages
Write-Host "Test 3: Checking for duplicate IDs across pages 1-3..." -ForegroundColor Yellow
$page1Result = Invoke-SqlQuery -Query $page1Query -Server $ServerName -Database $DatabaseName
$page2Result = Invoke-SqlQuery -Query $page2Query -Server $ServerName -Database $DatabaseName
$page3Result = Invoke-SqlQuery -Query $page3Query -Server $ServerName -Database $DatabaseName

$allIds = @()
if ($page1Result -and $page1Result.Count -ge 2) {
    $page1Ids = $page1Result[1].Rows | ForEach-Object { $_.Id }
    $allIds += $page1Ids
    Write-Host "  Page 1: $($page1Ids.Count) records (IDs: $($page1Ids -join ', '))" -ForegroundColor Gray
}
if ($page2Result -and $page2Result.Count -ge 2) {
    $page2Ids = $page2Result[1].Rows | ForEach-Object { $_.Id }
    $allIds += $page2Ids
    Write-Host "  Page 2: $($page2Ids.Count) records (IDs: $($page2Ids -join ', '))" -ForegroundColor Gray
}
if ($page3Result -and $page3Result.Count -ge 2) {
    $page3Ids = $page3Result[1].Rows | ForEach-Object { $_.Id }
    $allIds += $page3Ids
    Write-Host "  Page 3: $($page3Ids.Count) records (IDs: $($page3Ids -join ', '))" -ForegroundColor Gray
}

$uniqueIds = $allIds | Select-Object -Unique
Write-Host ""
Write-Host "  Total IDs across 3 pages: $($allIds.Count)" -ForegroundColor Gray
Write-Host "  Unique IDs: $($uniqueIds.Count)" -ForegroundColor Gray

if ($allIds.Count -eq $uniqueIds.Count) {
    Write-Host "✓ No duplicates found across pages" -ForegroundColor Green
} else {
    Write-Host "✗ DUPLICATES FOUND: Same records appearing on multiple pages!" -ForegroundColor Red
    $duplicates = $allIds | Group-Object | Where-Object { $_.Count -gt 1 }
    foreach ($dup in $duplicates) {
        Write-Host "  ID $($dup.Name) appears $($dup.Count) times" -ForegroundColor Red
    }
}
Write-Host ""

# Summary
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Test Summary" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Total Records: $totalCount" -ForegroundColor White
Write-Host "Consistency: $(if ($allSame) { 'PASS' } else { 'FAIL' })" -ForegroundColor $(if ($allSame) { 'Green' } else { 'Red' })
Write-Host "No Duplicates: $(if ($allIds.Count -eq $uniqueIds.Count) { 'PASS' } else { 'FAIL' })" -ForegroundColor $(if ($allIds.Count -eq $uniqueIds.Count) { 'Green' } else { 'Red' })
Write-Host ""

if (-not $allSame -or $allIds.Count -ne $uniqueIds.Count) {
    Write-Host "⚠ PAGINATION ISSUE CONFIRMED" -ForegroundColor Red
    Write-Host ""
    Write-Host "The stored procedure has non-deterministic ordering due to:" -ForegroundColor Yellow
    Write-Host "  1. Multiple NULL values in ORDER BY CASE statements" -ForegroundColor White
    Write-Host "  2. No tie-breaker column (like Id) to ensure consistent ordering" -ForegroundColor White
    Write-Host ""
    Write-Host "Recommendation: Fix the ORDER BY clause to ensure deterministic sorting" -ForegroundColor Yellow
} else {
    Write-Host "✓ Pagination appears to be working correctly!" -ForegroundColor Green
}
Write-Host ""
