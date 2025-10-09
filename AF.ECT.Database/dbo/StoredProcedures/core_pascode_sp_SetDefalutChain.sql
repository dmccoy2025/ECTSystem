
CREATE PROCEDURE [dbo].[core_pascode_sp_SetDefalutChain]
(
	@unitId int,
	@unitPasCode varchar(4) ,
	@userId int
	 
	
)

AS
	SET NOCOUNT ON
--insert command_struct_chain records --for all view types 
   INSERT INTO Command_Struct_Chain 
	  (CS_ID,CHAIN_TYPE,view_type,created_by,created_date) 	
 	select cs.cs_id ,chain.Name,chain.id , @userId,GETUTCDATE() from 
 		core_lkupChainType as chain 
 	   ,Command_Struct as  cs where chain.active =1 
		and   cs.CS_ID   =@unitId 
 		
     UPDATE Command_Struct_chain		
	 SET  CSC_ID_PARENT = P.parent_csc_id  	
	 FROM  
	(   SELECT P1.*,P2.CSC_ID as parent_csc_id   FROM Command_Struct_chain P2 JOIN 
	    ( 
			SELECT t2.PAS_CODE,t1.CSC_ID  as csc_id,t1.cs_id ,t2.CS_ID_PARENT ,
			 t1.CHAIN_TYPE ,t1.view_type		
		    FROM 
		    Command_Struct_Chain t1
		    LEFT OUTER JOIN
		    Command_Struct  t2  on t2.cs_id =t1.CS_ID   
		   WHERE t2.cs_id  =@unitId
		 ) p1 ON p1.CS_ID_PARENT =P2.CS_ID AND P2.view_type =p1.view_type   
	)  P	  
	WHERE 
	Command_Struct_chain.CS_ID=P.CS_ID AND	
	Command_Struct_Chain.view_type =P.view_type  
 	AND P.cs_id  =@unitId
GO

