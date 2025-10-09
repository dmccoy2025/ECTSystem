
/*
NEEDED FOR USER IMPORT
 [imp_dba_role_privs]
 [imp_DBA_USERS]
 [imp_gradeLookUp]
 [IMP_lkupAccessStatus]
 [dbo].[imp_manpower]
 [imp_non_unit_PERSONNEL]
 [imp_PERMMAPPING]
 [imp_PERSONNEL]
 [imp_PERSONNEL_FEED]
 
CREATES TABLE imp_USERMAPPING,IMP_USERROLES

*/
--Execute imp_BeginALL
CREATE PROCEDURE [dbo].[imp_BeginUserDataImport]
 
AS
BEGIN 

   DELETE FROM FORM348_UNIT
	DELETE FROM FORM348_MEDICAL
	DELETE FROM  FORM348_findings 
	DELETE FROM FORM261
    DELETE FROM RWOA
	DELETE FROM core_WorkStatus_Tracking
		DELETE FROM imp_LODMAPPING
	DELETE FROM FORM348 
    DELETE FROM import_Error_LOG
    DELETE FROM imp_USERMAPPING
    DELETE from core_LogAction
    UPDATE CORE_USERS SET CURReNTROLE=NULL
    DELETE from IMP_USERROLES  
    
 
	print ('Starting Users import at :' +cast(getUTCDate() as varchar(50)))
	EXECUTE imp_CreateUsers
 	print ('Starting Users role import at :' +cast(getUTCDate() as varchar(50)))
	EXECUTE imp_CreateUserRoles
	print ('Import completed at :' +cast(getUTCDate() as varchar(50)))
		
END
GO

