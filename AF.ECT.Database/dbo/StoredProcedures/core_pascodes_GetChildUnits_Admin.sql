 -- =============================================
-- Author:		Nandita Srivastava
-- Create date: Feb20th 2009
-- Description:	 Procedure to return child units needs to be improved for the joins
-- =============================================
 --EXEC core_pascodes_GetChildUnits_Admin_OLD 'FHLX','PHA_REPORT','FHLX'
 --EXEC core_pascodes_GetChildUnits_Admin  'FHLX','PHA_REPORT','FHLX'
-- EXEC core_pascodes_GetChildUnits_Admin  'FNVV','PHA_ADMIN','FHLX'
 --EXEC core_pascodes_GetChildUnits_Admin  'FNVV','PHA_ADMIN','FHLX'
CREATE PROCEDURE [dbo].[core_pascodes_GetChildUnits_Admin]
	 
	    @parentCS int ,
		@ChainType varchar(20) ,
		@userUnit int,
		@adminUserID int 
	 
		

AS

		
 --Get current role, groupid, and access scope for the administrator 
	   DECLARE @adminCurrentRoleId int,	@adminCurrentGroupId int, @adminCurrentScope tinyint

		DECLARE @roleId int

		SET @roleId = (SELECT TOP 1 userroleId FROM core_userroles WHERE userid = @adminUserID AND active = 1);

		SELECT 
			@adminCurrentRoleId = @roleId, @adminCurrentGroupId = b.groupId
			,@adminCurrentScope = c.accessscope
		FROM 
			core_Users a
		JOIN 
			core_UserRoles b ON b.userRoleID = @roleId
		JOIN
			core_UserGroups c on b.groupId = c.groupId
		WHERE 
			a.userID = @adminUserID   
	
	DECLARE  @userCS int, @userCSC_ID int
	--Fetch the admin cs and csc_id
	SET @userCS=(SELECT CS_ID FROM COMMAND_STRUCT WHERE CS_ID=@userUnit)
 	SET @userCSC_ID=(SELECT CSC_ID FROM COMMAND_STRUCT_CHAIN WHERE CS_ID=@userCS AND CHAIN_TYPE=@ChainType)
	

	DECLARE   @parentCSC_ID int  
 	--Fetch the parent csc_id
	IF (@parentCS IS NOT NULL)   
		BEGIN
			SET @parentCSC_ID=(SELECT CSC_ID FROM COMMAND_STRUCT_CHAIN WHERE CS_ID=@parentCS AND CHAIN_TYPE=@ChainType)
		END

	
	--IF OBJECT_ID('dbo.childUnits') IS NOT NULL DROP TABLE dbo.childUnits
	--IF OBJECT_ID('dbo.userUnits') IS NOT NULL DROP TABLE dbo.userUnits
	
	IF (@parentCSC_ID IS NOT NULL)   
  	
	BEGIN 
	--Create child Units 
		DECLARE  @childUnits TABLE
		(
				  cs_Id INT NOT NULL , 
				  csc_id int,
				  csc_id_parent int,
				  chain_type varchar(20),
				  active  bit,
				  LEVEL   INT NOT NULL
		)  


	

 
	    DECLARE @lvl INT;
		SET @lvl = 0;   

		 INSERT INTO @childUnits(cs_Id,csc_id ,csc_id_parent,chain_type,LEVEL)
		 SELECT   DISTINCT (CS_ID ),CSC_ID, CSC_ID_PARENT, CHAIN_TYPE, 0  
									FROM COMMAND_STRUCT_CHAIN 
									WHERE CSC_ID_PARENT=@parentCSC_ID  AND CHAIN_TYPE=@ChainType	AND Cs_ID is not null	
		 

			  WHILE @@rowcount > 0         

				  BEGIN
				  
							  SET @lvl = @lvl + 1; 

							  INSERT INTO @childUnits(cs_Id,csc_id ,csc_id_parent,chain_type,LEVEL)
							  SELECT DISTINCT (C.CS_ID) , C.CSC_ID,C.CSC_ID_PARENT,C.CHAIN_TYPE,@lvl
							  FROM @childUnits  S   
							  INNER JOIN dbo.COMMAND_STRUCT_CHAIN  C 
							  ON S.LEVEL = @lvl - 1  AND C.CSC_ID_PARENT = S.CSC_ID AND C.CHAIN_TYPE=@ChainType
							  WHERE C.CS_ID NOT IN (SELECT cs_Id FROM @childUnits)
					END
		--End create child units 	

		IF (@adminCurrentScope>=3 ) --If the userscope is less then the compo level then we need to create userunits table else not 
			--Get final resultset
			BEGIN 
						SELECT  child.cs_id,csChild.LONG_NAME as childName,csChild.PAS_CODE as childPasCode, t.CS_ID  as parentCS_ID,child.CHAIN_TYPE,CHILD.Level,
						NULL as userUnit,csChild.Inactive
						FROM @childUnits child 
						 INNER JOIN COMMAND_STRUCT_CHAIN  t	on t.CSC_ID=child.CSC_ID_PARENT
						 INNER JOIN COMMAND_STRUCT  csChild	on csChild.CS_ID=child.cs_id
						 ORDER BY [parentCS_ID]  

			 END 
		ELSE
		    BEGIN
					--Create User Units 
					DECLARE  @userUnits TABLE
							(
									  cs_Id INT NOT NULL , 
									  csc_id int,
									  csc_id_parent int,
									  chain_type varchar(20),
									  LEVEL   INT NOT NULL
							)
					 

					SET @lvl = 0;   

					 INSERT INTO @userUnits (cs_Id,csc_id ,csc_id_parent,chain_type,LEVEL)
					 SELECT   DISTINCT (CS_ID ),CSC_ID, CSC_ID_PARENT, CHAIN_TYPE, 0  
					 FROM COMMAND_STRUCT_CHAIN 
					 WHERE CSC_ID_PARENT=@userCSC_ID  AND CHAIN_TYPE=@ChainType	AND Cs_ID is not null	
					 

					  WHILE @@rowcount > 0         

						  BEGIN
						 
								SET @lvl = @lvl + 1; 

								 INSERT INTO @userUnits(cs_Id,csc_id ,csc_id_parent,chain_type,LEVEL)
								  SELECT DISTINCT (C.CS_ID) , C.CSC_ID,C.CSC_ID_PARENT,C.CHAIN_TYPE,@lvl
								  FROM @userUnits  S   
								  INNER JOIN dbo.COMMAND_STRUCT_CHAIN  C 
								  ON S.LEVEL = @lvl - 1  AND C.CSC_ID_PARENT = S.CSC_ID AND C.CHAIN_TYPE=@ChainType   AND C.Cs_ID is not null	
								  WHERE C.CS_ID NOT IN (SELECT cs_Id FROM @userUnits)
						 END
 
					--Get final resultset
					--The user units is joined just to know which units fall under the pascode
						SELECT  child.cs_id,csChild.LONG_NAME as childName,csChild.PAS_CODE as childPasCode, t.CS_ID  as parentCS_ID,child.CHAIN_TYPE,CHILD.Level,
						t3.cs_Id as userUnit,csChild.Inactive
						FROM @childUnits child 
						 INNER JOIN COMMAND_STRUCT_CHAIN  t	on t.CSC_ID=child.CSC_ID_PARENT
						 INNER JOIN COMMAND_STRUCT  csChild	on csChild.CS_ID=child.cs_id
						 LEFT JOIN @userUnits t3 on t3.cs_id=child.cs_id
	  					ORDER BY  userUnit  DESC  
 			END
	 
	END
GO

