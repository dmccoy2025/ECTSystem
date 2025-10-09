-- =============================================
-- Author:		Eric Kelley
-- Create date: Jun 15 2021
-- Description:	Save JA section of the Audit 
-- =============================================
CREATE PROCEDURE [dbo].[Form348_Audit_JASaveAudit]
@lodId INTEGER,
@legal smallInt,
@standardOfProof bit,
@deathAndMVA bit,
@formalPolicy bit,
@aFI bit,
@other nvarchar(500),
@proof smallInt,
@standard smallInt,
@proofMet smallInt,
@evidence smallInt,
@misconduct smallInt,
@investigation smallInt,
@comment nvarchar(500)
AS

IF (@legal = -1)
BEGIN 
	SET @legal = null
END
IF (@proof = -1)
BEGIN 
	SET @proof = null
END
IF (@standard = -1)
BEGIN 
	SET @standard = null
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
IF (@investigation = -1)
BEGIN 
	SET @investigation = null
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
			   ,[LegallySufficient]
			   ,[JA_StandardOfProof]
			  ,[JA_DeathAndMVA]
			  ,[JA_FormalPolicy]
			  ,[JA_AFI]
			  ,[JA_Other]
			  ,[JA_ProofApplied]
			  ,[JA_CorrectStandard]
			  ,[JA_ProofMet]
			  ,[JA_Evidence]
			  ,[JA_Misconduct]
			  ,[JA_FormalInvestigation]
			   ,[JA_Comment]
			   )
		 VALUES
			   (@lodId ,
			   @caseId,
			   @workflow,
			   @legal ,
				@standardOfProof,
				@deathAndMVA ,
				@formalPolicy ,
				@aFI ,
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
   SET		   [LegallySufficient] = @legal
			  ,[JA_StandardOfProof] = @standardOfProof
			  ,[JA_DeathAndMVA] = @deathAndMVA 
			  ,[JA_FormalPolicy] = @formalPolicy
			  ,[JA_AFI] = @aFI
			  ,[JA_Other] = @other
			  ,[JA_ProofApplied] =  @proof
			  ,[JA_CorrectStandard] = @standard
			  ,[JA_ProofMet] = @proofMet
			  ,[JA_Evidence] = @evidence
			  ,[JA_Misconduct] = @misconduct
			  ,[JA_FormalInvestigation] = @investigation
			   ,[JA_Comment] = @comment
 WHERE LOD_ID = @lodId


	END
GO

