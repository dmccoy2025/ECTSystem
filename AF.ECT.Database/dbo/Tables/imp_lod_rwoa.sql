CREATE TABLE [dbo].[imp_lod_rwoa] (
    [RWOA_ID]                      NVARCHAR (50)   NULL,
    [LOD_ID]                       NVARCHAR (50)   NULL,
    [SENT_TO]                      NVARCHAR (50)   NULL,
    [REASON_SENT_BACK]             NVARCHAR (4000) NULL,
    [EXPLANATION_FOR_SENDING_BACK] NVARCHAR (4000) NULL,
    [SENDER]                       NVARCHAR (50)   NULL,
    [DATE_SENT]                    NVARCHAR (50)   NULL,
    [COMMENTS_BACK_TO_SENDER]      NVARCHAR (4000) NULL,
    [DATE_SENT_BACK]               NVARCHAR (50)   NULL,
    [CREATED_BY]                   NVARCHAR (50)   NULL,
    [CREATED_DATE]                 NVARCHAR (50)   NULL
);
GO

