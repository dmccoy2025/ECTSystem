Create Procedure [usp].[GetSignedECTCasesByUserGroup]
AS
-- ============================================================================
-- Author:		Gary Colbert
-- Create date: 2/17/2021
-- Description:	Get signed ECT cases by UserGroup betwen dates
-- ============================================================================
select caseId, core_lkupModule.moduleName 'Case Type', actionDate 'Date Signed', core_Users.LastName, core_Users.FirstName, core_UserGroups.name, 
		
core_lkupAction.actionName from vw_UserTracking
INNER JOIN core_lkupModule ON vw_UserTracking.moduleId = core_lkupModule.moduleId
Inner join core_Users on core_Users.userID=vw_UserTracking.userId
Inner join core_UserRoles on core_UserRoles.userID=vw_UserTracking.userId
Inner join core_lkupAction on core_lkupAction.actionId=vw_UserTracking.actionId
Inner join core_UserGroups on core_UserGroups.groupId=core_UserRoles.groupId
--inner join core_WorkStatus ws on ws
where core_UserRoles.groupId in ('92', '7', '88', '9') and core_UserRoles.active = '1' and core_Users.workCompo = 6 and actionDate between '2018-01-20 09:00:00.001' and '2021-01-21 09:00:00.000' and vw_UserTracking.actionName = 'signed'  
order by [Date Signed] asc


--select * from core_UserGroups

 --select * from core_Users where username like 'greene%'

 select count (core_lkupAction.actionName) 'Total', core_UserGroups.name from vw_UserTracking
INNER JOIN core_lkupModule ON vw_UserTracking.moduleId = core_lkupModule.moduleId
Inner join core_Users on core_Users.userID=vw_UserTracking.userId
Inner join core_UserRoles on core_UserRoles.userID=vw_UserTracking.userId
Inner join core_lkupAction on core_lkupAction.actionId=vw_UserTracking.actionId
Inner join core_UserGroups on core_UserGroups.groupId=core_UserRoles.groupId
where core_UserRoles.groupId in ('92', '7', '88', '9') and core_UserRoles.active = '1' and core_Users.workCompo = 6 and actionDate between '2018-01-20 09:00:00.001' and '2021-01-21 09:00:00.000' and vw_UserTracking.actionName = 'signed'-- and name = 'Medical  Officer'
group by core_UserGroups.name
--select * from core_UserRoles
--where userID in ('12815', '12827')

--Select *
--from vw_UserTracking
GO

