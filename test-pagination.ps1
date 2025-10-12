# Test script for GetAllLogsPagination endpoint
Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "GetAllLogsPagination Test Suite" -ForegroundColor Cyan
Write-Host "========================================`n" -ForegroundColor Cyan

$baseUrl = "http://localhost:5173/v1/logs/pagination"

$tests = @(
    @{
        name = "Test 1: Basic Pagination"
        body = '{"page_number": 1, "page_size": 10}'
    },
    @{
        name = "Test 2: Large Page Size"
        body = '{"page_number": 1, "page_size": 50}'
    },
    @{
        name = "Test 3: Process Name Filter"
        body = '{"page_number": 1, "page_size": 10, "process_name": "test"}'
    },
    @{
        name = "Test 4: Date Range Filter"
        body = '{"page_number": 1, "page_size": 10, "start_date": "2025-10-01", "end_date": "2025-10-12"}'
    },
    @{
        name = "Test 5: Message Filter"
        body = '{"page_number": 1, "page_size": 10, "message_filter": "error"}'
    },
    @{
        name = "Test 6: Sort Ascending by ExecutionDate"
        body = '{"page_number": 1, "page_size": 10, "sort_by": "ExecutionDate", "sort_order": "ASC"}'
    },
    @{
        name = "Test 7: Sort Descending by Name"
        body = '{"page_number": 1, "page_size": 10, "sort_by": "Name", "sort_order": "DESC"}'
    },
    @{
        name = "Test 8: Combined Filters"
        body = '{"page_number": 1, "page_size": 20, "process_name": "test", "start_date": "2025-10-01", "end_date": "2025-10-12", "message_filter": "success", "sort_by": "ExecutionDate", "sort_order": "DESC"}'
    },
    @{
        name = "Test 9: Page 2"
        body = '{"page_number": 2, "page_size": 10}'
    },
    @{
        name = "Test 10: Empty Request (defaults)"
        body = '{}'
    }
)

$passCount = 0
$failCount = 0

foreach ($test in $tests) {
    try {
        Write-Host $test.name -ForegroundColor Yellow
        Write-Host ("─" * 50) -ForegroundColor Gray
        
        $response = Invoke-WebRequest -Uri $baseUrl -Method POST -ContentType "application/json" -Body $test.body -ErrorAction Stop
        
        $json = $response.Content | ConvertFrom-Json
        
        Write-Host "✓ Status: $($response.StatusCode) OK" -ForegroundColor Green
        Write-Host "✓ Total Count: $($json.totalCount)" -ForegroundColor Green
        Write-Host "✓ Items Returned: $($json.items.Count)" -ForegroundColor Green
        
        if ($json.items.Count -gt 0) {
            Write-Host "✓ First Item:" -ForegroundColor Green
            Write-Host "  - Log ID: $($json.items[0].logId)" -ForegroundColor Gray
            Write-Host "  - Process: $($json.items[0].processName)" -ForegroundColor Gray
            Write-Host "  - Date: $($json.items[0].executionDate)" -ForegroundColor Gray
            Write-Host "  - Message: $($json.items[0].message.Substring(0, [Math]::Min(50, $json.items[0].message.Length)))..." -ForegroundColor Gray
        }
        
        $passCount++
        Write-Host ""
        
    } catch {
        Write-Host "✗ FAILED: $_" -ForegroundColor Red
        $failCount++
        Write-Host ""
    }
}

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "Test Summary" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Passed: $passCount" -ForegroundColor Green
Write-Host "Failed: $failCount" -ForegroundColor Red
Write-Host "Total:  $($passCount + $failCount)" -ForegroundColor Cyan
Write-Host ""
