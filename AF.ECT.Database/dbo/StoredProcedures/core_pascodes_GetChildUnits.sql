 -- =============================================
-- Author:		Nandita Srivastava
-- Create date: Feb20th 2009
-- Description:	 Procedure to return child units needs to be improved for the joins
-- =============================================
--EXEC core_pascodes_GetChildUnits 'FNVV','PHA_ADMIN'
 
CREATE PROCEDURE [dbo].[core_pascodes_GetChildUnits]
	 
	    
	    @parentCS int ,
		@ChainType varchar(20) ,
		@userUnit int,
		@adminUserID int 
		
	     
		  

AS

 
DECLARE @parentCSC_ID AS INT
 
DECLARE  @maxDepth as int; 
SET  @maxDepth = 20; 
 
	 	 
 	--Fetch the parent csc_id
	IF (@parentCS IS NOT NULL)   
		BEGIN
			SET @parentCSC_ID=(SELECT CSC_ID FROM COMMAND_STRUCT_CHAIN WHERE CS_ID=@parentCS AND CHAIN_TYPE=@ChainType)
		END

	  
	IF (@parentCSC_ID IS NOT NULL)   
		BEGIN 
		 
				 WITH  childUnits 
						(CS_ID,CSC_ID, CSC_ID_PARENT, CHAIN_TYPE,[Level]) 
					AS (    
							SELECT   CS_ID ,CSC_ID, CSC_ID_PARENT, CHAIN_TYPE, 0  
							FROM COMMAND_STRUCT_CHAIN 
							WHERE CSC_ID_PARENT=@parentCSC_ID  AND CHAIN_TYPE=@ChainType   and CS_ID is not null
						  UNION ALL 
							SELECT    t1.CS_ID ,t1.CSC_ID, t1.CSC_ID_PARENT, t1.CHAIN_TYPE,childUnits.[Level] + 1   
							FROM COMMAND_STRUCT_CHAIN t1  
							INNER JOIN childUnits ON t1.CSC_ID_PARENT = childUnits.CSC_ID    and childUnits.CHAIN_TYPE=t1.CHAIN_TYPE  and t1.cs_id is not null   
							WHERE       '/' + cast(t1.cs_id as varchar(20)) + '/' NOT LIKE '%/' + CAST(childUnits.cs_id AS VARCHAR(MAX)) + '/%'
						  	AND childUnits.[Level] + 1 < @maxDepth	 		
					 ) 
					
						SELECT  child.cs_id,cs.LONG_NAME as childName,cs.PAS_CODE as childPasCode, t.CS_ID  as parentCS_ID,child.CHAIN_TYPE,CHILD.Level,
						NULL as userUnit,cs.Inactive	FROM childUnits child 	
					    INNER JOIN COMMAND_STRUCT_CHAIN  t	on t.CSC_ID=child.CSC_ID_PARENT
						INNER JOIN COMMAND_STRUCT  cs 	on cs.CS_ID=child.cs_id
						 
						ORDER BY [Level] option (maxrecursion 0)  
				   

			END
GO

