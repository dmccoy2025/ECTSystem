

-- Exec core_lod_sp_GetCompletedRRs 498, 0
CREATE PROCEDURE [dbo].[core_lod_sp_GetCompletedRRs]
	@userId int,
	@sarc bit 
 

AS

--Declare @userId int
--Declare @sarc bit

--SET @userId = 469  -- 469 is system admin, 295 is MPF  , 430/498 is HQ Tech
--SET @sarc = 1

DECLARE @roleId int, @isAdmin int = 0, @groupId int, @scope int, @rptView int, @userUnit int
Select  @roleId = currentRole from core_Users where userID = @userId
Select  @groupId = groupId From core_UserRoles where userRoleID = @roleId
SELECT  @userUnit = unit_id, @scope = accessscope, @rptView = ViewType from vw_Users where userId = @userId

If @groupId = 1  -- System Admin
	Set @isAdmin = 1
If @groupId = 7  -- Board Tech
	Set @isAdmin = 1
If @groupId = 8  -- Board Legal
	Set @isAdmin = 1
If @groupId = 9  -- Board Medical
	Set @isAdmin = 1
If @groupId = 11  -- Approving Auth
	Set @isAdmin = 1
If @groupId = 88  -- HQ Tech
	Set @isAdmin = 1

Select distinct request_id
	, SubString(frr.member_ssn, 6, 4) As Protected_SSN
	, lod.member_name As Member_Name
	, lod.member_unit As Unit_Name
	, frr.Case_Id
	, vws.description As Status
	, Convert(char(11), ISNULL(t.ReceiveDate, frr.createddate), 101) As Receive_Date
	, DateDiff(d, ISNULL(t.ReceiveDate, frr.createddate), GetDate()) As Days
From Form348_RR frr
	Inner Join Form348 lod On lod.lodId = frr.InitialLodId
	Inner Join vw_WorkStatus vws On frr.status = vws.ws_id
	LEFT JOIN (
		SELECT Max(startDate) ReceiveDate, ws_id, refId 
		FROM core_WorkStatus_Tracking 
		GROUP BY ws_id, refId) t 
			ON t.refId = frr.request_id AND t.ws_id = frr.status
Where 
		@isAdmin = 1 -- Admin View, See all
		OR
		(
			lod.member_unit_id  IN 				 
				(
					SELECT child_id FROM Command_Struct_Tree WHERE parent_id=@userUnit and view_type=@rptView 
				)			
				AND  vws.ws_id IN (36,39, 88)	
--			Or vws.ws_id = 36  -- Approved REquest (Complete)		
--			Or vws.ws_id = 39  -- Denied Request (Complete)				
--			Or vws.ws_id = 88  -- Cancelled Request (Complete)				
		)
		
		

 -- Select @isAdmin, @roleId, @groupId
GO

