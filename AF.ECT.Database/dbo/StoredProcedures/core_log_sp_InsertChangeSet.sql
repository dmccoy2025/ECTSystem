-- =============================================
-- Author:		Nick McQuillen
-- Create date: 4/14/2008
-- Description:	Insert multi rows in changeSet table
-- =============================================
CREATE PROCEDURE [dbo].[core_log_sp_InsertChangeSet]
	@changerows as nText

AS

--Prepare input values as an XML document
DECLARE @hDoc int 
EXEC sp_xml_preparedocument @hDoc OUTPUT, @changerows

-- Rollback the transaction if there were any errors
	INSERT INTO 
		core_LogChangeSet(logId,section,field,old,new) 
	SELECT  
		ID,section,field,oldVal,newVal 
	FROM 
		OPENXML (@hdoc, '/XML_Array/XMLList',1)  
	WITH   
		(ID varchar(20),section varchar(50),field varchar(50), 
		oldVal varchar(1000),newVal varchar(1000))

	EXEC sp_xml_removedocument @hDoc
GO

