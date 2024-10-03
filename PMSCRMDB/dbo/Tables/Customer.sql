CREATE TABLE [dbo].[Customer] (
    [customer_id]       UNIQUEIDENTIFIER CONSTRAINT [DF__Customer__custom__6383C8BA] DEFAULT (newid()) NOT NULL,
    [company_id]        UNIQUEIDENTIFIER NOT NULL,
    [name]              NVARCHAR (100)   NOT NULL,
    [phone_number]      NVARCHAR (100)   NOT NULL,
    [email_address]     NVARCHAR (100)   NOT NULL,
    [street_address]    NVARCHAR (100)   NOT NULL,
    [city]              NVARCHAR (100)   NOT NULL,
    [state_or_province] NVARCHAR (100)   NOT NULL,
    [postal_code]       NVARCHAR (20)    NOT NULL,
    [country]           NVARCHAR (100)   NOT NULL,
    [timestamp]         SMALLDATETIME    CONSTRAINT [DF__Customer__timest__6477ECF3] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK__Customer__CD65CB858D1E2325] PRIMARY KEY CLUSTERED ([customer_id] ASC),
    CONSTRAINT [FK__Customer__compan__656C112C] FOREIGN KEY ([company_id]) REFERENCES [dbo].[Company] ([company_id])
);

