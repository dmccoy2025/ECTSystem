-- =============================================
-- Author:		Kenneth Barnett
-- Create date: 10/20/2014
-- Description:	Inserts a new record into the rpt_user_query_clause table. Stores the clause id in an output parameter. 
-- Example: 
--	DECLARE @clauseId INT
--	EXEC utility_rpt_sp_InsertUserQueryClause 17, 'AND', @clauseId OUTPUT
-- =============================================
CREATE PROCEDURE [dbo].[utility_rpt_sp_InsertUserQueryClause]
(
	  @queryId AS INT
	, @whereType AS VARCHAR(3)
	, @out_clauseId AS INT OUTPUT
)

AS
BEGIN

	SET NOCOUNT ON;
	
	DECLARE @OutputTable TABLE (id int)

	-- Insert the new user query clause record and store its ID value in the output table. 
	INSERT INTO rpt_user_query_clause ([query_id], [where_type])
	OUTPUT INSERTED.id INTO @OutputTable
	VALUES (@queryId, @whereType)

	-- Select the clause id from the output table and store it in the output parameter
	SELECT	@out_clauseId = id
	FROM	@OutputTable

END
GO

