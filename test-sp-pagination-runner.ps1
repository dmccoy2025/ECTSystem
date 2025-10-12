# ============================================================================
# PowerShell Test Runner for Stored Procedure Pagination Testing
# ============================================================================

param(
    [string]$ServerName = "localhost",
    [string]$DatabaseName = "ALOD",
    [string]$SqlFile = "test-sp-pagination.sql"
)

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "Stored Procedure Pagination Test Runner" -ForegroundColor Cyan
Write-Host "========================================`n" -ForegroundColor Cyan

# Check if SQL file exists
if (-not (Test-Path $SqlFile)) {
    Write-Host "Error: SQL file '$SqlFile' not found!" -ForegroundColor Red
    exit 1
}

Write-Host "Configuration:" -ForegroundColor Yellow
Write-Host "  Server: $ServerName" -ForegroundColor Gray
Write-Host "  Database: $DatabaseName" -ForegroundColor Gray
Write-Host "  SQL File: $SqlFile" -ForegroundColor Gray
Write-Host ""

# Try to run with sqlcmd
Write-Host "Attempting to run tests with sqlcmd..." -ForegroundColor Yellow

try {
    $sqlcmdPath = Get-Command sqlcmd -ErrorAction Stop
    Write-Host "✓ sqlcmd found at: $($sqlcmdPath.Source)" -ForegroundColor Green
    Write-Host ""
    
    Write-Host "Executing SQL test script..." -ForegroundColor Cyan
    Write-Host ("=" * 80) -ForegroundColor Gray
    
    & sqlcmd -S $ServerName -d $DatabaseName -E -i $SqlFile -b
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host ("=" * 80) -ForegroundColor Gray
        Write-Host "✓ Tests completed successfully!" -ForegroundColor Green
    } else {
        Write-Host ""
        Write-Host ("=" * 80) -ForegroundColor Gray
        Write-Host "✗ Tests completed with errors (exit code: $LASTEXITCODE)" -ForegroundColor Red
    }
    
} catch {
    Write-Host "✗ sqlcmd not found. Please install SQL Server command-line tools." -ForegroundColor Red
    Write-Host ""
    Write-Host "Alternative: Run the SQL script manually in SQL Server Management Studio" -ForegroundColor Yellow
    Write-Host "  File location: $((Resolve-Path $SqlFile).Path)" -ForegroundColor Gray
    Write-Host ""
    Write-Host "Or install sqlcmd:" -ForegroundColor Yellow
    Write-Host "  Windows: Install 'Microsoft Command Line Utilities for SQL Server'" -ForegroundColor Gray
    Write-Host "  Download: https://aka.ms/sqlcmd" -ForegroundColor Gray
    exit 1
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Analysis Tips" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Review the results for:" -ForegroundColor Yellow
Write-Host "  1. ✓ Duplicate Check: Should show 'PASS: No duplicates'" -ForegroundColor White
Write-Host "  2. ✓ Consistency: Two runs should produce identical results" -ForegroundColor White
Write-Host "  3. ✓ OFFSET Calculation: Verify records are skipped correctly" -ForegroundColor White
Write-Host "  4. ⚠ CASE Statement NULLs: Most columns will show NULL (this is the problem!)" -ForegroundColor White
Write-Host ""
Write-Host "If you see issues:" -ForegroundColor Yellow
Write-Host "  - Records appearing on multiple pages = Non-deterministic ordering" -ForegroundColor White
Write-Host "  - Inconsistent results on repeated queries = Missing tie-breaker" -ForegroundColor White
Write-Host "  - Gaps in pagination = CASE statement evaluation problem" -ForegroundColor White
Write-Host ""
