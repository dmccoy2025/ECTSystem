CREATE TABLE [dbo].[ASPStateTempApplications] (
    [AppId]   INT        NOT NULL,
    [AppName] CHAR (280) NOT NULL,
    PRIMARY KEY CLUSTERED ([AppId] ASC) WITH (FILLFACTOR = 90)
);
GO

CREATE NONCLUSTERED INDEX [Index_AppName]
    ON [dbo].[ASPStateTempApplications]([AppName] ASC) WITH (FILLFACTOR = 90);
GO

