CREATE TABLE [dbo].[core_Test] (
    [ID]           INT          IDENTITY (1, 1) NOT NULL,
    [name]         VARCHAR (50) NOT NULL,
    [active]       BIT          NULL,
    [CREATED_BY]   VARCHAR (50) NOT NULL,
    [CREATED_DAYE] DATETIME     NOT NULL
);
GO

ALTER TABLE [dbo].[core_Test]
    ADD CONSTRAINT [PK_core_Test] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90);
GO

