CREATE TABLE [dbo].[core_LogError] (
    [logId]      INT           IDENTITY (1, 1) NOT NULL,
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

ALTER TABLE [dbo].[core_LogError]
    ADD CONSTRAINT [DF_core_ErrorLog_errorTime1] DEFAULT (getdate()) FOR [errorTime];
GO

