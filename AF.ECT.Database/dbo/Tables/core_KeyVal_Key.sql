CREATE TABLE [dbo].[core_KeyVal_Key] (
    [ID]          INT           IDENTITY (1, 1) NOT NULL,
    [Description] VARCHAR (200) NULL,
    [Key_Type_Id] INT           DEFAULT ((1)) NOT NULL
);
GO

ALTER TABLE [dbo].[core_KeyVal_Key]
    ADD CONSTRAINT [FK_KeyType] FOREIGN KEY ([Key_Type_Id]) REFERENCES [dbo].[core_KeyVal_KeyType] ([Id]);
GO

ALTER TABLE [dbo].[core_KeyVal_Key]
    ADD CONSTRAINT [PK_core_KeyVal_Key] PRIMARY KEY CLUSTERED ([ID] ASC);
GO

