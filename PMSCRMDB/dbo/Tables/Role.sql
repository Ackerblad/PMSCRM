CREATE TABLE [dbo].[Role] (
    [role_id]    UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [company_id] UNIQUEIDENTIFIER NOT NULL,
    [name]       NVARCHAR (50)    NOT NULL,
    [timestamp]  SMALLDATETIME    DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([role_id] ASC),
    FOREIGN KEY ([company_id]) REFERENCES [dbo].[Company] ([company_id])
);

