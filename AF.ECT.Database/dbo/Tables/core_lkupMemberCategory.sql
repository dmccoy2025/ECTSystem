CREATE TABLE [dbo].[core_lkupMemberCategory] (
    [Member_Status_ID]   INT          IDENTITY (1, 1) NOT NULL,
    [Member_Status_Desc] NVARCHAR (5) NOT NULL,
    [sort_order]         INT          NULL
);
GO

ALTER TABLE [dbo].[core_lkupMemberCategory]
    ADD CONSTRAINT [PK_core_lkupMemberCategory] PRIMARY KEY CLUSTERED ([Member_Status_ID] ASC) WITH (FILLFACTOR = 90);
GO

