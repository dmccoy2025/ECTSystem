CREATE TABLE [DBSign].[DBS_OCSP_CA_CERTS] (
    [OCSP_ID]     INT            NOT NULL,
    [SCOPE_ID]    INT            NOT NULL,
    [CA_CERT_ID]  INT            NOT NULL,
    [DESCRIPTION] VARCHAR (8000) NULL
);
GO

