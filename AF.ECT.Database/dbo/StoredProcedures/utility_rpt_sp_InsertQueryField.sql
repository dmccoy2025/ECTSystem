-- =============================================
-- Author:		Kenneth Barnett
-- Create date: 5/23/2014
-- Description:	Insert a new record into the rpt_query_fields table. 
-- =============================================
CREATE PROCEDURE [dbo].[utility_rpt_sp_InsertQueryField]
	 @fieldName VARCHAR(50)
	,@queryType VARCHAR(20)
	,@sortOrder INT
	,@tableName VARCHAR(50)
AS


IF NOT EXISTS(SELECT * FROM dbo.rpt_query_fields WHERE field_name = @fieldName AND table_name = @tableName)
	BEGIN
		INSERT INTO [dbo].[rpt_query_fields] ([field_name], [query_type], [sort_order], [table_name]) 
			VALUES (@fieldName, @queryType, @sortOrder, @tableName)
		
		PRINT 'Inserted new values (' + @fieldName + ', ' + 
										@queryType + ', ' +
										CONVERT(CHAR(1), @sortOrder) + ', ' +
										@tableName + ') into rpt_query_fields table.'
										
	END
ELSE
	BEGIN
		PRINT 'Values already exist in rpt_query_fields table.'
	END
GO

