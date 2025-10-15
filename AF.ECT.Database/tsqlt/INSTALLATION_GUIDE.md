# tSQLt Installation Guide

## Current Status
✅ **Prerequisites Configured:**
- CLR integration: **ENABLED**
- Database TRUSTWORTHY: **ENABLED**

## Manual Installation Required

The automated download is not working, so please follow these manual steps:

### Step 1: Download tSQLt

Visit one of these sources:

**Option A: Official Website (Recommended)**
1. Go to: https://tsqlt.org/downloads/
2. Download the latest tSQLt version
3. Extract the ZIP file

**Option B: Direct SQL Server Database Project**
1. Go to: https://github.com/tSQLt-org/tSQLt/
2. Navigate to Releases
3. Download the latest `tSQLt.zip` or `tSQLtFiles.zip`

### Step 2: Install tSQLt

Once you have the tSQLt.class.sql file:

**Using sqlcmd (Command Line):**
```powershell
sqlcmd -S localhost -d ALOD -E -i "C:\path\to\tSQLt.class.sql"
```

**Using SQL Server Management Studio:**
1. Open SSMS and connect to `localhost`
2. Select the `ALOD` database
3. Open `tSQLt.class.sql` (File → Open → File)
4. Execute the script (F5)
5. Wait for completion (may take 1-2 minutes)

### Step 3: Verify Installation

Run this command to verify:
```powershell
sqlcmd -S localhost -d ALOD -E -Q "EXEC tSQLt.Info;"
```

Or in SSMS:
```sql
USE ALOD;
GO

-- Check if tSQLt is installed
SELECT * FROM sys.schemas WHERE name = 'tSQLt';

-- Get tSQLt version info
EXEC tSQLt.Info;
```

### Step 4: Run Your Tests

Once verified, run the pagination tests:
```powershell
.\run-tsqlt-tests.ps1
```

## Troubleshooting

### If you see "Could not load assembly"
```sql
-- Enable CLR again
USE master;
GO
EXEC sp_configure 'clr enabled', 1;
RECONFIGURE;
GO
```

### If you see "Database is not trustworthy"
```sql
-- Set trustworthy again
USE master;
GO
ALTER DATABASE ALOD SET TRUSTWORTHY ON;
GO
```

### If installation hangs
- Be patient - tSQLt installation can take 1-2 minutes
- Check Task Manager for sqlservr.exe activity
- Do NOT cancel the installation mid-way

## Alternative: Use Built-in SQL Tests

If you prefer not to install tSQLt, you can use the PowerShell test scripts instead:

```powershell
# Quick pagination test
.\test-pagination-quick.ps1 -DatabaseName "ALOD"

# Comprehensive SQL tests (no tSQLt required)
sqlcmd -S localhost -d ALOD -E -i "test-sp-pagination.sql"
```

## Files Created

- `tsqlt\configure-prerequisites.sql` - CLR and trustworthy setup (already run)
- `tsqlt\install-tsqlt-manual.sql` - Manual installation commands
- `tsqlt\verify-tsqlt.sql` - Verification script

## Need Help?

- tSQLt Documentation: https://tsqlt.org/user-guide/
- tSQLt GitHub: https://github.com/tSQLt-org/tSQLt
- SQL Server CLR: https://learn.microsoft.com/en-us/sql/relational-databases/clr-integration/
