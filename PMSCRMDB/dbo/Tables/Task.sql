﻿CREATE TABLE [dbo].[Task] (
    [task_id]     UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [company_id]  UNIQUEIDENTIFIER NOT NULL,
    [name]        NVARCHAR (50)    NOT NULL,
    [description] NVARCHAR (255)   NULL,
    [duration]    TINYINT          NOT NULL,
    [timestamp]   SMALLDATETIME    DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([task_id] ASC),
    FOREIGN KEY ([company_id]) REFERENCES [dbo].[Company] ([company_id])
);

