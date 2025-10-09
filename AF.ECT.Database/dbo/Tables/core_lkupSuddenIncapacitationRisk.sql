CREATE TABLE [dbo].[core_lkupSuddenIncapacitationRisk] (
    [Id]        TINYINT        NOT NULL,
    [RiskLevel] NVARCHAR (100) NOT NULL
);
GO

ALTER TABLE [dbo].[core_lkupSuddenIncapacitationRisk]
    ADD CONSTRAINT [PK_core_lkupSuddenIncapacitationRisk] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

