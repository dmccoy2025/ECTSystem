USE [ALOD];
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
