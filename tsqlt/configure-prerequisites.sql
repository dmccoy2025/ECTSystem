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
ALTER DATABASE [ALOD] SET TRUSTWORTHY ON;
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
WHERE name = 'ALOD';
GO
