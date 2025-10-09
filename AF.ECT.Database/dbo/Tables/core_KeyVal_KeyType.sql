CREATE TABLE [dbo].[core_KeyVal_KeyType] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Type_Name] NVARCHAR (100) NOT NULL
);
GO

ALTER TABLE [dbo].[core_KeyVal_KeyType]
    ADD CONSTRAINT [PK_core_KeyVal_KeyType] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 80);
GO

