-- =============================================
-- Author:		Kenneth Barnett
-- Create date: 10/20/2014
-- Description:	Inserts a new record into the rpt_user_query_clause table. Stores the clause id in an output parameter. 
-- Example: 
--	EXEC utility_rpt_sp_InsertUserQueryParam 36, 5, 'greater', 'AND', '20', '20', '', '', 1
-- =============================================
CREATE PROCEDURE [dbo].[utility_rpt_sp_InsertUserQueryParam]
(
	  @clauseId AS INT
	, @sourceDisplayName AS VARCHAR(50)
	, @sourceTableName AS VARCHAR(50)
	, @operatorType AS VARCHAR(50)
	, @whereType AS VARCHAR(3)
	, @startValue AS VARCHAR(100)
	, @startDisplay AS VARCHAR(100)
	, @endValue AS VARCHAR(100)
	, @endDisplay AS VARCHAR(100)
	, @executeOrder AS INT
)

AS
BEGIN

	SET NOCOUNT ON;
	
	DECLARE @sourceId AS INT = 0
	
	-- Attempt to find the ID of the source
	SELECT	@sourceId = id
	FROM	rpt_query_source
	WHERE	display_name = @sourceDisplayName
			AND table_name = @sourceTableName
	
	-- Check if an ID was found
	IF @sourceId > 0
		BEGIN

		-- Insert the new user query clause record and store its ID value in the output table. 
		INSERT INTO rpt_user_query_param ([clause_id], [source_id], [operator_type], [where_type], [start_value], [start_display], [end_value], [end_display], [execute_order])
		VALUES (@clauseId, @sourceId, @operatorType, @whereType, @startValue, @startDisplay, @endValue, @endDisplay, @executeOrder)

		SELECT *
		FROM rpt_user_query_param
		WHERE clause_id = @clauseId
		
		END
	ELSE
		BEGIN
		
		PRINT 'Could not find ID for the source that has display name of ' + @sourceDisplayName + ' and table name of ' + @sourceTableName + '.'
		
		END

END
GO

