-- =============================================
-- Author:		Kenneth Barnett
-- Create date: 8/28/2014
-- Description:	Returns all of the sub type workflow titles and ids for a specific workflow Id. 
-- =============================================
CREATE PROCEDURE [dbo].[core_lookUps_sp_Get_SCSubTypesByWorkflowId] 
	@workflowId INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	IF (@workflowId IS NOT NULL)
		BEGIN
			
		SELECT	subTypeId AS Id, subTypeTitle AS Title
		FROM	core_lkupSCSubType
		WHERE	associatedWorkflowId = @workflowId
		
		END
END
GO

