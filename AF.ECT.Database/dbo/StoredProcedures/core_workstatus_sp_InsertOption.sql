

CREATE PROCEDURE [dbo].[core_workstatus_sp_InsertOption]
	@optionId int,
	@workstatus int,
	@statusOut int,
	@text varchar(100),
	@active bit,
	@sortOrder tinyint,
	@template tinyint,
	@compo int

AS

IF @optionId = 0
BEGIN

INSERT INTO core_WorkStatus_Options
(ws_id, ws_id_out, displayText, active, sortOrder, template, compo)
VALUES
(@workstatus, @statusOut, @text, @active, @sortOrder, @template, @compo)

SELECT @@IDENTITY 

END
ELSE
BEGIN

UPDATE core_WorkStatus_Options
SET ws_id_out = @statusOut, displayText = @text, active = @active, sortOrder = @sortOrder, template = @template, compo = @compo
WHERE wso_id = @optionId

SELECT @optionId

END
GO

