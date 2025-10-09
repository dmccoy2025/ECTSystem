CREATE TABLE [dbo].[core_lkupCancelReason] (
    [Id]           TINYINT        IDENTITY (1, 1) NOT NULL,
    [Description]  NVARCHAR (150) NOT NULL,
    [DisplayOrder] TINYINT        NOT NULL
);
GO

ALTER TABLE [dbo].[core_lkupCancelReason]
    ADD CONSTRAINT [PK_core_lkupCancelReason] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 80);
GO

