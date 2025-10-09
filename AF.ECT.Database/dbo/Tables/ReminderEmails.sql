CREATE TABLE [dbo].[ReminderEmails] (
    [id]               INT          IDENTITY (1, 1) NOT NULL,
    [settingId]        INT          NOT NULL,
    [caseId]           VARCHAR (50) NOT NULL,
    [lastSentDate]     DATE         NOT NULL,
    [lastModifiedDate] DATE         NOT NULL,
    [sentCount]        INT          DEFAULT ((0)) NULL,
    [member_unit_id]   INT          NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    FOREIGN KEY ([settingId]) REFERENCES [dbo].[ReminderEmailSettings] ([id])
);
GO

