CREATE TABLE [dbo].[core_lkupPhyCancelReasons] (
    [ID]          TINYINT       NOT NULL,
    [Description] VARCHAR (150) NOT NULL,
    [Type]        VARCHAR (100) NOT NULL
);
GO

ALTER TABLE [dbo].[core_lkupPhyCancelReasons]
    ADD CONSTRAINT [PK_core_lkupPhyCancelReasons] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 80);
GO

