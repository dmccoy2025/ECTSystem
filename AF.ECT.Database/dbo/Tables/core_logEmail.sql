CREATE TABLE [dbo].[core_logEmail] (
    [e_id]       INT           IDENTITY (1, 1) NOT NULL,
    [user_id]    INT           NULL,
    [date_sent]  DATETIME      NULL,
    [e_to]       VARCHAR (100) NULL,
    [e_cc]       VARCHAR (100) NULL,
    [e_bcc]      VARCHAR (MAX) NULL,
    [subject]    VARCHAR (200) NULL,
    [body]       VARCHAR (MAX) NULL,
    [failed]     VARCHAR (MAX) NULL,
    [templateId] INT           DEFAULT (NULL) NULL
);
GO

ALTER TABLE [dbo].[core_logEmail]
    ADD CONSTRAINT [DF_core_logEmail_date_sent] DEFAULT (getdate()) FOR [date_sent];
GO

