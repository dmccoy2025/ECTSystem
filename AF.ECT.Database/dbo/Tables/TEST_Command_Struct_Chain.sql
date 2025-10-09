CREATE TABLE [dbo].[TEST_Command_Struct_Chain] (
    [CSC_ID]        INT           IDENTITY (1, 1) NOT NULL,
    [CS_ID]         INT           NULL,
    [CHAIN_TYPE]    NVARCHAR (20) NULL,
    [CSC_ID_PARENT] INT           NULL,
    [CREATED_BY]    NVARCHAR (30) NULL,
    [CREATED_DATE]  DATETIME      NULL,
    [MODIFIED_BY]   NVARCHAR (30) NULL,
    [MODIFIED_DATE] DATETIME      NULL,
    [view_type]     TINYINT       NULL
);
GO

ALTER TABLE [dbo].[TEST_Command_Struct_Chain]
    ADD CONSTRAINT [PK_TEST_Command_Struct_Chain] PRIMARY KEY CLUSTERED ([CSC_ID] ASC) WITH (FILLFACTOR = 80);
GO

ALTER TABLE [dbo].[TEST_Command_Struct_Chain]
    ADD CONSTRAINT [FK_TEST_Command_Struct_Chain_core_lkupChainType] FOREIGN KEY ([view_type]) REFERENCES [dbo].[core_lkupChainType] ([id]);
GO

ALTER TABLE [dbo].[TEST_Command_Struct_Chain]
    ADD CONSTRAINT [FK_TEST_Command_Struct_Chain_Command_Struct] FOREIGN KEY ([CS_ID]) REFERENCES [dbo].[TEST_Command_Struct] ([CS_ID]);
GO

