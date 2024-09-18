CREATE TABLE [dbo].[Customer] (
    [customer_id]       UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [company_id]        UNIQUEIDENTIFIER NOT NULL,
    [name]              NVARCHAR (100)   NOT NULL,
    [phone_number]      NVARCHAR (100)   NOT NULL,
    [email_address]     NVARCHAR (100)   NOT NULL,
    [street_address]    NVARCHAR (100)   NOT NULL,
    [city]              NVARCHAR (100)   NOT NULL,
    [state_or_province] NVARCHAR (100)   NULL,
    [postal_code]       NVARCHAR (20)    NULL,
    [country]           NVARCHAR (100)   NOT NULL,
    [timestamp]         SMALLDATETIME    DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([customer_id] ASC),
    FOREIGN KEY ([company_id]) REFERENCES [dbo].[Company] ([company_id])
);

