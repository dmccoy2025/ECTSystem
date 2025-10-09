CREATE TABLE [dbo].[core_lkupOccurrenceDescription] (
    [occurrence_id]          INT           IDENTITY (1, 1) NOT NULL,
    [occurrence_description] VARCHAR (100) NOT NULL,
    [sort_order]             INT           NOT NULL
);
GO

ALTER TABLE [dbo].[core_lkupOccurrenceDescription]
    ADD CONSTRAINT [PK_core_lkupOccurrenceDescription] PRIMARY KEY CLUSTERED ([occurrence_id] ASC);
GO

