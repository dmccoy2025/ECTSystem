 -- =============================================
-- Author:		Nandita Srivastava
-- Create date: Feb20th 2009
-- Description:	 Procedure to return child units needs to be improved for the joins with pagination
-- =============================================
 --EXEC core_pascodes_GetChildUnits_Admin_OLD 'FHLX','PHA_REPORT','FHLX'
 --EXEC core_pascodes_GetChildUnits_Admin  'FHLX','PHA_REPORT','FHLX'
-- EXEC core_pascodes_GetChildUnits_Admin  'FNVV','PHA_ADMIN','FHLX'
 --EXEC core_pascodes_GetChildUnits_Admin  'FNVV','PHA_ADMIN','FHLX'
CREATE PROCEDURE [dbo].[core_pascodes_GetAllUserPasCodes_pagination]
		@ChainType varchar(20)  		 
		,@adminUserID int,
		@PageNumber INT = 1,
		@PageSize INT = 10

AS
	
		
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
						SELECT  child.cs_id,csChild.LONG_NAME as childName,csChild.PAS_CODE as childPasCode, t.CS_ID  as parentCS_ID,child.CHAIN_TYPE,CHILD.Level
					    FROM @userUnits child 
						 INNER JOIN COMMAND_STRUCT_CHAIN  t	on t.CSC_ID=child.CSC_ID_PARENT
						 INNER JOIN COMMAND_STRUCT  csChild	on csChild.CS_ID=child.cs_id
						ORDER BY child.cs_id
						OFFSET (@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
					   

	END
GO