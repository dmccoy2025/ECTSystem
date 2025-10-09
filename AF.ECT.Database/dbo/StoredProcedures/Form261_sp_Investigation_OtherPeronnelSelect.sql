-- =============================================
-- Author:		Nandita Srivastava
-- Create date: June 9  2008
-- Description:	Selects the Lod Investigation Other Personnel Information 
-- =============================================


CREATE PROCEDURE [dbo].[Form261_sp_Investigation_OtherPeronnelSelect]
		@lodid int
AS

BEGIN

	Declare @persons xml
	SET  @persons=(SELECT otherPersonnels FROM  Form261 WHERE   lodId=@lodid )
 	 --Prepare input values as an XML documnet
	DECLARE @hDoc INT 
	EXEC sp_xml_preparedocument @hDoc OUTPUT, @persons
	
	SELECT ssn,name,grade,compo,investigationDone FROM OPENXML (@hdoc, '/Personnel_List/Personnel',1)  
	WITH   (ssn varchar(9),name varchar(100),grade varchar(6),compo char(1),investigationDone char(1))
	
-- Rollback the transaction if there were any errors
 	EXEC sp_xml_removedocument @hDoc

END
GO

