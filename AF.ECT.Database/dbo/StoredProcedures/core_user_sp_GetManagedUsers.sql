
-- ============================================================================
-- Created By:      ?
-- Created Date:    ??/??/??
-- Description:     Returns a list of users that the requesting user mananges.
-- ============================================================================
-- Modified By:     Nandita Srivastva
-- Modified Date:   4/4/2011
-- Description:     Added @IsTestEnvironment argument to see the testers all 
--                  the users with no hirerchy restrictions 
-- ============================================================================
-- Modified By:     Ken Barnett
-- Modified Date:   8/25/2017
-- Description:     o Observed that the @IsTestEnvironment argument mentioned
--                  in Nandita Srivastva's 2011 change is missing. 
--                  o Cleaned up the stored procedure to be more readable.
--                  o Updated to do a wildcard search against the full name
--                  (last first) of the users instead of just the last name.
-- ============================================================================
-- Modified By:     Michael van Diest
-- Modified Date:   11/8/2019
-- Description:     Added workCompo to SELECT statement.
-- ============================================================================
-- Modified By:     Eric Kelley
-- Modified Date:   04/5/2020
-- Description:     Includes the member viewBy results.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_user_sp_GetManagedUsers]
    @userid INT,
    @ssn CHAR(4),
    @name VARCHAR(20),
    @status INT,
    @role INT,
    @srchUnit INT,
    @ShowAllUsers BIT = NULL
AS
BEGIN
	DECLARE @tempManged as TABLE 
	(
		 Id INT NOT NULL, 
         Status TINYINT NOT NULL, 
         LastFour VARCHAR(4) NULL,
         FirstName VARCHAR(50) NOT NULL , 
         LastName VARCHAR(50) NOT NULL, 
         RANK NVARCHAR(50) NULL, 
         GRADE NVARCHAR(50) NULL, 
         ExpirationDate DATETIME NOT NULL, 
         RoleName NVARCHAR(50) NOT NULL, 
         CurrentUnitName NVARCHAR(100) NULL,
         AccessStatusText VARCHAR(50) NOT NULL,
         username VARCHAR(50) NOT NULL,
         workCompo CHAR NOT NULL,
         GroupId TINYINT NOT NULL,
		 ViewOrManaged Int
	)

    IF (LEN(@name) = 0)
        SET @name = NULL
    IF (LEN(@ssn) <> 4)
        SET @ssn = NULL
    IF (@status = 0)
        SET @status = NULL
    IF (@role = 0)
        SET @role = NULL 
    IF (@srchUnit = 0)
        SET @srchUnit = NULL 
    DECLARE @unitId INT,
            @viewType INT,
            @groupId INT,
            @systemAdminGroupId INT = 1,
			@view bit = 0,
			@managed bit = 1
    SELECT @unitId = ISNULL(ada_cs_id, cs_id) FROM core_users WHERE userID = @userId;
    SELECT  TOP 1 @viewType = ViewType, @groupId = groupId
    FROM    vw_users
    WHERE   userid = @userId ;
    SELECT  @systemAdminGroupId = ug.groupId
    FROM    core_UserGroups ug
    WHERE   ug.name = 'System Administrator';
	iNSERT INTO @tempManged(Id,
	Status,LastFour,FirstName,LastName,RANK, GRADE,ExpirationDate,RoleName,CurrentUnitName,AccessStatusText,username,workCompo,GroupId,ViewOrManaged)
   SELECT  
            a1.userId Id, 
            a1.accessStatus Status, 
            RIGHT(a1.SSN, 4) LastFour,
            a1.FirstName, 
            a1.LastName, 
            gr.RANK, 
            gr.GRADE, 
            LEFT(a1.expirationDate,11) AS expirationDate, 
            g.name RoleName, 
            c.LONG_NAME + ' (' + c.PAS_CODE + ')' CurrentUnitName,
            s.description AccessStatusText,
			a1.username, --in (Select username from core_users),
            a1.workCompo,
            r.groupId,
			@managed 'ViewOrManaged' 
    FROM 
            core_users a1
            Left JOIN core_lkupAccessStatus s ON s.statusId = a1.accessStatus
            Left JOIN core_UserRoles r ON r.userid = a1.userID AND r.active = 1
            Left JOIN core_UserGroups g ON g.groupId = r.groupId
            Left JOIN core_lkupGrade gr ON gr.CODE = a1.rank_code
            Left JOIN Command_Struct c ON c.CS_ID = ISNULL(a1.ada_cs_id, a1.cs_id)
    WHERE 
            (
                (
                    @ShowAllUsers = 1
                    AND @groupId = @systemAdminGroupId
                ) 
				
                OR

                ISNULL(a1.ada_cs_id, a1.cs_id) IN (SELECT child_id FROM Command_Struct_Tree WHERE parent_id = @unitId AND view_type = @viewType)
				
            )
            AND 
            (   
                g.groupId IN (SELECT groupId FROM core_UserGroupsManagedBy WHERE managedBy = @groupId) 
                OR
                g.groupId = @groupId

            )
            AND
            (
                @name IS NULL 
                OR (a1.lastname + ' ' + a1.FirstName) LIKE '%' + @name + '%'
            )
            AND
            (
                @ssn IS NULL 
                OR RIGHT(a1.SSN, 4) = @ssn
            ) 
            AND 
            (
                @status IS NULL 
                OR a1.accessStatus = @status
            ) 
            AND
            (
                @role IS NULL 
                OR g.groupId = @role
            )
            AND
            (
                @srchUnit IS NULL 
                OR ISNULL(a1.ada_cs_id, a1.cs_id) = @srchUnit

            )
-------------------------Adds viewBy data----------------------------------------
			UNION 
			    
				(SELECT 
				a.userId Id, 
            a.accessStatus Status, 
            RIGHT(a.SSN, 4) LastFour,
            a.FirstName, 
            a.LastName, 
            gr.RANK, 
            gr.GRADE, 
            LEFT(a.expirationDate,11) AS expirationDate, 
            g.name RoleName, 
            c.LONG_NAME + ' (' + c.PAS_CODE + ')' CurrentUnitName,
            s.description AccessStatusText,
            a.username,
            a.workCompo,
            r.groupId,
			@view 'ViewOrManaged' 
			

    FROM 
            core_users a
             RIGHT JOIN core_lkupAccessStatus s ON s.statusId = a.accessStatus
             RIGHT JOIN core_UserRoles r ON r.userid = a.userID AND r.active = 1
             RIGHT JOIN core_UserGroups g ON g.groupId = r.groupId
             RIGHT JOIN core_lkupGrade gr ON gr.CODE = a.rank_code
             RIGHT JOIN Command_Struct c ON c.CS_ID = ISNULL(a.ada_cs_id, a.cs_id)
			 RIGHT Join core_UserGroupsViewBy v ON v.viewerId = @groupId 
    WHERE 
            (
                (
                    @ShowAllUsers = 1
                    AND @groupId = @systemAdminGroupId
                ) 
			
            	OR 
			
                ISNULL(a.ada_cs_id, a.cs_id) IN (SELECT child_id FROM Command_Struct_Tree WHERE parent_id = @unitId AND view_type = @viewType)

				
				)
			
				AND 
            (   
               
				v.viewerId IN (SELECT viewerId FROM core_UserGroupsViewBy WHERE v.viewerId = @groupId and v.memberId = r.groupId) 
				
				
            )
			
            AND
            (
                @name IS NULL 
                OR (a.lastname + ' ' + a.FirstName) LIKE '%' + @name + '%'
            )
            AND
            (
                @ssn IS NULL 
                OR RIGHT(a.SSN, 4) = @ssn
            ) 
            AND 
            (
                @status IS NULL 
                OR a.accessStatus = @status
            ) 
            AND
            (
                @role IS NULL 
                OR g.groupId = @role
            )
            AND
            (
                @srchUnit IS NULL 
                OR ISNULL(a.ada_cs_id, a.cs_id) = @srchUnit
            )
			
			) 
			
			SELECT Id,Status,LastFour,FirstName,LastName, 
			RANK
			,GRADE
			,expirationDate
			,RoleName
			,CurrentUnitName
			,AccessStatusText
			,username
			,workCompo
			,GroupId 
			,max(ViewOrManaged) ViewOrManaged
			FROM @tempManged
			Group by Id,Status,LastFour,FirstName,LastName
			,RANK
			,GRADE
			,expirationDate
			,RoleName
			,CurrentUnitName
			,AccessStatusText
			,username
			,workCompo
			,GroupId 

	
	


END

--[dbo].[core_user_sp_GetManagedUsers] 4,'' ,'' , 3,0 ,0, 0

/******@userid INT, @ssn CHAR(4), @name VARCHAR(20), @status INT, @role INT, @srchUnit INT, @ShowAllUsers BIT = NULL******/
GO

