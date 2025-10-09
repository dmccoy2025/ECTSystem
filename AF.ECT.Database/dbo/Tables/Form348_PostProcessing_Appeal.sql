CREATE TABLE [dbo].[Form348_PostProcessing_Appeal] (
    [appeal_id]                INT            NOT NULL,
    [initial_lod_id]           INT            NOT NULL,
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

ALTER TABLE [dbo].[Form348_PostProcessing_Appeal]
    ADD CONSTRAINT [FK_Form348_PostProcessing_Appeal] FOREIGN KEY ([initial_lod_id]) REFERENCES [dbo].[Form348] ([lodId]);
GO

ALTER TABLE [dbo].[Form348_PostProcessing_Appeal]
    ADD CONSTRAINT [PK_Form348_PostProcessing_Appeal] PRIMARY KEY CLUSTERED ([appeal_id] ASC) WITH (FILLFACTOR = 80);
GO

