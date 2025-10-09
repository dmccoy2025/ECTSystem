
 -- =============================================
-- Author:		Nandita Srivastava
-- Create date: Feb20th 2009
-- Description:	 Procedure to update reporting information reagarding the   pascode
-- =============================================

CREATE PROCEDURE [dbo].[core_pascode_sp_UpdateReporting]
	@userId int,		 
	@reporting nText 

AS

DECLARE @csId as int, @userName as nvarchar(100);

set @userName = (select userName from core_Users where userID = @userId);

DECLARE @newReporting TABLE 
( 
	cs_id int, 
	chain_type nvarchar(20),
	parent_cs_id int ,
	parent_csc_id int
)

DECLARE @hDoc int 
EXEC sp_xml_preparedocument @hDoc OUTPUT, @reporting

--Read the new reporting structure in a table
INSERT INTO @newReporting(cs_id,chain_type,parent_cs_id) 
SELECT cs_id,chain_type,parent_cs_id FROM OPENXML (@hdoc, '/ReportList/command',1)  
WITH (cs_id int, chain_type nvarchar(20), parent_cs_id int)

set @csId=(select top 1 cs_id from @newReporting)

SET XACT_ABORT ON
BEGIN TRANSACTION

UPDATE 
	@newReporting    
SET  
	parent_csc_id=t2.csc_id	
	  
FROM 
	@newReporting a
join 
	command_struct_chain AS t2 		
		on t2.CS_ID = a.parent_cs_id 
		and t2.CHAIN_TYPE like a.chain_type
	
WHERE 
	parent_cs_id=t2.cs_id;



UPDATE 
	command_struct_chain   
SET 
	csc_id_parent=t2.parent_csc_id
	,MODIFIED_BY = @userName
	,MODIFIED_DATE = GETUTCDATE()	
	,view_type =v.id	 
	,UserModified=1 
FROM 
	@newReporting AS t2 
	join core_lkupChainType as v on v.name=t2.CHAIN_TYPE 
	
WHERE 
	command_struct_chain.cs_id=t2.cs_id
		
AND 
	command_struct_chain.chain_type like t2.chain_type

insert into 
	command_struct_chain(cs_id,chain_type,csc_id_parent, MODIFIED_BY, MODIFIED_DATE,CREATED_BY ,CREATED_DATE ,view_type,UserModified )
select
	a.cs_id, a.chain_type, a.parent_csc_id, @userName, GETUTCDATE(),@userName, GETUTCDATE(),v.id,1
from 
	@newReporting a 
	join core_lkupChainType as v on v.name= a.CHAIN_TYPE 	
where 
	a.chain_type not in 
		(select chain_type from command_struct_chain where cs_id=@csId)
	

EXEC sp_xml_removedocument @hDoc

COMMIT TRANSACTION
GO

