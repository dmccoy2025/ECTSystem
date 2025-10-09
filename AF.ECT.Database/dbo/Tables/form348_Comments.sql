CREATE TABLE [dbo].[form348_Comments] (
    [id]           INT           IDENTITY (1, 1) NOT NULL,
    [lodid]        INT           NOT NULL,
    [comments]     VARCHAR (MAX) NOT NULL,
    [created_by]   INT           NOT NULL,
    [created_date] DATETIME      NOT NULL,
    [deleted]      BIT           NOT NULL
);
GO

ALTER TABLE [dbo].[form348_Comments]
    ADD CONSTRAINT [PK_form348_Comments] PRIMARY KEY CLUSTERED ([id] ASC) WITH (FILLFACTOR = 80);
GO

