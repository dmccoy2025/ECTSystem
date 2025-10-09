


CREATE procedure [dbo].[imp_CreateUserRoles]

AS


declare ucur cursor for
select username from core_users
where userId > 2

declare @username varchar(100), @groupId int, @userId int, @roleId int

open ucur
fetch next from ucur into @username

while @@fetch_status = 0
begin
	
	set @userId = (select top 1 userId from imp_usermapping where username like @username)
	set @groupId = 3 --default to med tech (per AFRC)
	
	--start at the lowest level of access and move up
	if exists (select grantee from imp_dba_role_privs where grantee like @userName and granted_role like 'PHA_MEDTECH')
		set @groupId = 3;
	else if exists (select grantee from imp_dba_role_privs where grantee like @userName and granted_role like 'PHA_PHAM')
		set @groupId = 4;
	else if exists (select grantee from imp_dba_role_privs where grantee like @userName and granted_role like 'PHA_SENIOR_PHYS')
		set @groupId = 4;
	else if exists (select grantee from imp_dba_role_privs where grantee like @userName and granted_role like 'PHA_CMDR')
		set @groupId = 2;
	else if exists (select grantee from imp_dba_role_privs where grantee like @userName and granted_role like 'PHA_WING_JA')
		set @groupId = 6;
	else if exists (select grantee from imp_dba_role_privs where grantee like @userName and granted_role like 'PHA_WING_CC')
		set @groupId = 5;
	else if exists (select grantee from imp_dba_role_privs where grantee like @userName and granted_role like 'PHA_MPF')
		set @groupId = 13;
	else if exists (select grantee from imp_dba_role_privs where grantee like @userName and granted_role like 'PHA_HQJA')
		set @groupId = 8;
	else if exists (select grantee from imp_dba_role_privs where grantee like @userName and granted_role like 'PHA_HQSG')
		set @groupId = 9;
	else if exists (select grantee from imp_dba_role_privs where grantee like @userName and granted_role like 'PHA_HQSR')
		set @groupId = 9;
	else if exists (select grantee from imp_dba_role_privs where grantee like @userName and granted_role like 'PHA_HQBOARD')
		set @groupId = 7;
	else if exists (select grantee from imp_dba_role_privs where grantee like @userName and granted_role like 'PHA_HQAA')
		set @groupId = 11;
	else if exists (select grantee from imp_dba_role_privs where grantee like @userName and granted_role like 'AD_MAJCOM')
		set @groupId = 13;
	else if exists (select grantee from imp_dba_role_privs where grantee like @userName and granted_role like 'PHA_NONMEDRPT')
		set @groupId = 14;
	
	begin try

		insert into core_userRoles (userId, groupId, status, active) values (@userId, @groupId, 3, 1)
		set @roleId = scope_identity()
		
		update core_users set currentRole = @roleId where userId = @userId
	
	end try
	begin catch
	
		DECLARE  @msg varchar(2000)
		DECLARE @number int ,@errmsg varchar(2000)
		SELECT  @number= ERROR_NUMBER(), @errmsg= ERROR_MESSAGE() 
		 
		print '** Error creating user role ' + @username

		EXECUTE imp_InsertErrorRecord @number
		,'IMPORTING USER ROLES','imp_CreateUserRoles ',@USERNAME,null, @errmsg
	
	end catch
	
fetch next from ucur into @username
end

close ucur
deallocate ucur
GO

