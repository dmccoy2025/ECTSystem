
CREATE procedure [dbo].[core_user_sp_CopyAccount]
	@copyFromId int,
	@userId int
	
as

declare @newId int

--get the new username
declare @lastName varchar(50), @firstName varchar(50)

select @lastName = lastName, @firstName = firstName 
from core_Users where userID = @copyFromId

declare @userName table
(
	name varchar(100)
)

insert @userName (name)
exec core_user_sp_GetUserName @firstName, @lastName;

declare @newUserName varchar(100)
set @newUserName = (select top 1 name from @userName)

set xact_abort on
begin transaction 

--create the user account
insert into core_Users 
	([EDIPIN], [username], [LastName],[FirstName],[MiddleName], [Title]
	,[SSN], [DateOfBirth], [Phone], [DSN], [Email], [workCompo],[accessStatus]
	,[receiveEmail],[expirationDate], [comment], [lastAccessDate], [rank_code]
	,[work_street],[work_city], [work_state],[work_zip], [work_country]
	,[created_date], [modified_date], [modified_by], [cs_id],[ada_cs_id]
	,[currentRole], [DisabledDate], [Email2], [Email3])
select 
	[EDIPIN], @newUserName, [LastName],[FirstName],[MiddleName], [Title]
	,[SSN], [DateOfBirth], [Phone], [DSN], [Email], [workCompo],[accessStatus]
	,[receiveEmail],[expirationDate], '', NULL, [rank_code]
	,[work_street],[work_city], [work_state],[work_zip], [work_country]
	,GETDATE(), GETDATE(), @userId, [cs_id],[ada_cs_id]
	,NULL, [DisabledDate], [Email2], [Email3]
from 
	core_Users
where
	userID = @copyFromId
	
set @newId = SCOPE_IDENTITY()

--create the role

--if the user only has one role, copy that
declare @newRole int

--copy the role we have for this user
insert into core_UserRoles 
	(groupId, userID, [status], active)
select top 1 
	groupId, @newId, [status], active 
from 
	core_UserRoles
where 
	userID = @copyFromId
order by active desc 

--update the account
set @newRole = SCOPE_IDENTITY();
update core_Users set currentRole = @newRole where userID = @newId;

commit transaction

select @newId
GO

