
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 7/6/2017
-- Description:	Add Signature to the core_SignatureMetaData table
-- ============================================================================
CREATE PROCEDURE [dbo].[core_SignatureMetaData_sp_Add]
	@refId INT,
	@workflowId INT,
	@workStatus INT,
	@userGroup TINYINT,
	@userId INT,
	@date DATETIME,
	@NameAndRank VARCHAR(100),
	@Title VARCHAR(100)
AS
BEGIN
	
	EXEC core_SignatureMetaData_sp_Delete @refId, @workflowId, @workStatus

	INSERT INTO core_SignatureMetaData (refId, workflowId, workStatus, userGroup, userId, date, nameAndRank, title)
	VALUES(@refId, @workflowId, @workStatus, @userGroup, @userId, @date, @NameAndRank, @Title)

END
GO

