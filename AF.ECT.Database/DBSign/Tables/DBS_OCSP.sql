CREATE TABLE [DBSign].[DBS_OCSP] (
    [OCSP_ID]           INT            NOT NULL,
    [SCOPE_ID]          INT            NOT NULL,
    [DESCRIPTION]       VARCHAR (8000) NULL,
    [URL]               VARCHAR (8000) NOT NULL,
    [RESPONDER_CERT_ID] INT            NOT NULL,
    [TIME_TOLERANCE]    INT            NOT NULL,
    [SEARCH_PRIORITY]   INT            NOT NULL,
    [DISABLE_NONCES]    CHAR (1)       NOT NULL,
    [OCSP_PROFILE]      INT            NULL
);
GO

