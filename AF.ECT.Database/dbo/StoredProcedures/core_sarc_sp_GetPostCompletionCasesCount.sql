
-- ============================================================================
-- Author:      Ken Barnett
-- Create date: 12/1/2016
-- Description: Returns the number of Restricted SARC cases in their Post
--              Completion step that can be worked on by the specified user.
-- ============================================================================
CREATE PROCEDURE [dbo].[core_sarc_sp_GetPostCompletionCasesCount]
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

    DECLARE @rptView TINYINT, @compo TINYINT

    SELECT  @rptView = viewType, @compo = compo
    FROM    vw_Users
    WHERE   userId = @userId

    INSERT  INTO    @Results
            EXEC    core_sarc_sp_PostCompletionSearch @userId, NULL, NULL, NULL, @rptView, @compo, NULL

    SELECT  COUNT(*)
    FROM    @Results
END
GO

