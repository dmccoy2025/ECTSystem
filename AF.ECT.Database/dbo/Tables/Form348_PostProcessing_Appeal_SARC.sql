CREATE TABLE [dbo].[Form348_PostProcessing_Appeal_SARC] (
    [appeal_id]                INT            NOT NULL,
    [appeal_street]            NVARCHAR (200) NULL,
    [appeal_city]              NVARCHAR (100) NULL,
    [appeal_state]             NVARCHAR (50)  NULL,
    [appeal_zip]               NVARCHAR (100) NULL,
    [appeal_country]           NVARCHAR (50)  NULL,
    [member_notification_date] DATE           NULL,
    [helpExtensionNumber]      VARCHAR (50)   NULL,
    [email]                    VARCHAR (200)  NULL
);
GO

ALTER TABLE [dbo].[Form348_PostProcessing_Appeal_SARC]
    ADD CONSTRAINT [PK_Form348_PostProcessing_Appeal_SARC] PRIMARY KEY CLUSTERED ([appeal_id] ASC) WITH (FILLFACTOR = 80);
GO

ALTER TABLE [dbo].[Form348_PostProcessing_Appeal_SARC]
    ADD CONSTRAINT [FK_Form348_PostProcessing_Appeal_SARC] FOREIGN KEY ([appeal_id]) REFERENCES [dbo].[Form348_AP_SARC] ([appeal_sarc_id]);
GO

