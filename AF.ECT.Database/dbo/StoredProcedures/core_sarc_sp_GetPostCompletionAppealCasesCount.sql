
-- ============================================================================
-- Author:		Evan Morrison
-- Create date: 1/31/2017
-- Description:	Returns the number of SARC appeal post completion count
-- ============================================================================
CREATE PROCEDURE [dbo].[core_sarc_sp_GetPostCompletionAppealCasesCount]
	@userId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- Validate input...
	IF (ISNULL(@userId, 0) = 0)
	BEGIN
		SELECT 0
	END

	DECLARE @Results TABLE
	(
		RefId INT,
		CaseId VARCHAR(50),
		MemberSSN VARCHAR(9),
		MemberName NVARCHAR(100),
		MemberUnit NVARCHAR(100),
		Status VARCHAR(50),
		DaysCompleted INT,
		ModuleId INT
	)

	DECLARE @rptView TINYINT

	SELECT	@rptView = viewType
	FROM	vw_Users
	WHERE	userId = @userId

	INSERT	INTO	@Results
			EXEC	core_sarc_sp_PostAppealCompletionSearch NULL, NULL, NULL, @userId, @rptView, '6', NULL, NULL, NULL, 1

	SELECT	COUNT(*)
	FROM	@Results
END
GO

