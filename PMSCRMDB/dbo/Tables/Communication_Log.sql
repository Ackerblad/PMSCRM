CREATE TABLE [dbo].[Communication_Log] (
    [communication_log_id] UNIQUEIDENTIFIER CONSTRAINT [DF__Communica__commu__07C12930] DEFAULT (newid()) NOT NULL,
    [company_id]           UNIQUEIDENTIFIER NOT NULL,
    [customer_id]          UNIQUEIDENTIFIER NOT NULL,
    [task_id]              UNIQUEIDENTIFIER NOT NULL,
    [email_id]             UNIQUEIDENTIFIER NULL,
    [phone_call_id]        UNIQUEIDENTIFIER NULL,
    [log_date]             DATETIME         CONSTRAINT [DF__Communica__log_d__08B54D69] DEFAULT (getdate()) NULL,
    [notes]                NVARCHAR (255)   NULL,
    [timestamp]            SMALLDATETIME    CONSTRAINT [DF__Communica__times__09A971A2] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK__Communic__6DB710724D8EE647] PRIMARY KEY CLUSTERED ([communication_log_id] ASC),
    CONSTRAINT [FK__Communica__compa__0E6E26BF] FOREIGN KEY ([company_id]) REFERENCES [dbo].[Company] ([company_id]),
    CONSTRAINT [FK__Communica__custo__0A9D95DB] FOREIGN KEY ([customer_id]) REFERENCES [dbo].[Customer] ([customer_id]),
    CONSTRAINT [FK__Communica__email__0C85DE4D] FOREIGN KEY ([email_id]) REFERENCES [dbo].[Email] ([email_id]),
    CONSTRAINT [FK__Communica__phone__0D7A0286] FOREIGN KEY ([phone_call_id]) REFERENCES [dbo].[Phone_Call] ([phone_call_id]),
    CONSTRAINT [FK__Communica__task___0B91BA14] FOREIGN KEY ([task_id]) REFERENCES [dbo].[Task] ([task_id])
);

