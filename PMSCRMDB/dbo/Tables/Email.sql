CREATE TABLE [dbo].[Email] (
    [email_id]     UNIQUEIDENTIFIER CONSTRAINT [DF__Email__email_id__7A672E12] DEFAULT (newid()) NOT NULL,
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
    [timestamp]    SMALLDATETIME    CONSTRAINT [DF__Email__timestamp__7B5B524B] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK__Email__3FEF87661A87C9F7] PRIMARY KEY CLUSTERED ([email_id] ASC),
    CONSTRAINT [FK__Email__company_i__7E37BEF6] FOREIGN KEY ([company_id]) REFERENCES [dbo].[Company] ([company_id]),
    CONSTRAINT [FK__Email__customer___7C4F7684] FOREIGN KEY ([customer_id]) REFERENCES [dbo].[Customer] ([customer_id]),
    CONSTRAINT [FK__Email__user_id__7D439ABD] FOREIGN KEY ([user_id]) REFERENCES [dbo].[User] ([user_id])
);

