
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 4/26/2017
-- Description:	Insert Signature for case 
-- ============================================================================
CREATE PROCEDURE [dbo].[core_Signature_sp_Insert]
	@refid INT,
	@workflow INT,
	@signature VARCHAR (MAX),
	@sig_date VARCHAR (MAX),
	@userId INT,
	@ptype INT
AS
BEGIN

	INSERT core_SignOnly_Signatures (refId, workflow, signature, sig_date, user_id, ptype)
	VALUES (@refid, @workflow, @signature, @sig_date, @userId, @ptype)
	
END
GO

