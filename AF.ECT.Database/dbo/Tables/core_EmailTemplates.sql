CREATE TABLE [dbo].[core_EmailTemplates] (
    [TemplateID] INT            IDENTITY (1, 1) NOT NULL,
    [compo]      CHAR (1)       NULL,
    [Title]      VARCHAR (100)  NOT NULL,
    [Subject]    VARCHAR (100)  NOT NULL,
    [Body]       VARCHAR (2000) NOT NULL,
    [DataProc]   VARCHAR (50)   NULL,
    [Active]     BIT            NOT NULL,
    [Sys_Date]   DATETIME       NOT NULL
);
GO

ALTER TABLE [dbo].[core_EmailTemplates]
    ADD CONSTRAINT [DF_core_EmailTemplates_Sys_Date] DEFAULT (getdate()) FOR [Sys_Date];
GO

ALTER TABLE [dbo].[core_EmailTemplates]
    ADD CONSTRAINT [DF_core_EmailTemplates_Deleate] DEFAULT ((0)) FOR [Active];
GO

ALTER TABLE [dbo].[core_EmailTemplates]
    ADD CONSTRAINT [PK_core_EmailTemplates] PRIMARY KEY CLUSTERED ([TemplateID] ASC) WITH (FILLFACTOR = 90);
GO

