-- =============================================
-- Author:		Kenneth Barnett
-- Create date: 4/11/2014
-- Description:	Insert a new record into the core_Permissions table. 
-- =============================================
CREATE PROCEDURE [dbo].[utility_workflow_sp_InsertPermission]
	@permId smallint
	,@permName varchar(50)
	,@permDesc varchar(100)
	,@exclude bit
AS

IF NOT EXISTS(SELECT * FROM dbo.core_Permissions WHERE permId = @permId AND permName = @permName)
	BEGIN
		SET IDENTITY_INSERT dbo.core_Permissions ON
		INSERT INTO [dbo].[core_Permissions] ([permId], [permName], [permDesc], [exclude]) 
			VALUES (@permId, @permName, @permDesc, @exclude)
		SET IDENTITY_INSERT dbo.core_Permissions OFF
		
		PRINT 'Inserted new values (' + CONVERT(VARCHAR(3), @permId) + ', ' +
										CONVERT(VARCHAR(50), @permName) + ', ' + 
										CONVERT(VARCHAR(3), @permDesc) + ', ' +
										CAST(@exclude AS CHAR(1)) + ') into core_Permissions table.'
										
		-- VERIFY NEW PERMISSION
		SELECT P.*
		FROM core_Permissions P
		WHERE P.permId = @permId AND P.permName = @permName
	END
ELSE
	BEGIN
		PRINT 'Values already exist in core_Permissions table.'
	END
GO

