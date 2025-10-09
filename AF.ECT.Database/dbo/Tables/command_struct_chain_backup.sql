CREATE TABLE [dbo].[command_struct_chain_backup] (
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

