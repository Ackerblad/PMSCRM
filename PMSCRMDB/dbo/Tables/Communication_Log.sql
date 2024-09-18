CREATE TABLE [dbo].[Communication_Log] (
    [communication_log_id] UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [company_id]           UNIQUEIDENTIFIER NOT NULL,
    [customer_id]          UNIQUEIDENTIFIER NOT NULL,
    [task_id]              UNIQUEIDENTIFIER NOT NULL,
    [email_id]             UNIQUEIDENTIFIER NULL,
    [phone_call_id]        UNIQUEIDENTIFIER NULL,
    [log_date]             DATETIME         DEFAULT (getdate()) NULL,
    [notes]                NVARCHAR (255)   NULL,
    [timestamp]            SMALLDATETIME    DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([communication_log_id] ASC),
    FOREIGN KEY ([company_id]) REFERENCES [dbo].[Company] ([company_id]),
    FOREIGN KEY ([customer_id]) REFERENCES [dbo].[Customer] ([customer_id]),
    FOREIGN KEY ([email_id]) REFERENCES [dbo].[Email] ([email_id]),
    FOREIGN KEY ([phone_call_id]) REFERENCES [dbo].[Phone_Call] ([phone_call_id]),
    FOREIGN KEY ([task_id]) REFERENCES [dbo].[Task] ([task_id])
);

