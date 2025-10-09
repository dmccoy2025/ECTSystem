
-- ============================================================================
-- Author:		Kenneth Barnett
-- Create date: 5/25/2016
-- Description:	Updates the ExecuteOrder value for PH Ad Hoc query parameter 
--				records that match the clause id when a new record is inserted
--				or edited. Called after new record has already been edit/inserted. 
--				filter = 0 means an edit occurred; filter = 1 means an insert 
--				occurred; filter = 2 means a delete occurred
--				Modeled after the query_param_sp_UpdateExecuteOrders stored
--				procedure.
-- ============================================================================
CREATE PROCEDURE [dbo].[ph_query_param_sp_UpdateExecuteOrders]  
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
			UPDATE	rpt_ph_user_query_param
			SET		ExecuteOrder = ExecuteOrder - 1
			WHERE	PHClauseId = @clauseId
					AND Id != @paramId
					AND ExecuteOrder <= @newOrder
					AND ExecuteOrder >= @oldOrder			
		END
		
		-- If EO decreases, then increment the EO of records who have an EO larger or equal to the new EO.
		IF @newOrder < @oldOrder 
		BEGIN
			UPDATE	rpt_ph_user_query_param
			SET		ExecuteOrder = ExecuteOrder + 1
			WHERE	PHClauseId = @clauseId
					AND Id != @paramId
					AND ExecuteOrder >= @newOrder
					AND ExecuteOrder <= @oldOrder
		END
	END
		
	-- Insertion of a new record has occurred. 
	IF @filter = 1
	BEGIN
		DECLARE @duplicates int = 0
		
		SELECT	@duplicates = COUNT(*)
		FROM	rpt_ph_user_query_param
		WHERE	PHClauseId = @clauseId
				AND ExecuteOrder = @newOrder
		
		-- If insert and EO equals an existing execute order
		IF @duplicates > 1
		BEGIN
			-- Increase the EO for all records >= the new record's EO
			UPDATE	rpt_ph_user_query_param
			SET		ExecuteOrder = ExecuteOrder + 1
			WHERE	PHClauseId = @clauseId
					AND Id != @paramId
					AND ExecuteOrder >= @newOrder
		END	
	END

	-- Deletion of a new record has occurred. 
	IF @filter = 2
	BEGIN
		UPDATE	rpt_ph_user_query_param
		SET		ExecuteOrder = ExecuteOrder - 1
		WHERE	PHClauseId = @clauseId
				AND ExecuteOrder > @oldOrder	
	END
END
GO

