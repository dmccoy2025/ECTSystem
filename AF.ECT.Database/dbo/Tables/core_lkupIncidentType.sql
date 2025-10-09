CREATE TABLE [dbo].[core_lkupIncidentType] (
    [id]            INT          IDENTITY (1, 1) NOT NULL,
    [incidentType]  VARCHAR (50) NULL,
    [incidentDescr] VARCHAR (50) NULL
);
GO

ALTER TABLE [dbo].[core_lkupIncidentType]
    ADD CONSTRAINT [PK_core_lkupIncidentType] PRIMARY KEY CLUSTERED ([id] ASC) WITH (FILLFACTOR = 80);
GO

