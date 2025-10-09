CREATE TABLE [dbo].[core_lkupNatureOfIncident] (
    [Id]    INT           IDENTITY (1, 1) NOT NULL,
    [Value] NVARCHAR (50) NOT NULL,
    [Text]  NVARCHAR (50) NOT NULL
);
GO

ALTER TABLE [dbo].[core_lkupNatureOfIncident]
    ADD CONSTRAINT [PK_core_lkupNatureOfIncident] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90);
GO

