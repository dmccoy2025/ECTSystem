CREATE TABLE [dbo].[PreviousWeeksECTCases] (
    [WorkFlowId]  TINYINT       NOT NULL,
    [Member_Name] NVARCHAR (50) NOT NULL,
    [CaseId]      VARCHAR (50)  NOT NULL,
    [MemberUnit]  NVARCHAR (50) NOT NULL,
    [description] VARCHAR (50)  NOT NULL,
    [Name]        VARCHAR (50)  NULL,
    [StartDate]   DATETIME      NOT NULL,
    [EndDate]     DATETIME      NULL,
    [Position]    VARCHAR (15)  NOT NULL,
    [Location]    VARCHAR (15)  NOT NULL,
    [TimesMoved]  SMALLINT      NULL,
    [CaseStatus]  VARCHAR (10)  NULL
);
GO

