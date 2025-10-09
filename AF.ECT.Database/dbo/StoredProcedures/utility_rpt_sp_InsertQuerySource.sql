-- =============================================
-- Author:		Kenneth Barnett
-- Create date: 5/23/2014
-- Description:	Insert a new record into the rpt_query_source table. 
-- =============================================
-- Modified By:		Evan Morrison
-- Date: 1/10/2017
-- Description:	update to add two new columns to table 
-- =============================================
CREATE PROCEDURE [dbo].[utility_rpt_sp_InsertQuerySource]
	 @displayName VARCHAR(50)
	,@fieldName VARCHAR(50)
	,@tableName VARCHAR(50)
	,@dataType CHAR(1)
	,@lookupSource VARCHAR(50)
	,@lookupValue VARCHAR(50)
	,@lookupText VARCHAR(50)
	,@lookupSort VARCHAR(50)
	,@lookupWhere VARCHAR(50)
	,@lookupWhereValue VARCHAR(50)
AS


IF NOT EXISTS(SELECT * FROM dbo.rpt_query_source WHERE display_name = @displayName AND field_name = @fieldName AND table_name = @tableName)
	BEGIN
		INSERT INTO [dbo].[rpt_query_source] ([display_name], [field_name], [table_name], [data_type], [lookup_source], [lookup_value], [lookup_text], [lookup_sort], [lookup_where], [lookup_where_value]) 
			VALUES (@displayName, @fieldName, @tableName, @dataType, @lookupSource, @lookupValue, @lookupText, @lookupSort, @lookupWhere, @lookupWhereValue)
		
		PRINT 'Inserted new values (' + @displayName + ', ' + 
										@fieldName + ', ' +
										@tableName + ', ' +
										@dataType + ', '+
										@lookupSource + ', ' +
										@lookupValue + ', ' +
										@lookupText + ', ' + 
										@lookupSort + ', ' +
										@lookupWhere + ', ' + 
										@lookupWhereValue + ') into rpt_query_source table.'
										
	END
ELSE
	BEGIN
		PRINT 'Values already exist in rpt_query_source table.'
	END
GO

