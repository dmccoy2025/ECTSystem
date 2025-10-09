CREATE TABLE [dbo].[Command_Struct_history] (
    [id]                 INT            IDENTITY (1, 1) NOT NULL,
    [pkgLog_Id]          INT            NULL,
    [pascode]            VARCHAR (4)    NOT NULL,
    [cs_id]              INT            NOT NULL,
    [created_date]       DATETIME       NOT NULL,
    [created_by]         INT            NOT NULL,
    [ADDRESS1]           NVARCHAR (100) NULL,
    [ADDRESS2]           NVARCHAR (100) NULL,
    [BASE_CODE]          NVARCHAR (10)  NULL,
    [CITY]               NVARCHAR (30)  NULL,
    [COMMAND_CODE]       NVARCHAR (50)  NULL,
    [COMMAND_STRUCT_UTC] NVARCHAR (10)  NULL,
    [COMPONENT]          NVARCHAR (4)   NULL,
    [COUNTRY]            NVARCHAR (40)  NULL,
    [CS_ID_PARENT]       INT            NULL,
    [CS_LEVEL]           NVARCHAR (10)  NULL,
    [CS_OPER_TYPE]       NVARCHAR (2)   NULL,
    [GEO_LOC]            NVARCHAR (10)  NULL,
    [LONG_NAME]          NVARCHAR (100) NULL,
    [POSTAL_CODE]        NVARCHAR (20)  NULL,
    [STATE]              NVARCHAR (2)   NULL,
    [UIC]                NVARCHAR (6)   NULL,
    [UNIT_DET]           NVARCHAR (4)   NULL,
    [UNIT_KIND]          NVARCHAR (5)   NULL,
    [UNIT_NBR]           NVARCHAR (4)   NULL,
    [UNIT_TYPE]          NVARCHAR (2)   NULL,
    [ChangeType]         VARCHAR (1)    NULL,
    [PARENT_PAS_CODE]    VARCHAR (4)    NULL
);
GO

ALTER TABLE [dbo].[Command_Struct_history]
    ADD CONSTRAINT [PK_Command_Struct_history] PRIMARY KEY CLUSTERED ([id] ASC) WITH (FILLFACTOR = 90);
GO

