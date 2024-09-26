CREATE TABLE [dbo].[User] (
    [user_id]        UNIQUEIDENTIFIER CONSTRAINT [DF__User__user_id__6D0D32F4] DEFAULT (newid()) NOT NULL,
    [company_id]     UNIQUEIDENTIFIER NOT NULL,
    [role_id]        UNIQUEIDENTIFIER NOT NULL,
    [username]       NVARCHAR (100)   NOT NULL,
    [password_hash]  NVARCHAR (64)    NOT NULL,
    [password_salt]  NVARCHAR (64)    NOT NULL,
    [password_token] NVARCHAR (255)   NULL,
    [token_expiry]   DATETIME         NULL,
    [first_name]     NVARCHAR (100)   NOT NULL,
    [last_name]      NVARCHAR (100)   NOT NULL,
    [phone_number]   NVARCHAR (100)   NOT NULL,
    [email_address]  NVARCHAR (100)   NOT NULL,
    [timestamp]      SMALLDATETIME    CONSTRAINT [DF__User__timestamp__6E01572D] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK__User__B9BE370F84373947] PRIMARY KEY CLUSTERED ([user_id] ASC),
    CONSTRAINT [FK__User__company_id__6FE99F9F] FOREIGN KEY ([company_id]) REFERENCES [dbo].[Company] ([company_id]),
    CONSTRAINT [FK__User__role_id__6EF57B66] FOREIGN KEY ([role_id]) REFERENCES [dbo].[Role] ([role_id])
);

