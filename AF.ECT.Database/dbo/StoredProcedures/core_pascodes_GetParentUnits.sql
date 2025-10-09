 -- =============================================
-- Author:		Nandita Srivastava
-- Create date: Feb20th 2009
-- Description:	 Procedure to return child units needs to be improved for the joins
-- =============================================
  --EXEC core_pascodes_GetParentUnits  'FHLX','PHA_REPORT','FHLX',4
 CREATE PROCEDURE [dbo].[core_pascodes_GetParentUnits]
	 
	    @childPasCode varchar(10) ,
		@ChainType varchar(20)  
		 
	 
		

AS

		
 
	DECLARE @childCS  int, @childCSC_ID int 
	--Fetch the parent cs
	SET @childCS=(SELECT CS_ID FROM COMMAND_STRUCT WHERE PAS_CODE=@childPasCode)

 	--Fetch the parent csc_id
	IF (@childCS IS NOT NULL)   
		BEGIN
			SET @childCSC_ID=(SELECT CSC_ID FROM COMMAND_STRUCT_CHAIN WHERE CS_ID=@childCS AND CHAIN_TYPE=@ChainType)
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
				  chain_type varchar(20),
				  LEVEL   INT NOT NULL
		)  

	    DECLARE @lvl INT;
		SET @lvl = 0;   

		 INSERT INTO @parentUnits(cs_Id,csc_id ,csc_id_parent,chain_type,LEVEL)
		 SELECT   DISTINCT (CS_ID ),CSC_ID, CSC_ID_PARENT, CHAIN_TYPE, 0  
									FROM COMMAND_STRUCT_CHAIN 
									WHERE csc_id=@childCSC_ID  AND CHAIN_TYPE=@ChainType	AND Cs_ID is not null	
		 

			  WHILE @@rowcount > 0         

				  BEGIN
				  
							  SET @lvl = @lvl + 1; 

							  INSERT INTO @parentUnits(cs_Id,csc_id ,csc_id_parent,chain_type,LEVEL)
							  SELECT DISTINCT (C.CS_ID) , C.CSC_ID,C.CSC_ID_PARENT,C.CHAIN_TYPE,@lvl
							  FROM @parentUnits  S   
							  INNER JOIN dbo.COMMAND_STRUCT_CHAIN  C 
							  ON S.LEVEL = @lvl - 1  AND C.CSC_ID = S.csc_id_parent AND C.CHAIN_TYPE=@ChainType
							  WHERE C.CS_ID NOT IN (SELECT cs_Id FROM @parentUnits)
					END



				   SELECT  parent.cs_id,csParent.LONG_NAME as ParentName,csParent.PAS_CODE as parentPasCode,  parent.CHAIN_TYPE,parent.Level,
						NULL as userUnit
						FROM @parentUnits parent 
						 	 INNER JOIN COMMAND_STRUCT  csParent	on csParent.CS_ID=parent.cs_id
						 

		  
 			END
GO

