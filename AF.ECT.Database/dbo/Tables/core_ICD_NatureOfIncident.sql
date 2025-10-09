CREATE TABLE [dbo].[core_ICD_NatureOfIncident] (
    [ICD_Id] INT NOT NULL,
    [NOI_Id] INT NOT NULL
);
GO

CREATE NONCLUSTERED INDEX [IX_NOI_ICD]
    ON [dbo].[core_ICD_NatureOfIncident]([NOI_Id] ASC, [ICD_Id] ASC) WITH (FILLFACTOR = 90);
GO

ALTER TABLE [dbo].[core_ICD_NatureOfIncident]
    ADD CONSTRAINT [PK_core_ICD_NatureOfIncident] PRIMARY KEY CLUSTERED ([ICD_Id] ASC, [NOI_Id] ASC) WITH (FILLFACTOR = 90);
GO

ALTER TABLE [dbo].[core_ICD_NatureOfIncident]
    ADD CONSTRAINT [FK_NOI] FOREIGN KEY ([NOI_Id]) REFERENCES [dbo].[core_lkupNatureOfIncident] ([Id]);
GO

ALTER TABLE [dbo].[core_ICD_NatureOfIncident]
    ADD CONSTRAINT [FK_ICD] FOREIGN KEY ([ICD_Id]) REFERENCES [dbo].[core_lkupICD9] ([ICD9_ID]);
GO

