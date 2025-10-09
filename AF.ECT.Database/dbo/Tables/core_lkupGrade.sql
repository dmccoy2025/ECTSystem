CREATE TABLE [dbo].[core_lkupGrade] (
    [CODE]         INT           NOT NULL,
    [RANK]         NVARCHAR (15) NULL,
    [GRADE]        NVARCHAR (50) NULL,
    [TITLE]        NVARCHAR (30) NULL,
    [DISPLAYORDER] SMALLINT      NULL
);
GO

ALTER TABLE [dbo].[core_lkupGrade]
    ADD CONSTRAINT [PK_LKUP_GRADE] PRIMARY KEY CLUSTERED ([CODE] ASC) WITH (FILLFACTOR = 80);
GO

