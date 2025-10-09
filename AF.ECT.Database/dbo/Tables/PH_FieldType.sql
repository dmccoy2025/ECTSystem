CREATE TABLE [dbo].[PH_FieldType] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (100) NOT NULL,
    [Placeholder] NVARCHAR (25)  NULL,
    [DataTypeId]  INT            NOT NULL,
    [Datasource]  NVARCHAR (200) NULL,
    [Color]       NVARCHAR (50)  NULL,
    [Length]      INT            NULL
);
GO

ALTER TABLE [dbo].[PH_FieldType]
    ADD CONSTRAINT [PK_PH_FieldType] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 80);
GO

ALTER TABLE [dbo].[PH_FieldType]
    ADD CONSTRAINT [FK_PH_FieldType_core_lkupDataType] FOREIGN KEY ([DataTypeId]) REFERENCES [dbo].[core_lkupDataType] ([Id]);
GO

