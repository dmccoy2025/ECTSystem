CREATE TABLE [dbo].[core_Messages] (
    [messageID]      SMALLINT       IDENTITY (1, 1) NOT NULL,
    [message]        VARCHAR (1024) NOT NULL,
    [title]          VARCHAR (50)   NULL,
    [name]           VARCHAR (50)   NOT NULL,
    [createdBy]      INT            NOT NULL,
    [popup]          BIT            NOT NULL,
    [startTime]      DATETIME       NOT NULL,
    [endTime]        DATETIME       NULL,
    [isAdminMessage] BIT            DEFAULT ((0)) NOT NULL
);
GO

