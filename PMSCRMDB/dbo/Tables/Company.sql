CREATE TABLE [dbo].[Company] (
    [company_id] UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [name]       NVARCHAR (50)    NOT NULL,
    [timestamp]  SMALLDATETIME    DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([company_id] ASC)
);

