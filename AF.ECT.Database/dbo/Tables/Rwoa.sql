CREATE TABLE [dbo].[Rwoa] (
    [rwoaId]                       INT           IDENTITY (1, 1) NOT NULL,
    [refId]                        INT           NOT NULL,
    [workstatus]                   INT           NOT NULL,
    [workflow]                     TINYINT       NOT NULL,
    [sent_to]                      VARCHAR (100) NULL,
    [reason_sent_back]             TINYINT       NULL,
    [explanation_for_sending_back] VARCHAR (MAX) NULL,
    [sender]                       VARCHAR (100) NULL,
    [date_sent]                    DATETIME      NULL,
    [comments_back_to_sender]      VARCHAR (MAX) NULL,
    [date_sent_back]               DATETIME      NULL,
    [created_by]                   INT           NULL,
    [created_date]                 DATETIME      NULL,
    [rerouting]                    BIT           NULL
);
GO

ALTER TABLE [dbo].[Rwoa]
    ADD CONSTRAINT [PK_Form348_rwoa] PRIMARY KEY CLUSTERED ([rwoaId] ASC) WITH (FILLFACTOR = 90);
GO

