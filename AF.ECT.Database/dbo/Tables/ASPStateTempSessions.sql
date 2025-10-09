CREATE TABLE [dbo].[ASPStateTempSessions] (
    [SessionId]        NVARCHAR (88)    NOT NULL,
    [Created]          DATETIME         DEFAULT (getutcdate()) NOT NULL,
    [Expires]          DATETIME         NOT NULL,
    [LockDate]         DATETIME         NOT NULL,
    [LockDateLocal]    DATETIME         NOT NULL,
    [LockCookie]       INT              NOT NULL,
    [Timeout]          INT              NOT NULL,
    [Locked]           BIT              NOT NULL,
    [SessionItemShort] VARBINARY (7000) NULL,
    [SessionItemLong]  IMAGE            NULL,
    [Flags]            INT              DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([SessionId] ASC) WITH (FILLFACTOR = 90)
);
GO

CREATE NONCLUSTERED INDEX [Index_Expires]
    ON [dbo].[ASPStateTempSessions]([Expires] ASC) WITH (FILLFACTOR = 90);
GO

