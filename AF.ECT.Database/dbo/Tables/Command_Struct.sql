CREATE TABLE [dbo].[Command_Struct] (
    [CS_ID]                 INT            IDENTITY (1, 1) NOT NULL,
    [ADDRESS1]              NVARCHAR (100) NULL,
    [ADDRESS2]              NVARCHAR (100) NULL,
    [BASE_CODE]             NVARCHAR (10)  NULL,
    [CITY]                  NVARCHAR (30)  NULL,
    [COMMAND_CODE]          NVARCHAR (50)  NULL,
    [COMMAND_STRUCT_UTC]    NVARCHAR (10)  NULL,
    [COMPONENT]             NVARCHAR (4)   NULL,
    [COUNTRY]               NVARCHAR (40)  NULL,
    [CREATED_BY]            NVARCHAR (30)  NULL,
    [CREATED_DATE]          DATETIME       NULL,
    [CS_ID_PARENT]          INT            NULL,
    [CS_LEVEL]              NVARCHAR (10)  NULL,
    [CS_OPER_TYPE]          NVARCHAR (2)   NULL,
    [E_MAIL]                NVARCHAR (40)  NULL,
    [GAINING_COMMAND_CS_ID] INT            NULL,
    [GEO_LOC]               NVARCHAR (10)  NULL,
    [LONG_NAME]             NVARCHAR (100) NULL,
    [MEDICAL_SERVICE]       NVARCHAR (4)   NULL,
    [MODIFIED_BY]           NVARCHAR (30)  NULL,
    [MODIFIED_DATE]         DATETIME       NULL,
    [MRDSS_DOC_DATE]        DATETIME       NULL,
    [MRDSS_DOC_ID]          NVARCHAR (4)   NULL,
    [MRDSS_DOC_REVIEW]      NVARCHAR (24)  NULL,
    [MRDSS_KIND]            NVARCHAR (10)  NULL,
    [PAS_CODE]              VARCHAR (4)    NULL,
    [PHYS_EXAM_YN]          NVARCHAR (1)   NULL,
    [POSTAL_CODE]           NVARCHAR (20)  NULL,
    [SCHEDULING_YN]         NVARCHAR (1)   NULL,
    [STATE]                 NVARCHAR (2)   NULL,
    [TIME_ZONE]             NVARCHAR (3)   NULL,
    [UIC]                   NVARCHAR (6)   NULL,
    [UNIT_DET]              NVARCHAR (4)   NULL,
    [UNIT_KIND]             NVARCHAR (5)   NULL,
    [UNIT_NBR]              NVARCHAR (4)   NULL,
    [UNIT_TYPE]             NVARCHAR (2)   NULL,
    [Inactive]              BIT            NOT NULL,
    [UserModified]          BIT            NULL,
    [IsCollocated]          BIT            DEFAULT ((0)) NOT NULL
);
GO

ALTER TABLE [dbo].[Command_Struct]
    ADD CONSTRAINT [DF_Command_Struct_Inactive] DEFAULT ((0)) FOR [Inactive];
GO

ALTER TABLE [dbo].[Command_Struct]
    ADD CONSTRAINT [FK_COMMAND_STRUCT_core_lkupTimeZone] FOREIGN KEY ([TIME_ZONE]) REFERENCES [dbo].[core_lkupTimeZone] ([ZoneId]);
GO

CREATE NONCLUSTERED INDEX [IDX_CommandStruct_CSID]
    ON [dbo].[Command_Struct]([CS_ID] ASC) WITH (FILLFACTOR = 90);
GO

CREATE UNIQUE CLUSTERED INDEX [PX_commandStruct_Primary]
    ON [dbo].[Command_Struct]([CS_ID] ASC) WITH (FILLFACTOR = 90);
GO

CREATE NONCLUSTERED INDEX [IX_CommandStruct_PasCode]
    ON [dbo].[Command_Struct]([PAS_CODE] ASC) WITH (FILLFACTOR = 90);
GO

