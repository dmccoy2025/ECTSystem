CREATE TABLE [dbo].[core_lkupAccessStatus] (
    [statusId]    TINYINT      IDENTITY (1, 1) NOT NULL,
    [description] VARCHAR (20) NOT NULL
);
GO

ALTER TABLE [dbo].[core_lkupAccessStatus]
    ADD CONSTRAINT [PK_lkupAccessStatus] PRIMARY KEY CLUSTERED ([statusId] ASC) WITH (FILLFACTOR = 80);
GO

