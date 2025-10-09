CREATE TABLE [dbo].[core_lkupProximateCause] (
    [cause_id]          INT           IDENTITY (1, 1) NOT NULL,
    [cause_description] VARCHAR (100) NOT NULL,
    [sort_order]        INT           NOT NULL
);
GO

ALTER TABLE [dbo].[core_lkupProximateCause]
    ADD CONSTRAINT [PK_core_lkupProximateCause] PRIMARY KEY CLUSTERED ([cause_id] ASC);
GO

