CREATE TABLE [dbo].[imp_process_valid_status] (
    [PVS_ID]               NVARCHAR (50)   NULL,
    [PROC_ID]              NVARCHAR (50)   NULL,
    [PROCESS_NAME]         NVARCHAR (4000) NULL,
    [PROC_STATUS]          NVARCHAR (50)   NULL,
    [STATUS_SEQ]           NVARCHAR (50)   NULL,
    [STATUS_MEANING]       NVARCHAR (50)   NULL,
    [REQUIRED_YN]          NVARCHAR (50)   NULL,
    [CLOSES_PROCESS_YN]    NVARCHAR (50)   NULL,
    [RESPONSIBILITY_LEVEL] NVARCHAR (50)   NULL,
    [CREATED_BY]           NVARCHAR (50)   NULL,
    [CREATED_DATE]         NVARCHAR (50)   NULL,
    [MODIFIED_BY]          NVARCHAR (50)   NULL,
    [MODIFIED_DATE]        NVARCHAR (50)   NULL
);
GO

