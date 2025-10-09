CREATE TABLE [dbo].[core_SignOnly_Signatures] (
    [refId]     INT           NOT NULL,
    [workflow]  INT           NOT NULL,
    [signature] VARCHAR (MAX) NOT NULL,
    [sig_date]  VARCHAR (MAX) NOT NULL,
    [user_id]   INT           NULL,
    [ptype]     INT           NULL
);
GO

