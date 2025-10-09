CREATE TABLE [dbo].[core_logError_archive] (
    [logId]      INT           NOT NULL,
    [errorTime]  SMALLDATETIME NOT NULL,
    [userName]   VARCHAR (50)  NULL,
    [page]       VARCHAR (200) NULL,
    [appVersion] VARCHAR (50)  NULL,
    [browser]    VARCHAR (50)  NULL,
    [message]    VARCHAR (MAX) NULL,
    [stackTrace] VARCHAR (MAX) NULL,
    [caller]     VARCHAR (100) NULL,
    [address]    VARCHAR (20)  NULL
);
GO

