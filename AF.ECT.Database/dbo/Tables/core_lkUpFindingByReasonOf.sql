CREATE TABLE [dbo].[core_lkUpFindingByReasonOf] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Description] VARCHAR (100) NOT NULL
);
GO

ALTER TABLE [dbo].[core_lkUpFindingByReasonOf]
    ADD CONSTRAINT [PK_core_lkUpFindingByReasonOf] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 80);
GO

