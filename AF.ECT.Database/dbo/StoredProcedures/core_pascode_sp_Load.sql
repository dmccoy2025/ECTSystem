
 -- =============================================
-- Author:		Nandita Srivastava
-- Create date: Feb20th 2009
-- Description:	 Procedure to return pascode information like email etc 
-- =============================================
CREATE PROCEDURE [dbo].[core_pascode_sp_Load]
	 
	@cs_id int 

AS
	BEGIN 
--Fetch the parent info
	 SELECT 
		   p.*
		 FROM 
			vw_pasCode p
		 WHERE 
			CS_ID=@cs_id
 	 
	END
GO

