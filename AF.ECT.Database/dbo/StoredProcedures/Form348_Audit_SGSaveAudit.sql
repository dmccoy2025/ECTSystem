-- =============================================
-- Author:		Eric Kelley
-- Create date: Jun 15 2021
-- Description:	Save SG section of the Audit 
-- =============================================
CREATE PROCEDURE [dbo].[Form348_Audit_SGSaveAudit]
@lodId INTEGER,
@appropriate smallInt,
@dx bit,
@Isupport bit,
@epts bit,
@aggravation bit,
@principle bit,
@other nvarchar(500),
@proof smallInt,
@standard smallInt,
@proofMet smallInt,
@evidence smallInt,
@misconduct smallInt,
@investigation smallInt,
@comment nvarchar(500)
AS



IF (@appropriate = -1)
BEGIN 
	SET @appropriate = null
END
IF (@proof = -1)
BEGIN 
	SET @proof = null
END
IF (@standard = -1)
BEGIN 
	SET @standard = null
END
IF (@investigation = -1)
BEGIN 
	SET @investigation = null
END
IF (@proofMet = -1)
BEGIN 
	SET @proofMet = null
END
IF (@evidence = -1)
BEGIN 
	SET @evidence = null
END
IF (@misconduct = -1)
BEGIN 
	SET @misconduct = null
END

IF NOT EXISTS(Select LOD_ID FROM Form348_Audit WHERE LOD_ID = @lodId)
	BEGIN

	DECLARE 
	@caseId VARCHAR(15),
	@workflow int

	SET @caseId =
			(SELECT case_id
			FROM Form348 
			WHERE lodId = @lodId)
	SET	@workflow = 
			(SELECT workflow
			 FROM Form348
			 WHERE lodId = @lodId)

	INSERT INTO [dbo].[Form348_Audit]
			   ([LOD_ID]
			   ,[CaseID]
			   ,[Workflow]
			   ,[MedicallyAppropriate]
			  ,[SG_DX]
			  ,[SG_ISupport]
			  ,[SG_EPTS]
			  ,[SG_Aggravation]
			  ,[SG_Principles]
			  ,[SG_Other]
			  ,[SG_ProofApplied]
			  ,[SG_CorrectStandard]
			  ,[SG_ProofMet]
			  ,[SG_Evidence]
			  ,[SG_Misconduct]
			  ,[SG_FormalInvestigation]
			   ,[SG_Comment]
			   )
		 VALUES
			   (@lodId ,
			   @caseId,
			   @workflow,
			   @appropriate ,
				@dx,
				@Isupport ,
				@epts ,
				@aggravation ,
				@principle ,
				@other ,
				@proof ,
				@standard ,
				@proofMet ,
				@evidence ,
				@misconduct ,
				@investigation ,
				@comment 
			   )
	END
ELSE
	BEGIN

UPDATE [dbo].[Form348_Audit]
   SET		   [MedicallyAppropriate] = @appropriate
			  ,[SG_DX] = @dx
			  ,[SG_ISupport] = @Isupport
			  ,[SG_EPTS] = @epts
			  ,[SG_Aggravation] = @aggravation
			  ,[SG_Principles] = @principle
			  ,[SG_Other] = @other
			  ,[SG_ProofApplied] =  @proof
			  ,[SG_CorrectStandard] = @standard
			  ,[SG_ProofMet] = @proofMet
			  ,[SG_Evidence] = @evidence
			  ,[SG_Misconduct] = @misconduct
			  ,[SG_FormalInvestigation] = @investigation
			   ,[SG_Comment] = @comment
 WHERE LOD_ID = @lodId


	END

--[dbo].[Form348_Audit_SGSaveAudit] 31390, -1, false, false, false, false, false, '', -1, -1, -1, -1, -1, -1, ''
GO

