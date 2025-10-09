CREATE FUNCTION  [dbo].[core_fn_userPascodes]
(	
	-- Add the parameters for the function here
 
	@ChainType varchar(20)  		 
	,@adminUserID int 
	
)
RETURNS  
@pas_codes  TABLE 
(     pas_code varchar(10)  )

AS
		BEGIN

 --Get current role, groupid, and access scope for the administrator 
	   DECLARE @adminCurrentRoleId int,	@adminCurrentGroupId int, @adminCurrentScope tinyint
				,@userPasCode varchar(10)

	  SET @adminCurrentRoleId=NULL 
	  SET @userPasCode=NULL

		SELECT TOP 1
			@adminCurrentRoleId = b.userRoleId, @adminCurrentGroupId = b.groupId
			,@adminCurrentScope = c.accessscope
		FROM 
			core_userroles b
		JOIN
			core_UserGroups c on b.groupId = c.groupId
		WHERE 
			b.userID = @adminUserID   
		AND
			b.active = 1



		IF @adminCurrentRoleId IS NOT NULL 
		 BEGIN		
			SET  @userPasCode=(SELECT TOP 1 pascode FROM core_userRolePascodes where userRoleId=@adminCurrentRoleId)
		 END 
		 

		  
		DECLARE  @userCS int, @userCSC_ID int
		--Fetch the admin cs and csc_id
		IF @userPasCode IS NOT NULL 
			BEGIN 
				SET @userCS=(SELECT CS_ID FROM COMMAND_STRUCT WHERE PAS_CODE=@userPasCode)
			END 
		IF @userCS IS NOT NULL 
			BEGIN 
 				SET @userCSC_ID=(SELECT CSC_ID FROM COMMAND_STRUCT_CHAIN WHERE CS_ID=@userCS AND CHAIN_TYPE=@ChainType)
			END 


	
	--Create User Units 
	IF (@userCSC_ID IS NOT NULL)   
	BEGIN
					DECLARE @lvl INT;
					DECLARE  @userUnits TABLE
							(
									  cs_Id INT NOT NULL , 
									  csc_id int,
									  csc_id_parent int,
									  chain_type varchar(20),
									  LEVEL   INT NOT NULL,
									  pas_code varchar(10)
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

	
		INSERT  into @pas_codes(pas_code)  
			 	SELECT   distinct csChild.PAS_CODE as childPasCode 
					    FROM @userUnits child 
						  INNER JOIN COMMAND_STRUCT  csChild on csChild.CS_ID=child.cs_id
										 		   
		END 
	
	return	 

END --end Function --end Function
GO

