CREATE TABLE [dbo].[core_lkupModule] (
    [moduleId]      TINYINT       NOT NULL,
    [moduleName]    NVARCHAR (50) NOT NULL,
    [isSpecialCase] BIT           DEFAULT ((0)) NOT NULL
);
GO

ALTER TABLE [dbo].[core_lkupModule]
    ADD CONSTRAINT [PK_core_Modules] PRIMARY KEY CLUSTERED ([moduleId] ASC) WITH (FILLFACTOR = 90);
GO

