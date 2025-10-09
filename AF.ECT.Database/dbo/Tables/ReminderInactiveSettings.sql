CREATE TABLE [dbo].[ReminderInactiveSettings] (
    [i_id]                  INT NOT NULL,
    [interval]              INT NOT NULL,
    [notification_interval] INT NOT NULL,
    [templateId]            INT NOT NULL,
    [active]                BIT NULL,
    PRIMARY KEY CLUSTERED ([i_id] ASC)
);
GO

