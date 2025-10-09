-- =============================================
-- Author:		Kenneth Barnett
-- Create date: 10/20/2014
-- Description:	Inserts a new record into the rpt_user_query table. Stores the query id in an output parameter. 
-- Example: 
--	DECLARE @queryId INT
--	EXEC utility_rpt_sp_InsertUserQuery 1, 'Example', '2014-10-07 09:20:01.000', '2014-10-20 10:16:45.000', 0, 1, 'Case Id||Days', 'Days (ASC)', @queryId OUTPUT
-- =============================================
CREATE PROCEDURE [dbo].[utility_rpt_sp_InsertUserQuery] 
(
	  @userId AS INT
	, @queryTitle AS VARCHAR(50)
	, @createdDate AS DATETIME
	, @modifiedDate AS DATETIME
	, @transient AS BIT
	, @shared AS BIT
	, @outputFields AS VARCHAR(MAX)
	, @sortFields AS VARCHAR(MAX)
	, @out_queryId AS INT OUTPUT
)

AS
BEGIN

	SET NOCOUNT ON;
	
	DECLARE @OutputTable TABLE (id int)

	-- Insert the new user query record and store its ID value in the output table. 
	INSERT INTO rpt_user_query ([user_id], [title], [created_date], [modified_date], [transient], [shared], [output_fields], [sort_fields])
	OUTPUT INSERTED.id INTO @OutputTable(id)
	VALUES (@userId, @queryTitle, @createdDate, @modifiedDate, @transient, @shared, @outputFields, @sortFields)

	-- Select the query id from the output table and store it in the output parameter
	SELECT	@out_queryId = id
	FROM	@OutputTable

END
GO

