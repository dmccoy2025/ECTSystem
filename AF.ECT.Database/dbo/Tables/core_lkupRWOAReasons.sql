CREATE TABLE [dbo].[core_lkupRWOAReasons] (
    [ID]          TINYINT       IDENTITY (1, 1) NOT NULL,
    [Description] VARCHAR (150) NOT NULL,
    [Type]        VARCHAR (100) NOT NULL
);
GO

ALTER TABLE [dbo].[core_lkupRWOAReasons]
    ADD CONSTRAINT [PK_core_lkupRWOAReasons] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90);
GO

