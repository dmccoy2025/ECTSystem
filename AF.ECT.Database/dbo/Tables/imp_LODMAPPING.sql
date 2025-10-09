CREATE TABLE [dbo].[imp_LODMAPPING] (
    [alod_lod_id]                INT           NULL,
    [rcpha_lodid]                INT           NOT NULL,
    [createdDate]                DATETIME      NULL,
    [rc_pha_org_lodid]           INT           NULL,
    [case_id]                    VARCHAR (50)  NULL,
    [alod_status]                INT           NULL,
    [rcpha_valid_status]         INT           NULL,
    [rcpha_valid_status_meaning] VARCHAR (50)  NULL,
    [rcpha_proc_name]            VARCHAR (600) NULL
);
GO

ALTER TABLE [dbo].[imp_LODMAPPING]
    ADD CONSTRAINT [PK_LODMAPPING] PRIMARY KEY CLUSTERED ([rcpha_lodid] ASC) WITH (FILLFACTOR = 90);
GO

