# ============================================================================
# tSQLt Framework Installation Script
# Downloads and installs tSQLt into the ALOD database
# ============================================================================

param(
    [string]$ServerName = "localhost",
    [string]$DatabaseName = "ALOD",
    [string]$DownloadPath = "$PSScriptRoot\tsqlt"
)

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "tSQLt Framework Installer" -ForegroundColor Cyan
Write-Host "========================================`n" -ForegroundColor Cyan

Write-Host "Configuration:" -ForegroundColor Yellow
Write-Host "  Server:        $ServerName" -ForegroundColor Gray
Write-Host "  Database:      $DatabaseName" -ForegroundColor Gray
Write-Host "  Download Path: $DownloadPath" -ForegroundColor Gray
Write-Host ""

# Create download directory if it doesn't exist
if (-not (Test-Path $DownloadPath)) {
    New-Item -ItemType Directory -Path $DownloadPath | Out-Null
    Write-Host "✓ Created download directory: $DownloadPath" -ForegroundColor Green
}

# Step 1: Download tSQLt
Write-Host "`nStep 1: Downloading tSQLt..." -ForegroundColor Yellow

$tSQLtVersion = "latest"
$zipUrl = "https://tsqlt.org/downloads/tsqlt/latest/"
$zipFile = Join-Path $DownloadPath "tSQLt.zip"
$extractPath = Join-Path $DownloadPath "extracted"

try {
    Write-Host "  Downloading from tsqlt.org..." -ForegroundColor Gray
    
    # Try to get the latest download link from the website
    $downloadPage = "https://github.com/tSQLt-org/tSQLt/releases/latest"
    $response = Invoke-WebRequest -Uri $downloadPage -UseBasicParsing -ErrorAction Stop
    
    # Find the .zip file link in the releases
    $downloadLink = $response.Links | Where-Object { $_.href -like "*/tSQLt.zip" } | Select-Object -First 1
    
    if ($downloadLink) {
        $fullUrl = "https://github.com" + $downloadLink.href
        Write-Host "  Found download: $fullUrl" -ForegroundColor Gray
        
        Invoke-WebRequest -Uri $fullUrl -OutFile $zipFile -UseBasicParsing
        Write-Host "✓ Downloaded tSQLt.zip" -ForegroundColor Green
    } else {
        Write-Host "⚠ Could not auto-download. Please download manually." -ForegroundColor Yellow
        Write-Host ""
        Write-Host "Manual Installation Steps:" -ForegroundColor Cyan
        Write-Host "1. Download tSQLt from: https://tsqlt.org/downloads/" -ForegroundColor White
        Write-Host "2. Extract the ZIP file" -ForegroundColor White
        Write-Host "3. Find the tSQLt.class.sql file" -ForegroundColor White
        Write-Host "4. Run the SQL commands below in SSMS" -ForegroundColor White
        Write-Host ""
        
        # Show the SQL commands to run manually
        $manualSQL = @"
-- Run these commands in SQL Server Management Studio:

USE [master];
GO

-- Enable CLR integration (required for tSQLt)
EXEC sp_configure 'clr enabled', 1;
RECONFIGURE;
GO

-- Set database as trustworthy (required for tSQLt)
ALTER DATABASE [$DatabaseName] SET TRUSTWORTHY ON;
GO

USE [$DatabaseName];
GO

-- Run the tSQLt.class.sql file you downloaded
-- In SSMS: File > Open > File > Select tSQLt.class.sql > Execute

-- Verify installation
SELECT * FROM sys.schemas WHERE name = 'tSQLt';
EXEC tSQLt.Info;
"@
        
        Write-Host $manualSQL -ForegroundColor Gray
        
        # Save to file
        $sqlFile = Join-Path $DownloadPath "install-tsqlt-manual.sql"
        $manualSQL | Out-File -FilePath $sqlFile -Encoding UTF8
        Write-Host ""
        Write-Host "✓ SQL commands saved to: $sqlFile" -ForegroundColor Green
        Write-Host ""
        Write-Host "Would you like to continue with automated setup of prerequisites? (y/n): " -ForegroundColor Yellow -NoNewline
        $continue = Read-Host
        
        if ($continue -ne 'y') {
            Write-Host ""
            Write-Host "Installation paused. Please follow manual steps above." -ForegroundColor Yellow
            exit 0
        }
    }
} catch {
    Write-Host "⚠ Download error: $_" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Please download tSQLt manually from: https://tsqlt.org/downloads/" -ForegroundColor Yellow
    Write-Host "Then extract it to: $extractPath" -ForegroundColor Gray
    Write-Host ""
    Write-Host "Press Enter when ready to continue with prerequisite setup..." -ForegroundColor Yellow
    Read-Host
}

# Step 2: Configure SQL Server Prerequisites
Write-Host "`nStep 2: Configuring SQL Server Prerequisites..." -ForegroundColor Yellow

$prereqSQL = @"
USE [master];
GO

-- Enable CLR integration
PRINT 'Enabling CLR integration...';
EXEC sp_configure 'show advanced options', 1;
RECONFIGURE;
EXEC sp_configure 'clr enabled', 1;
RECONFIGURE;
PRINT 'CLR integration enabled';
GO

-- Set database as trustworthy
PRINT 'Setting database as trustworthy...';
ALTER DATABASE [$DatabaseName] SET TRUSTWORTHY ON;
PRINT 'Database set as trustworthy';
GO

-- Verify settings
PRINT '';
PRINT 'Verification:';
SELECT name, value_in_use 
FROM sys.configurations 
WHERE name = 'clr enabled';

SELECT name, is_trustworthy_on 
FROM sys.databases 
WHERE name = '$DatabaseName';
GO
"@

$prereqFile = Join-Path $DownloadPath "configure-prerequisites.sql"
$prereqSQL | Out-File -FilePath $prereqFile -Encoding UTF8

Write-Host "  Executing prerequisite configuration..." -ForegroundColor Gray

try {
    $prereqOutput = & sqlcmd -S $ServerName -d master -E -i $prereqFile 2>&1
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✓ Prerequisites configured successfully" -ForegroundColor Green
        
        # Show verification output
        $prereqOutput | ForEach-Object {
            if ($_ -match "enabled|trustworthy") {
                Write-Host "  $_" -ForegroundColor Green
            } elseif ($_ -match "clr enabled|Verification") {
                Write-Host "  $_" -ForegroundColor Gray
            }
        }
    } else {
        Write-Host "✗ Failed to configure prerequisites" -ForegroundColor Red
        $prereqOutput | ForEach-Object { Write-Host "  $_" -ForegroundColor Gray }
    }
} catch {
    Write-Host "✗ Error configuring prerequisites: $_" -ForegroundColor Red
}

# Step 3: Install tSQLt
Write-Host "`nStep 3: Installing tSQLt Framework..." -ForegroundColor Yellow

# Check if tSQLt.class.sql exists
$tsqltClassFile = Join-Path $extractPath "tSQLt.class.sql"

if (Test-Path $zipFile) {
    Write-Host "  Extracting tSQLt.zip..." -ForegroundColor Gray
    try {
        Expand-Archive -Path $zipFile -DestinationPath $extractPath -Force
        Write-Host "✓ Extracted tSQLt files" -ForegroundColor Green
        
        # Find the tSQLt.class.sql file
        $tsqltFiles = Get-ChildItem -Path $extractPath -Filter "tSQLt.class.sql" -Recurse
        if ($tsqltFiles) {
            $tsqltClassFile = $tsqltFiles[0].FullName
            Write-Host "✓ Found tSQLt.class.sql: $tsqltClassFile" -ForegroundColor Green
        }
    } catch {
        Write-Host "⚠ Extraction error: $_" -ForegroundColor Yellow
    }
}

if (Test-Path $tsqltClassFile) {
    Write-Host "  Installing tSQLt into database..." -ForegroundColor Gray
    Write-Host "  (This may take a few minutes...)" -ForegroundColor Gray
    
    try {
        $installOutput = & sqlcmd -S $ServerName -d $DatabaseName -E -i $tsqltClassFile -b 2>&1
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "✓ tSQLt framework installed successfully!" -ForegroundColor Green
        } else {
            Write-Host "✗ Installation failed" -ForegroundColor Red
            $installOutput | Select-Object -Last 20 | ForEach-Object { Write-Host "  $_" -ForegroundColor Gray }
        }
    } catch {
        Write-Host "✗ Installation error: $_" -ForegroundColor Red
    }
} else {
    Write-Host "⚠ tSQLt.class.sql not found" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Please install manually:" -ForegroundColor Yellow
    Write-Host "1. Download from: https://tsqlt.org/downloads/" -ForegroundColor White
    Write-Host "2. Extract and find tSQLt.class.sql" -ForegroundColor White
    Write-Host "3. Run: sqlcmd -S $ServerName -d $DatabaseName -E -i ""path\to\tSQLt.class.sql""" -ForegroundColor White
}

# Step 4: Verify Installation
Write-Host "`nStep 4: Verifying Installation..." -ForegroundColor Yellow

$verifySQL = @"
USE [$DatabaseName];
GO

-- Check if tSQLt schema exists
IF EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'tSQLt')
BEGIN
    PRINT 'SUCCESS: tSQLt schema found';
    
    -- Try to run tSQLt.Info
    BEGIN TRY
        EXEC tSQLt.Info;
        PRINT 'SUCCESS: tSQLt is working correctly';
    END TRY
    BEGIN CATCH
        PRINT 'ERROR: tSQLt schema exists but Info command failed';
        PRINT ERROR_MESSAGE();
    END CATCH
END
ELSE
BEGIN
    PRINT 'ERROR: tSQLt schema not found';
END
GO
"@

$verifyFile = Join-Path $DownloadPath "verify-tsqlt.sql"
$verifySQL | Out-File -FilePath $verifyFile -Encoding UTF8

try {
    $verifyOutput = & sqlcmd -S $ServerName -d $DatabaseName -E -i $verifyFile 2>&1
    
    $verifyOutput | ForEach-Object {
        if ($_ -match "SUCCESS:") {
            Write-Host "  $_" -ForegroundColor Green
        } elseif ($_ -match "ERROR:") {
            Write-Host "  $_" -ForegroundColor Red
        } elseif ($_ -match "Version|tSQLt") {
            Write-Host "  $_" -ForegroundColor Cyan
        }
    }
} catch {
    Write-Host "✗ Verification error: $_" -ForegroundColor Red
}

# Summary
Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Installation Summary" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Files created in: $DownloadPath" -ForegroundColor Gray
Write-Host "  - configure-prerequisites.sql" -ForegroundColor Gray
Write-Host "  - verify-tsqlt.sql" -ForegroundColor Gray
if (Test-Path (Join-Path $DownloadPath "install-tsqlt-manual.sql")) {
    Write-Host "  - install-tsqlt-manual.sql (for manual installation)" -ForegroundColor Gray
}
Write-Host ""
Write-Host "Next Steps:" -ForegroundColor Yellow
Write-Host "  1. Verify installation: sqlcmd -S $ServerName -d $DatabaseName -E -Q ""EXEC tSQLt.Info;""" -ForegroundColor White
Write-Host "  2. Run tests: .\run-tsqlt-tests.ps1" -ForegroundColor White
Write-Host ""
