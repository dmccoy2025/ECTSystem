CREATE TABLE [dbo].[Form348_Unit] (
    [lodid]                      INT            NOT NULL,
    [cmdr_circ_details]          VARCHAR (1460) NULL,
    [cmdr_duty_determination]    VARCHAR (100)  NULL,
    [cmdr_duty_from]             DATETIME       NULL,
    [cmdr_duty_others]           VARCHAR (100)  NULL,
    [cmdr_duty_to]               DATETIME       NULL,
    [cmdr_activated_yn]          CHAR (1)       NULL,
    [modified_date]              DATETIME       NOT NULL,
    [modified_by]                INT            NOT NULL,
    [source_information]         INT            NULL,
    [witnesses]                  XML            DEFAULT (N'<ArrayOfWitnessData xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" />') NULL,
    [source_information_specify] VARCHAR (100)  NULL,
    [member_occurrence]          INT            NULL,
    [absent_from]                DATETIME       NULL,
    [absent_to]                  DATETIME       NULL,
    [member_on_orders]           VARCHAR (10)   NULL,
    [member_credible]            VARCHAR (10)   NULL,
    [proximate_cause]            INT            NULL,
    [proximate_cause_specify]    VARCHAR (100)  NULL,
    [workflow]                   INT            DEFAULT (NULL) NULL,
    [travel_occurrence]          INT            NULL,
    [verified_status]            BIT            NULL,
    [proof_of_status]            BIT            NULL
);
GO

ALTER TABLE [dbo].[Form348_Unit]
    ADD CONSTRAINT [DF_Form348_Unit_modified_date] DEFAULT (getdate()) FOR [modified_date];
GO

ALTER TABLE [dbo].[Form348_Unit]
    ADD CONSTRAINT [FK_Form348_Unit_Form348] FOREIGN KEY ([lodid]) REFERENCES [dbo].[Form348] ([lodId]);
GO

ALTER TABLE [dbo].[Form348_Unit]
    ADD CONSTRAINT [PK_Form348_Unit] PRIMARY KEY CLUSTERED ([lodid] ASC) WITH (FILLFACTOR = 90);
GO

