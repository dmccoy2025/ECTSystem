

CREATE PROCEDURE [dbo].[lod_sp_Initiate]
	@userId int,
	@workflow tinyint,
	@memberName nvarchar(100),
	@memberSSN char(9),
	@memberGrade nvarchar(4),
	@memberUnit nvarchar(100),
	@memberUnitId int,
	@memberDoB datetime,
	@memberCompo char(1)

AS

--get our starting status
DECLARE @status tinyint, @refId int

SET @refId = 0

--see if we have a status override
SET @status = (SELECT statusId FROM core_WorkflowInitStatus WHERE workflowId = @workflow AND groupId = (
			SELECT groupId FROM vw_users WHERE userID = @userId))

--if not use the default
IF (@status IS NULL)
	SET @status = (SELECT initialStatus FROM core_Workflow WHERE workflowId = @workflow)

-- insert into the primary table
SET XACT_ABORT ON

BEGIN TRANSACTION

INSERT INTO FORM348
	(STATUS, WORKFLOW, MEMBER_NAME, MEMBER_SSN, MEMBER_GRADE, MEMBER_UNIT, MEMBER_UNIT_ID,
	MEMBER_DOB, MEMBER_COMPO, CREATED_BY, CREATED_DATE, MODIFIED_BY, MODIFIED_DATE)
VALUES
	(@status, @workflow, @memberName, @memberSSN, @memberGrade, @memberUnit, @memberUnitId,
	@memberDoB, @memberCompo, @userId, getdate(), @userId, getdate())


SET @refId = SCOPE_IDENTITY()

--create secondary table entries
INSERT INTO Form348_Medical (LODID) VALUES (@refId)
INSERT INTO Form348_Unit (LODID) VALUES (@refId)

COMMIT TRANSACTION

SELECT @refId
GO

