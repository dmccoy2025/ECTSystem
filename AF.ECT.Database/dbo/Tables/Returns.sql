CREATE TABLE [dbo].[Returns] (
    [return_id]                    INT           IDENTITY (1, 1) NOT NULL,
    [refId]                        INT           NOT NULL,
    [workstatus_to]                INT           NOT NULL,
    [workstatus_from]              INT           NOT NULL,
    [workflow]                     TINYINT       NOT NULL,
    [sent_to]                      INT           NULL,
    [reason_sent_back]             TINYINT       NULL,
    [explanation_for_sending_back] VARCHAR (MAX) NULL,
    [sender]                       INT           NULL,
    [date_sent]                    DATETIME      NULL,
    [comments_back_to_sender]      VARCHAR (MAX) NULL,
    [date_sent_back]               DATETIME      NULL,
    [created_by]                   INT           NULL,
    [created_date]                 DATETIME      NULL,
    [rerouting]                    BIT           NULL,
    [board_return]                 BIT           NOT NULL
);
GO

ALTER TABLE [dbo].[Returns]
    ADD CONSTRAINT [PK_Form348_returns] PRIMARY KEY CLUSTERED ([return_id] ASC) WITH (FILLFACTOR = 90);
GO

