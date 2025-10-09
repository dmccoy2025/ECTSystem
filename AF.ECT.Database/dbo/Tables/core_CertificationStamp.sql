CREATE TABLE [dbo].[core_CertificationStamp] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (100) NOT NULL,
    [Body]        NVARCHAR (500) NOT NULL,
    [IsQualified] BIT            NOT NULL
);
GO

ALTER TABLE [dbo].[core_CertificationStamp]
    ADD CONSTRAINT [PK_core_CertificationStamp] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 80);
GO

