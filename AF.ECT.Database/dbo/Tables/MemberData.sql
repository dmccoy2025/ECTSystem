CREATE TABLE [dbo].[MemberData] (
    [SSAN]                     VARCHAR (11)  NOT NULL,
    [FIRST_NAME]               VARCHAR (50)  NULL,
    [MIDDLE_NAMES]             VARCHAR (60)  NULL,
    [LAST_NAME]                VARCHAR (50)  NULL,
    [SUFFIX]                   VARCHAR (30)  NULL,
    [PAS_GAINING]              VARCHAR (8)   NULL,
    [ASG_REPT_NLT_DATE]        DATETIME      NULL,
    [RCD_ID]                   VARCHAR (2)   NULL,
    [REC_STAT_CURR]            VARCHAR (2)   NULL,
    [PAS]                      VARCHAR (8)   NULL,
    [COMM_DUTY_PHONE]          VARCHAR (12)  NULL,
    [OFFICE_SYMBOL]            VARCHAR (8)   NULL,
    [LOCAL_ADDR_STREET]        VARCHAR (200) NULL,
    [LOCAL_ADDR_CITY]          VARCHAR (40)  NULL,
    [LOCAL_ADDR_STATE]         VARCHAR (2)   NULL,
    [HOME_PHONE]               VARCHAR (20)  NULL,
    [ADRS_MAIL_DOM_CITY]       VARCHAR (40)  NULL,
    [ADRS_MAIL_DOM_STATE]      VARCHAR (2)   NULL,
    [ADRS_MAIL_ZIP]            VARCHAR (10)  NULL,
    [GR_CURR]                  INT           NOT NULL,
    [DY_POSN_NR]               VARCHAR (9)   NULL,
    [DAFSC]                    VARCHAR (7)   NULL,
    [DUTY_STATUS]              VARCHAR (2)   NULL,
    [DUTY_STATUS_EFF_DATE]     DATETIME      NULL,
    [DUTY_STATUS_EXP_DATE]     DATETIME      NULL,
    [DEPL_AVAIL_STATUS_ADMIN]  VARCHAR (2)   NULL,
    [DEPL_AVAIL_STATUS_A_EX_D] DATETIME      NULL,
    [DEPL_AVAIL_STATUS_PHYS]   VARCHAR (2)   NULL,
    [DEPL_AVAIL_STATUS_P_EX_D] DATETIME      NULL,
    [DEPL_AVAIL_STATUS_LEGAL]  VARCHAR (500) NULL,
    [DEPL_AVAIL_STATUS_L_EX_D] DATETIME      NULL,
    [DEPL_AVAIL_STATUS_TIME]   VARCHAR (2)   NULL,
    [DEPL_AVAIL_STATUS_T_EX_D] DATETIME      NULL,
    [PAFSC]                    VARCHAR (7)   NULL,
    [AFSC2]                    VARCHAR (7)   NULL,
    [AFSC3]                    VARCHAR (7)   NULL,
    [SEX_SVC_MBR]              VARCHAR (1)   NULL,
    [DOB]                      DATETIME      NULL,
    [RET_SEP_EFF_DATE_PROJ]    DATETIME      NULL,
    [DOS]                      DATETIME      NULL,
    [SVC_COMP]                 VARCHAR (1)   NULL,
    [TAFCSD]                   DATETIME      NULL,
    [TAFMSD]                   DATETIME      NULL,
    [ETS]                      DATETIME      NULL,
    [PAS_NUMBER]               VARCHAR (4)   NULL,
    [ZIP]                      VARCHAR (10)  NULL,
    [Deleted]                  BIT           NULL,
    [DeletedDate]              DATETIME      NULL,
    [ATTACH_PAS]               VARCHAR (8)   NULL
);
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'The gaining PAS code for a member''s pending reassignment.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MemberData', @level2type = N'COLUMN', @level2name = N'PAS_GAINING';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'The report not later than date (RNLTD) for Regular component assignments or effective date of change in strength accountability (EDCSA) for a member of the Air National Guard or AF Reserve upon reassignment.  This effectively starts strength accountability for new unit and stops accountability for losing unit.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MemberData', @level2type = N'COLUMN', @level2name = N'ASG_REPT_NLT_DATE';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Member''s'' FILE_TYPE (AA=AF Enlisted Regular Mbr, AG=AF Enlisted ANG Mbr,AR=AF Enlisted Reserve Mbr,BA=AF Officer Regular Mbr, BG=AF Officer ANG  Mbr, BR=AF Officer Reserve Mbr, EX=Ex-employee, etc)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MemberData', @level2type = N'COLUMN', @level2name = N'RCD_ID';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Member''s Record Stats (i.e. 10=Active No Projected Action, 20=Active Projected Separation No Projected Assignment,etc.).', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MemberData', @level2type = N'COLUMN', @level2name = N'REC_STAT_CURR';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Member''s organization of assignment', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MemberData', @level2type = N'COLUMN', @level2name = N'PAS';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'PHONE_NUMBER as it pertains to the member based on PHONE_TYPE.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MemberData', @level2type = N'COLUMN', @level2name = N'COMM_DUTY_PHONE';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'A 50-character string containing the first line of the delivery address for a person.  This typically contains aspects of the street address.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MemberData', @level2type = N'COLUMN', @level2name = N'LOCAL_ADDR_STREET';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'A 30-character string containing the city designation in an address.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MemberData', @level2type = N'COLUMN', @level2name = N'LOCAL_ADDR_CITY';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'The code that identifies the state in the United States used as a component of an address.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MemberData', @level2type = N'COLUMN', @level2name = N'LOCAL_ADDR_STATE';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'PHONE_NUMBER as it pertains to the member based on PHONE_TYPE.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MemberData', @level2type = N'COLUMN', @level2name = N'HOME_PHONE';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'A 30-character string containing the city designation in an address.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MemberData', @level2type = N'COLUMN', @level2name = N'ADRS_MAIL_DOM_CITY';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'The code that identifies the state in the United States used as a component of an address.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MemberData', @level2type = N'COLUMN', @level2name = N'ADRS_MAIL_DOM_STATE';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'The postal code used in a person''s delivery address.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MemberData', @level2type = N'COLUMN', @level2name = N'ADRS_MAIL_ZIP';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'The two-digit code used to uniquely identify a particular military pay grade/rank.  The textual equivalent is contained in the Cleartext column of grade (i.e. code ''34'' corresponds to SGT.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MemberData', @level2type = N'COLUMN', @level2name = N'GR_CURR';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Position number is a nine character field in MDS.  It is used as part of the database key for all manpower requirement records.  The attribute consists of a seven character number and a two character Major Command (MAJCOM) Identifier.   ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MemberData', @level2type = N'COLUMN', @level2name = N'DY_POSN_NR';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Duty Air Force Specialty Code (DAFSC).  The type of Air Force Specialty that applies to a member while assigned to a specific position.  7 positions include Prefix and Suffix', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MemberData', @level2type = N'COLUMN', @level2name = N'DAFSC';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'DUTY_STATUS', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MemberData', @level2type = N'COLUMN', @level2name = N'DUTY_STATUS';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Identifies the deployment availability status of an individual by category of limitation as determined by related administrative actions.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MemberData', @level2type = N'COLUMN', @level2name = N'DEPL_AVAIL_STATUS_ADMIN';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Date administrative code expires.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MemberData', @level2type = N'COLUMN', @level2name = N'DEPL_AVAIL_STATUS_A_EX_D';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Identifies the deployment availability status of an individual by category of limitation as determined by related physical actions.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MemberData', @level2type = N'COLUMN', @level2name = N'DEPL_AVAIL_STATUS_PHYS';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Date physical code expires.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MemberData', @level2type = N'COLUMN', @level2name = N'DEPL_AVAIL_STATUS_P_EX_D';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Identifies the deployment availability status of an individual by category of limitation as determined by related legal actions.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MemberData', @level2type = N'COLUMN', @level2name = N'DEPL_AVAIL_STATUS_LEGAL';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Date legal code expires.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MemberData', @level2type = N'COLUMN', @level2name = N'DEPL_AVAIL_STATUS_L_EX_D';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Identifies the deployment availability status of an individual by category of limitation as determined by related personnel actions.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MemberData', @level2type = N'COLUMN', @level2name = N'DEPL_AVAIL_STATUS_TIME';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Date time code expires.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MemberData', @level2type = N'COLUMN', @level2name = N'DEPL_AVAIL_STATUS_T_EX_D';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Individual''s Air Force Specialty Code (AFSC) require common qualifications identify by a title.  An ability, skill, special qualification or system designator not restrcited to a single AFSC (i.e. the Prefix ''T''  affixed to an AFSC indicates the person has been qualified as a Formal Training Instructor.  The primary AFSC normally denotes the AFSC the member is most qualified to perform duty in.  The 7-digit code includes Prefix and Suffix.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MemberData', @level2type = N'COLUMN', @level2name = N'PAFSC';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Individual''s Air Force Specialty Code (AFSC) require common qualifications identify by a title.  An ability, skill, special qualification or system designator not restrcited to a single AFSC (i.e. the Prefix ''T''  affixed to an AFSC indicates the person has been qualified as a Formal Training Instructor.  The secondary AFSC is the specialty the member is second highest qualified to perform duty in.  The 7-digit code includes Prefix and Suffix.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MemberData', @level2type = N'COLUMN', @level2name = N'AFSC2';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Individual''s Air Force Specialty Code (AFSC) require common qualifications identify by a title.  An ability, skill, special qualification or system designator not restrcited to a single AFSC (i.e. the Prefix ''T''  affixed to an AFSC indicates the person has been qualified as a Formal Training Instructor.  The third AFSC is the specialty the member is least likely qualified to perform duty in.  The 7-digit code includes Prefix and Suffix.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MemberData', @level2type = N'COLUMN', @level2name = N'AFSC3';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Code denoting the sex of the service member.  (Null)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MemberData', @level2type = N'COLUMN', @level2name = N'SEX_SVC_MBR';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Date of Birth of the Service member.  (NULL) ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MemberData', @level2type = N'COLUMN', @level2name = N'DOB';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'The date a member is projected to separate from the Air Force enlisted force.  Member''s Date of Separation one day prior to retirement date.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MemberData', @level2type = N'COLUMN', @level2name = N'RET_SEP_EFF_DATE_PROJ';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Date a member is due to separate from the service.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MemberData', @level2type = N'COLUMN', @level2name = N'DOS';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Members Service Component (R=Regular, G=Guard, V=Reserve, C=Civilian, T=Temporary)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MemberData', @level2type = N'COLUMN', @level2name = N'SVC_COMP';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Total Active Federal Commission Service Date.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MemberData', @level2type = N'COLUMN', @level2name = N'TAFCSD';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Total Active Federal Military Service Date.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MemberData', @level2type = N'COLUMN', @level2name = N'TAFMSD';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Expiration Term of Service.  The date a member''s term of service expires.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MemberData', @level2type = N'COLUMN', @level2name = N'ETS';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'A four character string containing the Personnel Accounting Symbol (PAS) associated with a given unit.  The PAS_MPF-NUMBER, MAJCOM_ID and PAS_NUMBER make up the PAS Code.  ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MemberData', @level2type = N'COLUMN', @level2name = N'PAS_NUMBER';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Military Personnel Flight Address ZIP code. ZIP code is a system established in 1963 that uses 5-digit codes to identify the individual post offices or delivery stations in metropolitan areas associated with an address.  The basic ZIP Code format consists ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MemberData', @level2type = N'COLUMN', @level2name = N'ZIP';
GO

ALTER TABLE [dbo].[MemberData]
    ADD CONSTRAINT [PK__MemberData__257C74A0] PRIMARY KEY CLUSTERED ([SSAN] ASC) WITH (FILLFACTOR = 80);
GO

ALTER TABLE [dbo].[MemberData]
    ADD CONSTRAINT [FK_MemberData_MemberData] FOREIGN KEY ([GR_CURR]) REFERENCES [dbo].[core_lkupGrade] ([CODE]);
GO

