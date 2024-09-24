CREATE TABLE [dbo].[Phone_Call] (
    [phone_call_id] UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [company_id]    UNIQUEIDENTIFIER NOT NULL,
    [customer_id]   UNIQUEIDENTIFIER NOT NULL,
    [user_id]       UNIQUEIDENTIFIER NOT NULL,
    [phone_number]  NVARCHAR (20)    NOT NULL,
    [start_time]    DATETIME         NOT NULL,
    [end_time]      DATETIME         NULL,
    [duration]      TINYINT          NULL,
    [call_type]     TINYINT          NOT NULL,
    [api_response]  VARCHAR (MAX)    NULL,
    [status]        TINYINT          NOT NULL,
    [timestamp]     SMALLDATETIME    DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([phone_call_id] ASC),
    FOREIGN KEY ([company_id]) REFERENCES [dbo].[Company] ([company_id]),
    FOREIGN KEY ([customer_id]) REFERENCES [dbo].[Customer] ([customer_id]),
    CONSTRAINT [FK__Phone_Cal__user___03F0984C] FOREIGN KEY ([user_id]) REFERENCES [dbo].[User] ([user_id])
);

