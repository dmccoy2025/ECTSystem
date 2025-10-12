# Test GetAllLogsPagination endpoint

$baseUrl = "http://localhost:5174"

Write-Host "Testing GetAllLogsPagination endpoint..." -ForegroundColor Cyan
Write-Host ""

# Test 1: Basic pagination
Write-Host "Test 1: Basic pagination (page 1, 10 items)" -ForegroundColor Yellow
try {
    $body = @{
        page_number = 1
        page_size = 10
    } | ConvertTo-Json

    $response = Invoke-RestMethod -Uri "$baseUrl/v1/logs/pagination" -Method POST -ContentType "application/json" -Body $body
    Write-Host "Success! Total Count: $($response.total_count)" -ForegroundColor Green
    Write-Host "Items returned: $($response.items.Count)" -ForegroundColor Green
    Write-Host ""
    $response.items | Select-Object -First 3 | Format-Table log_id, process_name, execution_date
} catch {
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""
Write-Host "----------------------------------------" -ForegroundColor Cyan
Write-Host ""

# Test 2: Larger page size
Write-Host "Test 2: Larger page size (page 1, 50 items)" -ForegroundColor Yellow
try {
    $body = @{
        page_number = 1
        page_size = 50
    } | ConvertTo-Json

    $response = Invoke-RestMethod -Uri "$baseUrl/v1/logs/pagination" -Method POST -ContentType "application/json" -Body $body
    Write-Host "Success! Total Count: $($response.total_count)" -ForegroundColor Green
    Write-Host "Items returned: $($response.items.Count)" -ForegroundColor Green
} catch {
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""
Write-Host "----------------------------------------" -ForegroundColor Cyan
Write-Host ""

# Test 3: With sorting
Write-Host "Test 3: Sorting (ASC by ExecutionDate)" -ForegroundColor Yellow
try {
    $body = @{
        page_number = 1
        page_size = 10
        sort_by = "ExecutionDate"
        sort_order = "ASC"
    } | ConvertTo-Json

    $response = Invoke-RestMethod -Uri "$baseUrl/v1/logs/pagination" -Method POST -ContentType "application/json" -Body $body
    Write-Host "Success! Total Count: $($response.total_count)" -ForegroundColor Green
    Write-Host "Items returned: $($response.items.Count)" -ForegroundColor Green
    Write-Host ""
    $response.items | Select-Object -First 5 | Format-Table log_id, process_name, execution_date
} catch {
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""
Write-Host "----------------------------------------" -ForegroundColor Cyan
Write-Host ""

# Test 4: Default parameters
Write-Host "Test 4: Default parameters (empty body)" -ForegroundColor Yellow
try {
    $body = @{} | ConvertTo-Json

    $response = Invoke-RestMethod -Uri "$baseUrl/v1/logs/pagination" -Method POST -ContentType "application/json" -Body $body
    Write-Host "Success! Total Count: $($response.total_count)" -ForegroundColor Green
    Write-Host "Items returned: $($response.items.Count)" -ForegroundColor Green
} catch {
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""
Write-Host "All tests completed!" -ForegroundColor Cyan
