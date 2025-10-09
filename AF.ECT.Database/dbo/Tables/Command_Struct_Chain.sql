CREATE TABLE [dbo].[Command_Struct_Chain] (
    [CSC_ID]        INT           IDENTITY (1, 1) NOT NULL,
    [CS_ID]         INT           NULL,
    [CHAIN_TYPE]    NVARCHAR (20) NULL,
    [CSC_ID_PARENT] INT           NULL,
    [CREATED_BY]    NVARCHAR (30) NULL,
    [CREATED_DATE]  DATETIME      NULL,
    [MODIFIED_BY]   NVARCHAR (30) NULL,
    [MODIFIED_DATE] DATETIME      NULL,
    [view_type]     TINYINT       NOT NULL,
    [UserModified]  BIT           NULL
);
GO

CREATE NONCLUSTERED INDEX [IDX_Command_Struct_Chain_CS]
    ON [dbo].[Command_Struct_Chain]([CS_ID] ASC, [CHAIN_TYPE] ASC) WITH (FILLFACTOR = 80);
GO

CREATE NONCLUSTERED INDEX [IDX_CSC_CSC_ID_PARENT_view_type]
    ON [dbo].[Command_Struct_Chain]([CSC_ID_PARENT] ASC, [view_type] ASC);
GO

CREATE NONCLUSTERED INDEX [IDX_Command_Struct_Chain_CSC]
    ON [dbo].[Command_Struct_Chain]([CHAIN_TYPE] ASC)
    INCLUDE([CSC_ID], [CS_ID], [CSC_ID_PARENT]) WITH (FILLFACTOR = 80);
GO

CREATE NONCLUSTERED INDEX [IDX_Command_Struct_Chain_Type]
    ON [dbo].[Command_Struct_Chain]([CHAIN_TYPE] ASC, [CSC_ID_PARENT] ASC, [CS_ID] ASC) WITH (FILLFACTOR = 80);
GO

ALTER TABLE [dbo].[Command_Struct_Chain]
    ADD CONSTRAINT [FK_Command_Struct_Chain_core_lkupChainType] FOREIGN KEY ([view_type]) REFERENCES [dbo].[core_lkupChainType] ([id]);
GO

ALTER TABLE [dbo].[Command_Struct_Chain]
    ADD CONSTRAINT [DF_Command_Struct_Chain_UserModified] DEFAULT ((0)) FOR [UserModified];
GO

ALTER TABLE [dbo].[Command_Struct_Chain]
    ADD CONSTRAINT [PK_Command_Struct_Chain] PRIMARY KEY CLUSTERED ([CSC_ID] ASC) WITH (FILLFACTOR = 80);
GO

