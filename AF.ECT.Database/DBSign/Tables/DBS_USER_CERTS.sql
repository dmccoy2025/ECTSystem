CREATE TABLE [DBSign].[DBS_USER_CERTS] (
    [USER_ID]           INT           NOT NULL,
    [SCOPE_ID]          INT           NOT NULL,
    [CERT_ID]           INT           NOT NULL,
    [SECURITY_LEVEL_ID] INT           NULL,
    [CERT_DESCRIPTION]  VARCHAR (255) NULL,
    [NOTARY_FLG]        CHAR (1)      NULL
);
GO

