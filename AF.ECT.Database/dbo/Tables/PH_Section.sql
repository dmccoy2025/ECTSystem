CREATE TABLE [dbo].[PH_Section] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (100) NOT NULL,
    [ParentId]     INT            NULL,
    [FieldColumns] INT            DEFAULT ((3)) NOT NULL,
    [IsTopLevel]   BIT            DEFAULT ((0)) NOT NULL,
    [DisplayOrder] INT            DEFAULT ((1)) NOT NULL,
    [PageBreak]    BIT            DEFAULT ((0)) NOT NULL
);
GO

ALTER TABLE [dbo].[PH_Section]
    ADD CONSTRAINT [PK_PH_Section] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 80);
GO

