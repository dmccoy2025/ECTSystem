-- =============================================
-- Author:		Eric Kelley
-- Create date: Jun 15 2021
-- Description:	Retrieve Audit data 
-- =============================================
CREATE PROCEDURE [dbo].[Form348_Audit_GetAuditInfo]
@lodId	int
AS

SELECT			[LOD_ID]
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
      ,[StatusValidated]
      ,[StatusOfMember]
      ,[Orders]
      ,[A1_EPTS]
      ,[IDT]
	  ,PCARS
      ,[EightYearRule]
      ,[A1_Other]
      ,[LODInitiation]
      ,[WrittenDiagnosis]
      ,[MemberRequest]
      ,[IncurredOrAggravated]
      ,[IllnessOrDisease]
      ,[Activites]
      ,[A1_Comment]
      ,[JA_Comment]
      ,[SG_Comment]
	  ,[Determination]
      ,[A1_DeterminationNotCorrect]
FROM Form348_Audit
Where LOD_ID = @lodId

--[dbo].[Form348_Audit_GetAuditInfo] 31390
GO

