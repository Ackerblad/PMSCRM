CREATE TABLE [dbo].[Company] (
    [company_id] UNIQUEIDENTIFIER CONSTRAINT [DF__Company__company__49C3F6B7] DEFAULT (newid()) NOT NULL,
    [name]       NVARCHAR (50)    NOT NULL,
    [timestamp]  SMALLDATETIME    CONSTRAINT [DF__Company__timesta__4AB81AF0] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK__Company__3E267235BF4778DE] PRIMARY KEY CLUSTERED ([company_id] ASC)
);

