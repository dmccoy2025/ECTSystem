CREATE TABLE [dbo].[core_KeyVal_Value] (
    [Id]                INT           IDENTITY (1, 1) NOT NULL,
    [Key_Id]            INT           NOT NULL,
    [Value_Id]          VARCHAR (20)  NULL,
    [Value]             VARCHAR (MAX) NULL,
    [Value_Description] VARCHAR (50)  DEFAULT ('') NOT NULL
);
GO

ALTER TABLE [dbo].[core_KeyVal_Value]
    ADD CONSTRAINT [PK_core_KeyVal_Value] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

ALTER TABLE [dbo].[core_KeyVal_Value]
    ADD CONSTRAINT [FK_core_KeyVal_Value_core_KeyVal_Key] FOREIGN KEY ([Key_Id]) REFERENCES [dbo].[core_KeyVal_Key] ([ID]);
GO

