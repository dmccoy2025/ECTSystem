 -- =============================================
-- Author:		Nandita Srivastava
-- Create date: Feb20th 2009
-- Description:	 Procedure to return child units needs to be improved for the joins
-- =============================================
  --EXEC core_pascodes_GetParentChain   20151543,3
 CREATE PROCEDURE [dbo].[core_pascodes_GetParentChain]
	 
	    @childCS int,
		@ViewType int  
		 
	 
		

AS
 

		
 
	DECLARE  @childCSC_ID int 
 

 	--Fetch the parent csc_id
	IF (@childCS IS NOT NULL)   
		BEGIN
			SET @childCSC_ID=(SELECT CSC_ID FROM COMMAND_STRUCT_CHAIN WHERE CS_ID=@childCS AND view_type =@ViewType)
		END

	
	--IF OBJECT_ID('dbo.childUnits') IS NOT NULL DROP TABLE dbo.childUnits
	--IF OBJECT_ID('dbo.userUnits') IS NOT NULL DROP TABLE dbo.userUnits
	
	IF (@childCSC_ID IS NOT NULL)   
  	
	BEGIN 
	--Create child Units 
		DECLARE  @parentUnits TABLE
		(
				  cs_Id INT NOT NULL , 
				  csc_id int,
				  csc_id_parent int,
				  view_type int,
				  LEVEL   INT NOT NULL
		)  

	    DECLARE @lvl INT;
		SET @lvl = 0;   

		 INSERT INTO @parentUnits(cs_Id,csc_id ,csc_id_parent,view_type,LEVEL)
		 SELECT   DISTINCT (CS_ID ),CSC_ID, CSC_ID_PARENT, view_type , 0  
									FROM COMMAND_STRUCT_CHAIN 
									WHERE csc_id=@childCSC_ID  AND view_type=@viewType	AND Cs_ID is not null	
		 

			  WHILE @@rowcount > 0         

				  BEGIN
				  
							  SET @lvl = @lvl + 1; 

							  INSERT INTO @parentUnits(cs_Id,csc_id ,csc_id_parent,view_type,LEVEL)
							  SELECT DISTINCT (C.CS_ID) , C.CSC_ID,C.CSC_ID_PARENT,C.view_type,@lvl
							  FROM @parentUnits  S   
							  INNER JOIN dbo.COMMAND_STRUCT_CHAIN  C 
							  ON S.LEVEL = @lvl - 1  AND C.CSC_ID = S.csc_id_parent AND C.view_type=@viewType
							  WHERE C.CS_ID NOT IN (SELECT cs_Id FROM @parentUnits)
					END



				   SELECT  parent.cs_id,csParent.LONG_NAME as ParentName,csParent.PAS_CODE as parentPasCode,  parent.view_type,parent.Level,
						NULL as userUnit
						FROM @parentUnits parent 
						 	 INNER JOIN COMMAND_STRUCT  csParent	on csParent.CS_ID=parent.cs_id
						WHERE csParent.CS_ID <> 11 --Omit the unknown units
                        order By LEVEL desc
		  
 			END
GO

