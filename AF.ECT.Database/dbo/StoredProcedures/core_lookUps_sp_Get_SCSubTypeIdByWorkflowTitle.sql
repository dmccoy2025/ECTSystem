-- =============================================
-- Author:		Kenneth Barnett
-- Create date: 8/28/2014
-- Description:	Returns the ID of the sub type whose title matches the one passed into the stored procedure. 
-- =============================================
CREATE PROCEDURE [dbo].[core_lookUps_sp_Get_SCSubTypeIdByWorkflowTitle] 
	@workflowTitle NVARCHAR(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	IF (@workflowTitle IS NOT NULL)
		BEGIN
			
		SELECT	subTypeId AS Id
		FROM	core_lkupSCSubType
		WHERE	subTypeTitle = @workflowTitle
		
		END
END
GO

