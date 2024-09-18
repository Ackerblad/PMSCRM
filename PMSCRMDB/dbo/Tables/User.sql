CREATE TABLE [dbo].[User] (
    [user_id]       UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [company_id]    UNIQUEIDENTIFIER NOT NULL,
    [role_id]       UNIQUEIDENTIFIER NOT NULL,
    [username]      NVARCHAR (100)   NOT NULL,
    [password]      NVARCHAR (100)   NOT NULL,
    [first_name]    NVARCHAR (100)   NOT NULL,
    [last_name]     NVARCHAR (100)   NOT NULL,
    [phone_number]  NVARCHAR (100)   NOT NULL,
    [email_address] NVARCHAR (100)   NOT NULL,
    [timestamp]     SMALLDATETIME    DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([user_id] ASC),
    FOREIGN KEY ([company_id]) REFERENCES [dbo].[Company] ([company_id]),
    FOREIGN KEY ([role_id]) REFERENCES [dbo].[Role] ([role_id])
);

