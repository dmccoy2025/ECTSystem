CREATE TABLE [dbo].[Form348_PostProcessing] (
    [lodId]               INT            NOT NULL,
    [helpExtensionNumber] NVARCHAR (50)  NULL,
    [appealStreet]        NVARCHAR (200) NULL,
    [appealCity]          NVARCHAR (100) NULL,
    [appealState]         NVARCHAR (50)  NULL,
    [appealZip]           NVARCHAR (100) NULL,
    [appealCountry]       NVARCHAR (50)  NULL,
    [nokFirstName]        NVARCHAR (50)  NULL,
    [nokLastName]         NVARCHAR (50)  NULL,
    [nokMiddleName]       NVARCHAR (50)  NULL,
    [notification_date]   DATETIME       NULL,
    [email]               VARCHAR (200)  NULL,
    [address_flag]        INT            NULL,
    [email_flag]          INT            NULL,
    [phone_flag]          INT            NULL
);
GO

ALTER TABLE [dbo].[Form348_PostProcessing]
    ADD CONSTRAINT [PK_Form348_PostProcessing] PRIMARY KEY CLUSTERED ([lodId] ASC) WITH (FILLFACTOR = 80);
GO

ALTER TABLE [dbo].[Form348_PostProcessing]
    ADD CONSTRAINT [FK_Form348_PostProcessing_Form348] FOREIGN KEY ([lodId]) REFERENCES [dbo].[Form348] ([lodId]);
GO

