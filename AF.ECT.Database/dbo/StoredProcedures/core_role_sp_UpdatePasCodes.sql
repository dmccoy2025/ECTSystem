


CREATE PROCEDURE [dbo].[core_role_sp_UpdatePasCodes]
	@roleId int,
	@xmlData text

AS


SET NOCOUNT ON;

SET XACT_ABORT ON 
BEGIN TRANSACTION

-- delete existing passcodes for this role
DELETE FROM core_UserRolePasCodes WHERE userRoleId = @roleId

DECLARE @hDoc int 

EXEC sp_xml_preparedocument @hDoc OUTPUT, @xmlData

--now insert the ones with permission
INSERT INTO core_UserRolePasCodes (userRoleId, Pascode, Status)
SELECT  @roleId, id,  status FROM OPENXML (@hDoc, '/PasCodeList/PasCode', 1)  
	WITH (id varchar(50), status tinyint)

COMMIT TRANSACTION

--clean up
EXEC sp_xml_removedocument @hDoc
GO

