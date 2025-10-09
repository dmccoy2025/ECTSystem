CREATE TABLE [dbo].[conversion_run] (
    [run_id]            INT           IDENTITY (1, 1) NOT NULL,
    [start_datestamp]   DATETIME      NOT NULL,
    [end_datestamp]     DATETIME      NULL,
    [status]            VARCHAR (20)  NULL,
    [records_processed] INT           NULL,
    [erros_found]       INT           NULL,
    [last_user_id]      VARCHAR (100) NULL,
    [last_patient_id]   VARCHAR (100) NULL,
    [last_lod_case_id]  VARCHAR (100) NULL
);
GO

ALTER TABLE [dbo].[conversion_run]
    ADD CONSTRAINT [PK_conversion_run] PRIMARY KEY CLUSTERED ([run_id] ASC) WITH (FILLFACTOR = 80);
GO

ALTER TABLE [dbo].[conversion_run]
    ADD CONSTRAINT [DF_conversion_run_start_datestamp] DEFAULT (getdate()) FOR [start_datestamp];
GO

