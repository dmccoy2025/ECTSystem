-- ============================================================================
-- Author:		Kenneth Barnett
-- Create date: 4/11/2014
-- Description:	Inserts a new record into the core_StatusCodes table. 
-- ============================================================================
-- Modified By:		Ken Barnett
-- Modified Date:	8/7/2015	
-- Description:		Added the @isDisposition parameter. 
-- ============================================================================
-- Modified By:		Evan Morrison
-- Modified Date:	8/10/2017	
-- Description:		statusId now takes in an int
-- ============================================================================
CREATE PROCEDURE [dbo].[utility_workflow_sp_InsertStatusCode]
	 @statusId INT
	,@description varchar(50)
	--,@moduleId tinyint
	,@moduleName varchar(50)
	,@compo char(1)
	--,@groupId tinyint
	,@groupName varchar(50)
	,@isFinal bit
	,@isApproved bit
	,@canAppeal bit
	,@filter varchar(50)
	,@displayOrder tinyint
	,@isCancel bit
	,@isDisposition bit
AS

DECLARE @moduleId tinyint
DECLARE @groupId tinyint

SELECT @moduleId = moduleId FROM core_lkupModule
WHERE moduleName = @moduleName

SELECT @groupId = groupId FROM core_UserGroups
WHERE name = @groupName



IF NOT EXISTS(SELECT * FROM dbo.core_StatusCodes WHERE moduleId = @moduleId AND description LIKE '%'+@description+'%') 
	BEGIN
		SET IDENTITY_INSERT dbo.core_StatusCodes ON
		INSERT INTO [dbo].[core_StatusCodes] ([statusId], [description], [moduleId], [compo], [groupId], 
					[isFinal], [isApproved], [canAppeal], [filter], [displayOrder], [isCancel], [isDisposition]) 
			VALUES (@statusId, @description, @moduleId, @compo, NULLIF(@groupId, 0), 
					@isFinal, @isApproved, @canAppeal, @filter, @displayOrder, @isCancel, @isDisposition)
		SET IDENTITY_INSERT dbo.core_StatusCodes OFF
		
		PRINT 'Inserted new values (' + convert(varchar(4),@statusId) + ', ' + 
										convert(varchar(50),@description) + ', ' +
										convert(varchar(2),@moduleId) + ', ' +
										convert(varchar(2),@compo) + ', ' +
										convert(varchar(3),@groupId) + ', ' +
										convert(varchar(1),@isFinal) + ', ' +
										convert(varchar(1),@isApproved) + ', ' +
										convert(varchar(1),@canAppeal) + ', ' +
										convert(varchar(1),@filter) + ', ' +
										convert(varchar(2),@displayOrder) + ', ' +
										convert(varchar(1),@isCancel) + ', ' +
										convert(varchar(1),@isDisposition) + ') into core_StatusCodes table.'
										
		-- VERIFY NEW STATUS CODE
		SELECT SC.* ,M.moduleName ,U.name AS UserGroup
		FROM core_StatusCodes SC
		INNER JOIN core_lkupModule M ON M.moduleId = SC.moduleId
		LEFT JOIN core_UserGroups U ON U.groupId = SC.groupId
		WHERE M.moduleId = @moduleId
	END
ELSE
	BEGIN
		PRINT 'Record for ' + @description + ' already exists in core_StatusCodes values (' + convert(varchar(4),@statusId) + ', ' + 
										convert(varchar(50),@description) + ', ' +
										convert(varchar(2),@moduleId) + ', ' +
										convert(varchar(2),@compo) + ', ' +
										convert(varchar(3),@groupId) + ', ' +
										convert(varchar(1),@isFinal) + ', ' +
										convert(varchar(1),@isApproved) + ', ' +
										convert(varchar(1),@canAppeal) + ', ' +
										convert(varchar(1),@filter) + ', ' +
										convert(varchar(2),@displayOrder) + ', ' +
										convert(varchar(1),@isCancel) + ', ' +
										convert(varchar(1),@isDisposition) + ')'
		
	END
GO

