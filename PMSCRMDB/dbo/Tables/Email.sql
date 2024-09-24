CREATE TABLE [dbo].[Email] (
    [email_id]     UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [company_id]   UNIQUEIDENTIFIER NOT NULL,
    [customer_id]  UNIQUEIDENTIFIER NOT NULL,
    [user_id]      UNIQUEIDENTIFIER NOT NULL,
    [sender]       NVARCHAR (255)   NOT NULL,
    [recipient]    NVARCHAR (255)   NOT NULL,
    [subject]      NVARCHAR (255)   NULL,
    [body]         NVARCHAR (MAX)   NULL,
    [sent_date]    DATETIME         NOT NULL,
    [received]     BIT              NOT NULL,
    [api_response] NVARCHAR (MAX)   NULL,
    [status]       TINYINT          NOT NULL,
    [timestamp]    SMALLDATETIME    DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([email_id] ASC),
    FOREIGN KEY ([company_id]) REFERENCES [dbo].[Company] ([company_id]),
    FOREIGN KEY ([customer_id]) REFERENCES [dbo].[Customer] ([customer_id]),
    CONSTRAINT [FK__Email__user_id__7D439ABD] FOREIGN KEY ([user_id]) REFERENCES [dbo].[User] ([user_id])
);

