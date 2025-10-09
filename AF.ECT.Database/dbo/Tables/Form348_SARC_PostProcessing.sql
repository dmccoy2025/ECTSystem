CREATE TABLE [dbo].[Form348_SARC_PostProcessing] (
    [sarc_id]                INT            NOT NULL,
    [memberNotified]         BIT            NULL,
    [memberNotificationDate] DATETIME       NULL,
    [helpExtensionNumber]    NVARCHAR (50)  NULL,
    [appealStreet]           NVARCHAR (200) NULL,
    [appealCity]             NVARCHAR (100) NULL,
    [appealState]            NVARCHAR (50)  NULL,
    [appealZip]              NVARCHAR (100) NULL,
    [appealCountry]          NVARCHAR (50)  NULL,
    [email]                  VARCHAR (200)  NULL
);
GO

ALTER TABLE [dbo].[Form348_SARC_PostProcessing]
    ADD CONSTRAINT [FK_Form348_SARC_PostProcessing_Form348_SARC] FOREIGN KEY ([sarc_id]) REFERENCES [dbo].[Form348_SARC] ([sarc_id]);
GO

ALTER TABLE [dbo].[Form348_SARC_PostProcessing]
    ADD CONSTRAINT [PK_Form348_SARC_PostProcessing] PRIMARY KEY CLUSTERED ([sarc_id] ASC) WITH (FILLFACTOR = 90);
GO

