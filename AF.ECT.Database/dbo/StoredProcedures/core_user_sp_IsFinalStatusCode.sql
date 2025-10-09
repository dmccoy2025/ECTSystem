


CREATE PROCEDURE [dbo].[core_user_sp_IsFinalStatusCode]
	@statusId tinyint

AS

SET NOCOUNT ON

SELECT 
		isFinal  
FROM 
	core_StatusCodes 

where statusId=@statusId
GO

