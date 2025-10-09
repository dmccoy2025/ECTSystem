CREATE TABLE [dbo].[core_lkupFromLocation] (
    [location_id]          INT           IDENTITY (1, 1) NOT NULL,
    [location_description] VARCHAR (100) NOT NULL,
    [sort_order]           INT           NOT NULL
);
GO

ALTER TABLE [dbo].[core_lkupFromLocation]
    ADD CONSTRAINT [PK_core_lkupFromLocation] PRIMARY KEY CLUSTERED ([location_id] ASC);
GO

