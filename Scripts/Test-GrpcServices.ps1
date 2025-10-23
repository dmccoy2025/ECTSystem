# Test-GrpcServices.ps1
# PowerShell script to test gRPC service endpoints using grpcurl
# Assumes the gRPC server is running on localhost:5001 (default for ECTSystem)

param(
    [string]$ServerUrl = "localhost:5173",
    [switch]$UseTls = $false
)

$grpcurlCmd = ".\grpcurl.exe"
$options = if ($UseTls) { "" } else { "-plaintext" }

Write-Host "Testing gRPC services on $ServerUrl" -ForegroundColor Green

# Function to run grpcurl command
function Invoke-GrpcUrl {
    param([string]$Args)
    $cmd = "$grpcurlCmd $options $ServerUrl $Args"
    Write-Host "Running: $cmd" -ForegroundColor Yellow
    try {
        $result = Invoke-Expression $cmd 2>&1
        Write-Host "Output:" -ForegroundColor Cyan
        Write-Host $result
        return $result
    } catch {
        Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
        return $null
    }
}

# List all services
Write-Host "`nListing all gRPC services..." -ForegroundColor Green
$services = Invoke-GrpcUrl "list"
if ($services) {
    $serviceList = $services -split "`n" | Where-Object { $_ -match "^[a-zA-Z_][a-zA-Z0-9_]*\.[a-zA-Z_][a-zA-Z0-9_]*$" }
} else {
    Write-Host "Failed to list services" -ForegroundColor Red
    exit 1
}

# For each service, list methods and test
foreach ($service in $serviceList) {
    Write-Host "`nTesting service: $service" -ForegroundColor Green

    # List methods for the service
    $methods = Invoke-GrpcUrl "list $service"
    if ($methods) {
        $methodList = $methods -split "`n" | Where-Object { $_ -and $_ -notmatch "^$" }
    } else {
        Write-Host "Failed to list methods for $service" -ForegroundColor Red
        continue
    }

    foreach ($method in $methodList) {
        Write-Host "`n  Testing method: $service/$method" -ForegroundColor Yellow

        # Attempt to call the method with empty request (may fail for methods requiring params)
        $callResult = Invoke-GrpcUrl "$service/$method {}"
        if ($callResult -and $callResult -notmatch "ERROR") {
            Write-Host "  Success" -ForegroundColor Green
        } else {
            Write-Host "  Failed or requires parameters" -ForegroundColor Red
        }
    }
}

Write-Host "`nTesting complete." -ForegroundColor Green