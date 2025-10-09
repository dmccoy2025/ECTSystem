
 -- =============================================
-- Author:		Nandita Srivastava
-- Create date: Feb20th 2009
-- Description:	 Procedure to return parent units info for initialization 
-- =============================================
CREATE PROCEDURE [dbo].[core_pascode_sp_GetParentInfo]
	 
	@parentUnit int 

AS
	BEGIN 
--Fetch the parent info
	 SELECT   CS_ID,LONG_NAME,PAS_CODE,Inactive FROM COMMAND_STRUCT WHERE CS_ID=@parentUnit 
 	 
	END
GO

