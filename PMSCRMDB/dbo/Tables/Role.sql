CREATE TABLE [dbo].[Role] (
    [role_id]    UNIQUEIDENTIFIER CONSTRAINT [DF__Role__role_id__68487DD7] DEFAULT (newid()) NOT NULL,
    [company_id] UNIQUEIDENTIFIER NOT NULL,
    [name]       NVARCHAR (50)    NOT NULL,
    [timestamp]  SMALLDATETIME    CONSTRAINT [DF__Role__timestamp__693CA210] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK__Role__760965CC2C29A195] PRIMARY KEY CLUSTERED ([role_id] ASC),
    CONSTRAINT [FK__Role__company_id__6A30C649] FOREIGN KEY ([company_id]) REFERENCES [dbo].[Company] ([company_id])
);

