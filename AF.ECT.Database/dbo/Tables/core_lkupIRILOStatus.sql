CREATE TABLE [dbo].[core_lkupIRILOStatus] (
    [id]          INT           IDENTITY (1, 1) NOT NULL,
    [description] VARCHAR (200) NOT NULL
);
GO

ALTER TABLE [dbo].[core_lkupIRILOStatus]
    ADD CONSTRAINT [PK_core_lkupIRILOStatus] PRIMARY KEY CLUSTERED ([id] ASC);
GO

