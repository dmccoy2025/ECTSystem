CREATE TABLE [dbo].[core_lkupRMUs] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [RMU]        NVARCHAR (255) NULL,
    [cs_id]      INT            DEFAULT (NULL) NULL,
    [collocated] BIT            DEFAULT ((1)) NOT NULL
);
GO

ALTER TABLE [dbo].[core_lkupRMUs]
    ADD CONSTRAINT [PK_core_lkupRMUs] PRIMARY KEY CLUSTERED ([Id] ASC);
GO

