CREATE TABLE [dbo].[core_lkupMemberStatus] (
    [id]          INT          IDENTITY (1, 1) NOT NULL,
    [memberType]  VARCHAR (50) NULL,
    [memberDescr] VARCHAR (50) NULL,
    [sort_order]  INT          NULL
);
GO

ALTER TABLE [dbo].[core_lkupMemberStatus]
    ADD CONSTRAINT [PK_core_lkupMemberType] PRIMARY KEY CLUSTERED ([id] ASC) WITH (FILLFACTOR = 80);
GO

