



-- ============================================================================
-- Modified By:		Darel Johnson
-- Modified Date:	01/15/2020
-- Description:		Added workCompo to SELECT statement and one more NULL arg to 
--                  the EXEC core_user_sp_GetManagedUsers call.
-- ============================================================================
CREATE PROCEDURE [dbo].[sp_core_user_sp_GetPendingCount]
	@userId int

AS

--DECLARE @userId int
--SET @userId = 1

DECLARE @results TABLE
(
	id int,
	status int,
	lastFour char(4),
	firstname varchar(50),
	lastname varchar(50),
	rank varchar(20),
	grade varchar(4),
	expirationDate varchar(11),
	rolename varchar(40),
	currentunitname varchar(100),
	accessstatustext varchar(40),
	username varchar(100),
	workCompo char(1),
	groupId int 
)

INSERT @results (id,status,lastFour,firstname,lastname,rank,grade,expirationDate,rolename,currentunitname,accessstatustext, username,workCompo,groupId)
EXEC core_user_sp_GetManagedUsers @userId, NULL, NULL, 2, NULL, NULL, NULL 

SELECT count(*) FROM @results
GO

