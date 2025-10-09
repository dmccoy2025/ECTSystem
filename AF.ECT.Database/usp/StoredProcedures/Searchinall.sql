-- ============================================================================
-- Author:		Eric Kelley
-- Create date: 1/29/2021
-- Description:	This SP searches all Stored Procedures for a String.		
-- ============================================================================
Create PROCEDURE [usp].[Searchinall]       
(@strFind AS VARCHAR(MAX))
AS
BEGIN
    SET NOCOUNT ON; 
    --TO FIND STRING IN ALL PROCEDURES        
    BEGIN
        SELECT OBJECT_NAME(OBJECT_ID) SP_Name
              ,OBJECT_DEFINITION(OBJECT_ID) SP_Definition
        FROM   sys.procedures
        WHERE  OBJECT_DEFINITION(OBJECT_ID) LIKE '%'+@strFind+'%'
    END 

    --TO FIND STRING IN ALL VIEWS        
    BEGIN
        SELECT OBJECT_NAME(OBJECT_ID) View_Name
              ,OBJECT_DEFINITION(OBJECT_ID) View_Definition
        FROM   sys.views
        WHERE  OBJECT_DEFINITION(OBJECT_ID) LIKE '%'+@strFind+'%'
    END 

    --TO FIND STRING IN ALL FUNCTION        
    BEGIN
        SELECT ROUTINE_NAME           Function_Name
              ,ROUTINE_DEFINITION     Function_definition
        FROM   INFORMATION_SCHEMA.ROUTINES
        WHERE  ROUTINE_DEFINITION LIKE '%'+@strFind+'%'
               AND ROUTINE_TYPE = 'FUNCTION'
        ORDER BY
               ROUTINE_NAME
    END

    --TO FIND STRING IN ALL TABLES OF DATABASE.    
    BEGIN
        SELECT t.name      AS Table_Name
              ,c.name      AS COLUMN_NAME
        FROM   sys.tables  AS t
               INNER JOIN sys.columns c
                    ON  t.OBJECT_ID = c.OBJECT_ID
        WHERE  c.name LIKE '%'+@strFind+'%'
        ORDER BY
               Table_Name
    END
END

--[dbo].[Searchinall] 'P1'
GO

