CREATE TABLE [dbo].[core_lkupComponent] (
    [component_id]          INT           IDENTITY (1, 1) NOT NULL,
    [component_description] VARCHAR (100) NOT NULL,
    [sort_order]            INT           NOT NULL
);
GO

ALTER TABLE [dbo].[core_lkupComponent]
    ADD CONSTRAINT [PK_core_lkupComponent] PRIMARY KEY CLUSTERED ([component_id] ASC);
GO

