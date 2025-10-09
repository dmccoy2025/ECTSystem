CREATE TABLE [DBSign].[SIG_FORM348_AP_FINDINGS] (
    [appeal_id]         INT      NOT NULL,
    [ptype]             SMALLINT NOT NULL,
    [DBS_CERT_ID]       INT      NOT NULL,
    [DBS_SIGN_DATE]     DATETIME NOT NULL,
    [DBS_SIGN_DATE_GMT] CHAR (1) NOT NULL,
    [DBS_SIGNATURE]     IMAGE    NOT NULL
);
GO

