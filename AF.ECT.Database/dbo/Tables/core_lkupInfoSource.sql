CREATE TABLE [dbo].[core_lkupInfoSource] (
    [source_id]          INT           IDENTITY (1, 1) NOT NULL,
    [source_description] VARCHAR (100) NOT NULL,
    [sort_order]         INT           NOT NULL
);
GO

ALTER TABLE [dbo].[core_lkupInfoSource]
    ADD CONSTRAINT [PK_core_lkupInfoSource] PRIMARY KEY CLUSTERED ([source_id] ASC);
GO

