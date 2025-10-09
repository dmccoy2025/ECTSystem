-- ============================================================================
-- Author:		Eric Kelley
-- Create date: Jun 15 2021
-- Description:	Save A1 section of the Audit 
-- ===========================================================================
-- Modified By:		Eric Kelley
-- Modified Date:	Jun 29 2021
-- Description:		Adds [Determination] and [IfNoToC]
-- ============================================================================
-- Modified By:		Eric Kelley
-- Modified Date:	Jul 27 2021
-- Description:		Change [IfNoToC] to [A1_DeterminationNotCorrect]
-- ============================================================================
CREATE PROCEDURE [dbo].[Form348_Audit_A1SaveAudit]
@lodId INTEGER,
@validate INTEGER,
@status bit,
@orders bit,
@epts bit,
@idt bit,
@pcars bit,
@eightYear bit,
@other nvarchar(500),
@lod INTEGER,
@diagnosis INTEGER,
@request INTEGER,
@iOrA INTEGER,
@iOrD INTEGER,
@activites INTEGER,
@determination INTEGER,
@determinationNotCorrect INTEGER,
@comment nvarchar(500)
AS

IF (@validate = -1)
BEGIN 
	SET @validate = null
END
IF (@lod = -1)
BEGIN 
	SET @lod = null
END
IF (@diagnosis = -1)
BEGIN 
	SET @diagnosis = null
END
IF (@request = -1)
BEGIN 
	SET @request = null
END
IF (@iOrA = -1)
BEGIN 
	SET @iOrA = null
END
IF (@iOrD = -1)
BEGIN 
	SET @iOrD = null
END
IF (@determination = -1)
BEGIN 
	SET @determination = null
END
IF (@determinationNotCorrect = -1)
BEGIN 
	SET @determinationNotCorrect = null
END
IF (@activites = -1)
BEGIN 
	SET @activites = null
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
			   ,[Determination]
			   ,[A1_DeterminationNotCorrect]
			   )
		 VALUES
			   (@lodId
			   ,@caseId
			   ,@workflow
			   ,@validate
			   ,@status
			   ,@orders
			   ,@epts
			   ,@idt
			   ,@pcars
			   ,@eightYear
			   ,@other
			   ,@lod
			   ,@diagnosis
			   ,@request
			   ,@iOrA
			   ,@iOrD
			   ,@activites
			   ,@comment
			   ,@determination 
			   ,@determinationNotCorrect
			   )
	END
ELSE
	BEGIN

UPDATE [dbo].[Form348_Audit]
   SET [StatusValidated] = @validate
	  ,[StatusOfMember] = @status
      ,[Orders] = @orders
      ,[A1_EPTS] = @epts
      ,[IDT] = @idt
	  ,PCARS = @pcars
      ,[EightYearRule] = @eightYear
      ,[A1_Other] = @other
      ,[LODInitiation] = @lod
      ,[WrittenDiagnosis] = @diagnosis
      ,[MemberRequest] = @request
      ,[IncurredOrAggravated] = @iOrA
      ,[IllnessOrDisease] =@iOrD
      ,[Activites] =@activites
      ,[A1_Comment] = @comment
	  ,[Determination] = @determination
	  ,[A1_DeterminationNotCorrect] = @determinationNotCorrect
 WHERE LOD_ID = @lodId


	END

	--[dbo].[Form348_Audit_A1SaveAudit] 31390, -1, false, false, false, false, false, false, '', -1, -1, -1, -1, -1, -1, ''
GO

