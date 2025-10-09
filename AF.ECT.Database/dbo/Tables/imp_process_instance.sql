CREATE TABLE [dbo].[imp_process_instance] (
    [PI_ID]             NVARCHAR (50)   NULL,
    [PROC_ID]           NVARCHAR (50)   NULL,
    [PROCESS_NAME]      NVARCHAR (4000) NULL,
    [LOD_ID]            NVARCHAR (50)   NULL,
    [PERS_ID]           NVARCHAR (50)   NULL,
    [START_DATE]        NVARCHAR (50)   NULL,
    [END_DATE]          NVARCHAR (50)   NULL,
    [COMPLETED_YN]      NVARCHAR (50)   NULL,
    [CREATED_BY]        NVARCHAR (50)   NULL,
    [CREATED_DATE]      NVARCHAR (50)   NULL,
    [MODIFIED_BY]       NVARCHAR (50)   NULL,
    [MODIFIED_DATE]     NVARCHAR (50)   NULL,
    [SCRIPT_CLOSED]     NVARCHAR (50)   NULL,
    [DUE_DATE]          NVARCHAR (50)   NULL,
    [INST_TYPE]         NVARCHAR (50)   NULL,
    [EXPIRATION_DATE]   NVARCHAR (50)   NULL,
    [REQ_COMPLETE_DATE] NVARCHAR (50)   NULL,
    [DEPLOYMENT_ID]     NVARCHAR (50)   NULL
);
GO

