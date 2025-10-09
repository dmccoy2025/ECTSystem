CREATE TABLE [dbo].[core_LogPageGenerationTime] (
    [gId]           INT           IDENTITY (1, 1) NOT NULL,
    [action_date]   SMALLDATETIME NULL,
    [measuredTime]  VARCHAR (255) NULL,
    [currentPage]   VARCHAR (255) NULL,
    [referringPage] VARCHAR (255) NULL,
    [username]      VARCHAR (200) NULL,
    [role]          VARCHAR (200) NULL,
    PRIMARY KEY CLUSTERED ([gId] ASC)
);
GO

