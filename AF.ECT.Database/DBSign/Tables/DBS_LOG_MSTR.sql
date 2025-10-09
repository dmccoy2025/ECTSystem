CREATE TABLE [DBSign].[DBS_LOG_MSTR] (
    [LOG_NO]           INT           NOT NULL,
    [LOG_DATE]         DATETIME      NULL,
    [SCOPE_ID]         INT           NULL,
    [LOG_TYPE]         INT           NULL,
    [LOG_STATUS]       INT           NULL,
    [LOG_MESG]         VARCHAR (255) NULL,
    [TEMPLATE_ID]      INT           NULL,
    [SIGNER_CERT_ID]   INT           NULL,
    [VERIFIER_CERT_ID] INT           NULL,
    [SIGN_DATE]        DATETIME      NULL,
    [RESOLVED_IND]     VARCHAR (1)   NULL,
    [SIGNATURE]        IMAGE         NULL,
    [LOG_DATE_GMT]     CHAR (1)      NULL,
    [SIGN_DATE_GMT]    CHAR (1)      NULL
);
GO

