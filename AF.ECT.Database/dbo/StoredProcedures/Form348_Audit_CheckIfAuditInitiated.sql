-- =============================================
-- Author:		Eric Kelley
-- Create date: Jun 15 2021
-- Description:	Check If Audit Initiated 
-- =============================================
CREATE PROCEDURE [dbo].[Form348_Audit_CheckIfAuditInitiated]
@lodId int
AS
DECLARE
@initiated bit

IF NOT EXISTS(Select LOD_ID FROM Form348_Audit WHERE LOD_ID = @lodId)
BEGIN
	SET @initiated = 0
End

ELSE
BEGIN
	SET @initiated = 1
END

Select @initiated

--[dbo].[Form348_Audit_CheckIfAuditInitiated] 33253
GO

