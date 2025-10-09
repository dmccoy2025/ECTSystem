CREATE TABLE [dbo].[imp_DBA_USERS] (
    [USERNAME]                    VARCHAR (50) NULL,
    [USER_ID]                     VARCHAR (50) NULL,
    [PASSWORD]                    VARCHAR (50) NULL,
    [ACCOUNT_STATUS]              VARCHAR (50) NULL,
    [LOCK_DATE]                   VARCHAR (50) NULL,
    [EXPIRY_DATE]                 VARCHAR (50) NULL,
    [DEFAULT_TABLESPACE]          VARCHAR (50) NULL,
    [TEMPORARY_TABLESPACE]        VARCHAR (50) NULL,
    [CREATED]                     VARCHAR (50) NULL,
    [PROFILE]                     VARCHAR (50) NULL,
    [INITIAL_RSRC_CONSUMER_GROUP] VARCHAR (50) NULL,
    [EXTERNAL_NAME]               VARCHAR (50) NULL
);
GO

CREATE NONCLUSTERED INDEX [IX_USERNAME]
    ON [dbo].[imp_DBA_USERS]([USERNAME] ASC) WITH (FILLFACTOR = 90);
GO

