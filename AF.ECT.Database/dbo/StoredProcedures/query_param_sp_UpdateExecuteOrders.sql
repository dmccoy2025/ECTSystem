-- =============================================
-- Author:		Kenneth Barnett
-- Create date: 7/10/2014
-- Description:	Updates the execute_order value for records that match the clause
--				id when a new record is inserted or edited. 
--				Called after new record has already been edit/inserted. 
--				filter = 0 means an edit occurred; filter = 1 means an insert occurred; filter = 2 means a delete occurred
-- =============================================
CREATE PROCEDURE [dbo].[query_param_sp_UpdateExecuteOrders]  
	@clauseId INT,
	@paramId INT,
	@newOrder INT,
	@oldOrder INT,
	@filter INT
AS
BEGIN
	SET NOCOUNT ON;
    
    -- EO = Execute Order
    
    -- Edit of an existing record has occurred. 
    IF @filter = 0
		BEGIN
		-- If EO increases, then decrement the EO of records who have an EO smaller or equal to the new EO. 
		IF @newOrder > @oldOrder
			BEGIN
			
			UPDATE	rpt_user_query_param
			SET		execute_order = execute_order - 1
			WHERE	clause_id = @clauseId
					AND id != @paramId
					AND execute_order <= @newOrder
					AND execute_order >= @oldOrder
					
			END
		
		-- If EO decreases, then increment the EO of records who have an EO larger or equal to the new EO.
		IF @newOrder < @oldOrder 
			BEGIN
			
			UPDATE	rpt_user_query_param
			SET		execute_order = execute_order + 1
			WHERE	clause_id = @clauseId
					AND id != @paramId
					AND execute_order >= @newOrder
					AND execute_order <= @oldOrder
			
			END
		END
		
	-- Insertion of a new record has occurred. 
	IF @filter = 1
		BEGIN
		
		DECLARE @duplicates int = 0
		
		SELECT	@duplicates = COUNT(*)
		FROM	rpt_user_query_param
		WHERE	clause_id = @clauseId
				AND execute_order = @newOrder
		
		--If insert and EO equals an existing execute order
		IF @duplicates > 1
			BEGIN
			
			-- Increase the EO for all records >= the new record's EO
			UPDATE	rpt_user_query_param
			SET		execute_order = execute_order + 1
			WHERE	clause_id = @clauseId
					AND id != @paramId
					AND execute_order >= @newOrder
			
			END
		
		END

	-- Deletion of a new record has occurred. 
	IF @filter = 2
		BEGIN
		
		UPDATE	rpt_user_query_param
		SET		execute_order = execute_order - 1
		WHERE	clause_id = @clauseId
				AND execute_order > @oldOrder
		
		END
END
GO

