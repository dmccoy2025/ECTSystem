
CREATE procedure [dbo].[imp_create_user]
	@inputname nvarchar(200)
	,@newId int out
	
AS

set nocount on

if exists (select USERid from imp_USERMAPPING where USERNAME like @inputname)
begin
	set @newId = (select userid from imp_USERMAPPING where USERNAME like @inputname)
	return;
end
	

set @inputname = @inputname + ' ';

--split up the name (if needed)
declare @firstName as nvarchar(100), @lastName as nvarchar(100)

select
	@lastName = SUBSTRING(@inputname, 0, charindex(' ', @inputname))
	,@firstName = LTRIM(substring(@inputname, charindex(' ',@inputname, 2) + 1,
		CHARINDEX(' ', LTRIM(substring(@inputname, charindex(' ',@inputname,2),len(@inputname))))))


declare @username as nvarchar(100), @userId as int, @roleId as int

declare @tmpnames table
(
	username nvarchar(100)
);

insert into @tmpnames
exec core_user_sp_getusername @firstName, @lastName

set @username = (select top 1 username from @tmpnames)


--now insert them into the users table
INSERT INTO core_Users
		(USERNAME,FirstName,LastName,MiddleName
		,workCompo,accessStatus,receiveEmail,expirationDate
		,COMMENT,lastAccessDate,rank_code
		,created_date,modified_date,cs_id
		,DisabledDate) 
VALUES 
(
	 @username
	,@FirstName
	,@LastName
	,'' --middlename
	,'6' --compo
	,4 --disabled
	,0 --email
	,DATEADD(year,-1, getdate()) --expiration
	,'Account created by ALOD import for username: ' + @inputname --comment
	,'2005-01-01' --access data
	,99 --unknown rank
	,GETUTCDATE() --created date
	,GETUTCDATE() --modified date
	,11 --unit id (unknown unit)
	,GETUTCDATE() --disabled date
)
 
set @userId = scope_identity()

--now create a userrole, default to med-tech
insert into core_userRoles (userId, groupId, status, active) values (@userId, 3, 3, 1)
set @roleId = scope_identity()

update core_users set currentRole = @roleId where userId = @userId

--finally insert into the usermapping table 
INSERT INTO imp_USERMAPPING 
	(USERID,USERNAME )
VALUES 
	(@userid, @inputname )
	
set @newId = @userId;
GO

