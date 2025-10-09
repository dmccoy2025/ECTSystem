CREATE TABLE [DBSign].[DBS_TMPL_VERSIONS] (
    [TEMPLATE_ID]            INT            NOT NULL,
    [TEMPLATE_NAME]          VARCHAR (75)   NULL,
    [EFFECTIVE_DATE]         DATETIME       NULL,
    [SIG_TABLE_NAME]         VARCHAR (75)   NULL,
    [SIG_TABLE_OWNER]        VARCHAR (75)   NULL,
    [CERT_ID_COL_NAME]       VARCHAR (75)   NULL,
    [SIGN_DATE_COL_NAME]     VARCHAR (75)   NULL,
    [SIG_COL_NAME]           VARCHAR (75)   NULL,
    [SECURITY_LEVEL_ID]      INT            NULL,
    [JOIN_TEXT]              VARCHAR (8000) NULL,
    [DTBS_FMT_ID]            INT            NULL,
    [SIG_FMT_ID]             INT            NULL,
    [EFFECTIVE_DATE_GMT]     CHAR (1)       NULL,
    [SIGN_DATE_GMT_COL_NAME] VARCHAR (128)  NULL
);
GO

