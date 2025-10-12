-- Run these commands in SQL Server Management Studio:

USE [master];
GO

-- Enable CLR integration (required for tSQLt)
EXEC sp_configure 'clr enabled', 1;
RECONFIGURE;
GO

-- Set database as trustworthy (required for tSQLt)
ALTER DATABASE [ALOD] SET TRUSTWORTHY ON;
GO

USE [ALOD];
GO

-- Run the tSQLt.class.sql file you downloaded
-- In SSMS: File > Open > File > Select tSQLt.class.sql > Execute

-- Verify installation
SELECT * FROM sys.schemas WHERE name = 'tSQLt';
EXEC tSQLt.Info;
